import { Configuration, PublicClientApplication } from "@azure/msal-browser";

// Auth configuration
export const msalConfig: Configuration = {
  auth: {
    clientId: import.meta.env.VITE_AUTH_CLIENT_ID || "1ea2773e-e10a-4e8c-b050-14574337ac7e",
    authority: import.meta.env.VITE_AUTH_AUTHORITY || "https://login.microsoftonline.com/common",
    redirectUri: window.location.origin,
    postLogoutRedirectUri: window.location.origin,
  },
  cache: {
    cacheLocation: "localStorage",
    storeAuthStateInCookie: true,
  },
  system: {
    allowRedirectInIframe: false,
    loggerOptions: {
      loggerCallback: (level, message, containsPii) => {
        if (containsPii) {
          return;
        }
        
        switch (level) {
          case 0: // Error
            console.error(message);
            break;
          case 1: // Warning
            console.warn(message);
            break;
          case 2: // Info
            console.info(message);
            break;
          case 3: // Verbose
            console.debug(message);
            break;
          default:
            break;
        }
      }
    }
  }
};

// Login request configuration
export const loginRequest = {
  scopes: (import.meta.env.VITE_MICROSOFT_GRAPH_SCOPES || "user.read").split(",")
};

// Microsoft Graph API endpoints
export const graphConfig = {
  graphEndpoint: `${import.meta.env.VITE_MICROSOFT_GRAPH_DOMAIN || "https://graph.microsoft.com/"}${import.meta.env.VITE_MICROSOFT_GRAPH_VERSION || "v1.0"}`,
  graphMeEndpoint: `${import.meta.env.VITE_MICROSOFT_GRAPH_DOMAIN || "https://graph.microsoft.com/"}${import.meta.env.VITE_MICROSOFT_GRAPH_VERSION || "v1.0"}/me`
};

// Backend API configuration
export const apiConfig = {
  apiUrl: import.meta.env.VITE_PULSE_API_URL || "http://localhost:3000/api",
  apiScopes: (import.meta.env.VITE_PULSE_API_SCOPES || "api://20e5aada-0b67-4db5-9646-1b0316b2a242/access_as_user").split(",")
};

// Initialize the MSAL application instance
export const pca = new PublicClientApplication(msalConfig);
