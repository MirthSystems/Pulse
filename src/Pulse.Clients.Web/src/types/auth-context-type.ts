import { 
  AuthenticationResult, 
  AccountInfo, 
  RedirectRequest,
  PopupRequest,
  SilentRequest
} from '@azure/msal-browser';

export interface AuthContextType {
  isAuthenticated: boolean;
  isLoading: boolean;
  account: AccountInfo | null;
  error: Error | null;
  
  loginRedirect: (request?: RedirectRequest) => Promise<void>;
  loginPopup: (request?: PopupRequest) => Promise<AuthenticationResult | null>;
  logout: (postLogoutRedirectUri?: string) => Promise<void>;
  
  acquireToken: (request?: SilentRequest) => Promise<string | null>;
  
  clearError: () => void;
}