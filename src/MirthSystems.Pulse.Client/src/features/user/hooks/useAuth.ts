import { useAuth0 } from '@auth0/auth0-react';
import { useCallback, useEffect } from 'react';

/**
 * Custom hook that extends Auth0's useAuth0 hook with additional functionality
 */
export const useAuth = () => {
  const auth0 = useAuth0();
  const { isAuthenticated, getAccessTokenSilently } = auth0;

  /**
   * Get the authentication token for API requests
   */
  const getToken = useCallback(async () => {
    try {
      if (isAuthenticated) {
        const token = await getAccessTokenSilently();
        return token;
      }
      return null;
    } catch (error) {
      console.error('Error getting access token:', error);
      return null;
    }
  }, [isAuthenticated, getAccessTokenSilently]);

  // Store the token in localStorage when authenticated
  useEffect(() => {
    const updateToken = async () => {
      if (isAuthenticated) {
        const token = await getToken();
        if (token) {
          localStorage.setItem('auth_token', token);
        }
      } else {
        localStorage.removeItem('auth_token');
      }
    };

    updateToken();
  }, [isAuthenticated, getToken]);

  return {
    ...auth0,
    getToken,
  };
};