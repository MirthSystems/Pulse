import React, { useState, useEffect, ReactNode } from 'react';
import { 
  AuthenticationResult, 
  AccountInfo, 
  InteractionStatus, 
  RedirectRequest,
  PopupRequest,
  SilentRequest
} from '@azure/msal-browser';
import { useMsal, useIsAuthenticated as useMsalIsAuthenticated } from '@azure/msal-react';
import { loginRequest } from '../../configs/auth';
import { AuthContextType } from '../../types/auth-context-type';
import { AuthContext } from '../auth-context';

// Provider props
interface AuthProviderProps {
  children: ReactNode;
}

// Provider component
export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const { instance, accounts, inProgress } = useMsal();
  const isAuthenticated = useMsalIsAuthenticated();
  const [account, setAccount] = useState<AccountInfo | null>(null);
  const [error, setError] = useState<Error | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  // Update account when auth state changes
  useEffect(() => {
    const activeAccount = instance.getActiveAccount();
    if (activeAccount) {
      setAccount(activeAccount);
    } else if (accounts.length > 0) {
      setAccount(accounts[0]);
      instance.setActiveAccount(accounts[0]);
    } else {
      setAccount(null);
    }
  }, [instance, accounts, isAuthenticated]);

  // Clear loading state when interaction completes
  useEffect(() => {
    if (inProgress !== InteractionStatus.None) {
      setIsLoading(true);
    } else {
      setIsLoading(false);
    }
  }, [inProgress]);

  // Login with redirect
  const loginRedirect = async (request?: RedirectRequest) => {
    setError(null);
    try {
      await instance.loginRedirect(request || loginRequest);
    } catch (err) {
      setError(err as Error);
      throw err;
    }
  };

  // Login with popup
  const loginPopup = async (request?: PopupRequest): Promise<AuthenticationResult | null> => {
    setError(null);
    setIsLoading(true);
    try {
      const response = await instance.loginPopup(request || loginRequest);
      return response;
    } catch (err) {
      setError(err as Error);
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  // Logout
  const logout = async (postLogoutRedirectUri?: string) => {
    setError(null);
    try {
      await instance.logoutRedirect({
        postLogoutRedirectUri: postLogoutRedirectUri || window.location.origin
      });
    } catch (err) {
      setError(err as Error);
      throw err;
    }
  };

  // Acquire token silently
  const acquireToken = async (request?: SilentRequest): Promise<string | null> => {
    setError(null);
    try {
      const activeAccount = instance.getActiveAccount() || (accounts.length > 0 ? accounts[0] : null);
      if (!activeAccount) {
        throw new Error('No active account! Sign in before acquiring a token.');
      }
      
      const tokenRequest = {
        ...loginRequest,
        account: activeAccount,
        ...request
      };
      
      const response = await instance.acquireTokenSilent(tokenRequest);
      return response.accessToken;
    } catch (err) {
      setError(err as Error);
      return null;
    }
  };

  // Clear any error
  const clearError = () => {
    setError(null);
  };

  // Context value
  const contextValue: AuthContextType = {
    isAuthenticated,
    isLoading,
    account,
    error,
    loginRedirect,
    loginPopup,
    logout,
    acquireToken,
    clearError
  };

  return (
    <AuthContext.Provider value={contextValue}>
      {children}
    </AuthContext.Provider>
  );
};