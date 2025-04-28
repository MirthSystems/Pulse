import { useContext } from 'react';
import AuthContext, { AuthContextType } from '@/contexts/AuthContext';
import { AccountInfo } from '@azure/msal-browser';

/**
 * Custom hook to access the authentication context.
 * Throws if used outside an AuthProvider.
 * @returns AuthContextType
 */
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

/**
 * Custom hook to get only the authentication status.
 * @returns boolean - true if authenticated, false otherwise
 */
export const useIsAuthenticated = (): boolean => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated;
};

/**
 * Custom hook to get the current authenticated account.
 * @returns AccountInfo | null
 */
export const useAccount = (): AccountInfo | null => {
  const { account } = useAuth();
  return account;
};