import { useAuth0 } from '@auth0/auth0-react';
import { useCallback, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { apiClient } from '../../api/client';
import { setAuthenticated, setToken, setUserInfo } from '../redux/userSlice';
import { jwtDecode } from 'jwt-decode';

interface JwtTokenPayload {
  sub: string;
  name?: string;
  email?: string;
  'https://pulse.mirth-systems.com/roles'?: string[];
  exp?: number; // Expiration time
  iat?: number; // Issued at time
  [key: string]: unknown;
}

/**
 * Custom hook that extends Auth0's useAuth0 hook with additional functionality
 * and integrates with Redux store and API client
 */
export const useAuth = () => {
  const auth0 = useAuth0();
  const { isAuthenticated, getAccessTokenSilently } = auth0;
  const dispatch = useAppDispatch();
  const { token, lastTokenRefresh } = useAppSelector(state => state.user);
  
  /**
   * Get the authentication token for API requests and store in Redux
   */
  const getToken = useCallback(async () => {
    try {
      if (isAuthenticated) {
        const token = await getAccessTokenSilently();
        
        // Store the token in Redux
        dispatch(setToken(token));
        
        // Set the token in our API client
        apiClient.setAuthToken(token);
        
        // Store token in localStorage for persistence across refreshes
        localStorage.setItem('auth_token', token);
        
        return token;
      }
      return null;
    } catch (error) {
      console.error('Error getting access token:', error);
      return null;
    }
  }, [isAuthenticated, getAccessTokenSilently, dispatch]);

  /**
   * Extract user info from token and update Redux state
   */
  const updateUserInfo = useCallback((token: string) => {
    try {
      const decodedToken = jwtDecode<JwtTokenPayload>(token);
      dispatch(setUserInfo({
        userId: decodedToken.sub,
        userName: decodedToken.name || null,
        userEmail: decodedToken.email || null,
        userRoles: decodedToken['https://pulse.mirth-systems.com/roles'] || [],
      }));
    } catch (error) {
      console.error('Error decoding token:', error);
    }
  }, [dispatch]);

  /**
   * Get token expiration time in milliseconds
   */
  const getTokenExpirationTime = useCallback((token: string): number => {
    try {
      const decodedToken = jwtDecode<JwtTokenPayload>(token);
      return decodedToken.exp ? decodedToken.exp * 1000 : 0; // Convert to milliseconds
    } catch (error) {
      console.error('Error decoding token expiration:', error);
      return 0;
    }
  }, []);

  // Effect to update auth state when Auth0 state changes
  useEffect(() => {
    dispatch(setAuthenticated(isAuthenticated));
    
    const updateToken = async () => {
      if (isAuthenticated) {
        const newToken = await getToken();
        if (newToken) {
          updateUserInfo(newToken);
        }
      } else {
        // Clear token on logout
        localStorage.removeItem('auth_token');
        apiClient.clearAuthToken();
      }
    };

    updateToken();
  }, [isAuthenticated, getToken, dispatch, updateUserInfo]);

  // Check if token needs refreshing based on actual token expiration
  useEffect(() => {
    const refreshTokenIfNeeded = async () => {
      if (token) {
        const now = Date.now();
        const expirationTime = getTokenExpirationTime(token);
        
        if (expirationTime) {
          // Refresh token 5 minutes (300000 ms) before it expires
          const bufferTime = 5 * 60 * 1000;
          
          if (now + bufferTime > expirationTime) {
            console.log('Token refresh needed - expiring soon');
            await getToken();
          }
        } else if (lastTokenRefresh) {
          // Fallback to time-based refresh if expiration cannot be determined
          const refreshThreshold = 45 * 60 * 1000; // 45 minutes
          if (now - lastTokenRefresh > refreshThreshold) {
            console.log('Token refresh needed - time-based fallback');
            await getToken();
          }
        }
      }
    };
    
    refreshTokenIfNeeded();
  }, [token, lastTokenRefresh, getToken, getTokenExpirationTime]);

  return {
    ...auth0,
    getToken,
  };
};