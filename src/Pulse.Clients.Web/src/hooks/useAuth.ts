import { useContext } from 'react';
import { AccountInfo } from '@azure/msal-browser';
import { AuthContextType } from '../types/auth-context-type';
import { AuthContext } from '../contexts/auth-context';

// Custom hook to use the auth context
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

// Hook for just checking authentication status
export const useIsAuthenticated = (): boolean => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated;
};

// Hook for just getting the current account
export const useAccount = (): AccountInfo | null => {
  const { account } = useAuth();
  return account;
};