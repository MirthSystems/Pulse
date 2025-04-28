import { createContext } from 'react';
import { AuthContextType } from '../types/auth-context-type';

/**
 * React context for authentication state and actions.
 * Provides authentication status, account info, and auth methods to consumers.
 * @see AuthContextType for context value shape
 */
export const AuthContext = createContext<AuthContextType | undefined>(undefined);
