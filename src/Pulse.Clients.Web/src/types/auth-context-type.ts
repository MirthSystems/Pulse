import { AuthenticationResult, AccountInfo, RedirectRequest, PopupRequest, SilentRequest } from '@azure/msal-browser';

/**
 * Interface for authentication context value.
 * Provides authentication state, account info, error/loading state, and auth methods.
 */
export interface AuthContextType {
  /** Indicates if the user is authenticated. */
  isAuthenticated: boolean;
  /** Indicates if authentication is in progress. */
  isLoading: boolean;
  /** The current authenticated account, or null if not signed in. */
  account: AccountInfo | null;
  /** Any authentication error encountered. */
  error: Error | null;
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
}