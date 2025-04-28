import { createContext } from 'react';
import { 
  AuthenticationResult, 
  AccountInfo, 
  RedirectRequest,
  PopupRequest,
  SilentRequest
} from '@azure/msal-browser';

/**
 * Type for authentication context value.
 * Provides authentication state, account info, error/loading state, and auth methods.
 */
export type AuthContextType = {
  /** Indicates if the user is authenticated. */
  isAuthenticated: boolean;
  /** Indicates if authentication is in progress. */
  isLoading: boolean;
  /** The current authenticated account, or null if not signed in. */
  account: AccountInfo | null;
  /** Any authentication error encountered. */
  error: Error | null;
  /** User roles for authorization checks. */
  userRoles: string[];
  /** Indicates if roles are still being loaded. */
  isLoadingRoles: boolean;
  /** Initiates login using redirect flow. */
  loginRedirect: (request?: RedirectRequest) => Promise<void>;
  /** Initiates login using popup flow. */
  loginPopup: (request?: PopupRequest) => Promise<AuthenticationResult | null>;
  /** Logs out the current user. */
  logout: (postLogoutRedirectUri?: string) => Promise<void>;
  /** Acquires an access token silently for the current account. */
  acquireToken: (request?: SilentRequest) => Promise<string | null>;
  /** Clears any authentication error in context. */
  clearError: () => void;
};

/**
 * React context for authentication state and actions.
 * Provides authentication status, account info, and auth methods to consumers.
 */
const AuthContext = createContext<AuthContextType | undefined>(undefined);

export default AuthContext;