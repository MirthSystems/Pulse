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

/**
 * Props for the AuthProvider component.
 * @property children - React children to be wrapped by the auth provider.
 */
interface AuthProviderProps {
  children: ReactNode;
}

/**
 * Provides authentication context and MSAL integration to the application.
 * Handles login, logout, token acquisition, and account state.
 * @param props - AuthProviderProps
 * @returns JSX.Element
 */
export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const { instance, accounts, inProgress } = useMsal();
  const isAuthenticated = useMsalIsAuthenticated();
  const [account, setAccount] = useState<AccountInfo | null>(null);
  const [error, setError] = useState<Error | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

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

  useEffect(() => {
    if (inProgress !== InteractionStatus.None) {
      setIsLoading(true);
    } else {
      setIsLoading(false);
    }
  }, [inProgress]);

  /**
   * Initiates login using redirect flow.
   * @param request Optional redirect request configuration.
   */
  const loginRedirect = async (request?: RedirectRequest) => {
    setError(null);
    try {
      await instance.loginRedirect(request || loginRequest);
    } catch (err) {
      setError(err as Error);
      throw err;
    }
  };

  /**
   * Initiates login using popup flow.
   * @param request Optional popup request configuration.
   * @returns AuthenticationResult or null
   */
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

  /**
   * Logs out the current user.
   * @param postLogoutRedirectUri Optional URI to redirect to after logout.
   */
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

  /**
   * Acquires an access token silently for the current account.
   * @param request Optional silent request configuration.
   * @returns Access token string or null
   */
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

  /**
   * Clears any authentication error in context.
   */
  const clearError = () => {
    setError(null);
  };

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