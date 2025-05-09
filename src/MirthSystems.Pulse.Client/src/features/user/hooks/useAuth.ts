import { useCallback, useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';

// Interface for Auth0 user profile
export interface UserProfile {
  name?: string;
  email?: string;
  picture?: string;
  nickname?: string;
  email_verified?: boolean;
  updated_at?: string;
  [key: string]: unknown;
}

export const useAuth = () => {
  const { 
    user: auth0User, 
    isAuthenticated, 
    loginWithRedirect: auth0Login, 
    logout: auth0Logout,
    getAccessTokenSilently 
  } = useAuth0();
  const [userMetadata, setUserMetadata] = useState<unknown>(null);
  
  // Combine Auth0 user with user metadata
  const user: UserProfile | null = auth0User || null;

  // Fetch user metadata including roles
  useEffect(() => {
    if (!isAuthenticated || !auth0User?.sub) return;

    const getUserMetadata = async () => {
      try {
        const accessToken = await getAccessTokenSilently({
          authorizationParams: {
            audience: `https://${process.env.REACT_APP_AUTH0_DOMAIN || 'mithsystems-pulse.us.auth0.com'}/api/v2/`,
            scope: "read:current_user",
          },
        });

        const domain = process.env.REACT_APP_AUTH0_DOMAIN || 'mithsystems-pulse.us.auth0.com';
        const userDetailsByIdUrl = `https://${domain}/api/v2/users/${auth0User.sub}`;
        
        const metadataResponse = await fetch(userDetailsByIdUrl, {
          headers: {
            Authorization: `Bearer ${accessToken}`,
          },
        });

        const userData = await metadataResponse.json();
        setUserMetadata(userData);
      } catch (error) {
        console.error('Error fetching user metadata:', error);
      }
    };

    getUserMetadata();
  }, [getAccessTokenSilently, auth0User?.sub, isAuthenticated]);

  // Check for admin role (System.Administrator)
  const isAdmin = useCallback(() => {
    if (!isAuthenticated) return false;
    
    // Check in Auth0 roles or app_metadata
    const roles = 
      userMetadata?.app_metadata?.authorization?.roles || 
      userMetadata?.roles ||
      auth0User?.['https://mirth.com/roles'] || [];
    
    return Array.isArray(roles) && roles.includes('System.Administrator');
  }, [auth0User, userMetadata, isAuthenticated]);

  // Check for venue manager role (Content.Manager)
  const isVenueManager = useCallback(() => {
    if (!isAuthenticated) return false;
    
    // Check in Auth0 roles or app_metadata
    const roles = 
      userMetadata?.app_metadata?.authorization?.roles || 
      userMetadata?.roles ||
      auth0User?.['https://mirth.com/roles'] || [];
    
    return Array.isArray(roles) && roles.includes('Content.Manager');
  }, [auth0User, userMetadata, isAuthenticated]);

  // Login with redirect using Auth0
  const loginWithRedirect = async () => {
    await auth0Login();
  };

  // Logout using Auth0
  const logout = (options?: { returnTo?: string }) => {
    auth0Logout({ 
      logoutParams: {
        returnTo: options?.returnTo || window.location.origin
      }
    });
  };

  return {
    isAuthenticated,
    isAdmin,
    isVenueManager,
    user,
    userMetadata,
    loginWithRedirect,
    logout,
    getAccessTokenSilently
  };
};