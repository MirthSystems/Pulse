import { Configuration, PopupRequest, RedirectRequest, PublicClientApplication } from "@azure/msal-browser";

// Config object to be passed to Msal on creation
export const msalConfig: Configuration = {
  auth: {
    clientId: process.env.REACT_APP_AUTH_CLIENT_ID || "1ea2773e-e10a-4e8c-b050-14574337ac7e",
    authority: process.env.REACT_APP_AUTH_AUTHORITY || "https://login.microsoftonline.com/common",
    redirectUri: window.location.origin,
    postLogoutRedirectUri: window.location.origin,
  },
  cache: {
    cacheLocation: "sessionStorage", // This configures where your cache will be stored
    storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
  },
  system: {
    allowRedirectInIframe: false, // Avoids redirects within iframes
    loggerOptions: {
      loggerCallback: (level, message, containsPii) => {
        if (containsPii) {
          return;
        }
        switch (level) {
          case 0:
            console.error(message);
            return;
          case 1:
            console.warn(message);
            return;
          case 2:
            console.log(message);
            return;
          case 3:
            console.debug(message);
            return;
          default:
            return;
        }
      }
    }
  }
};

// Add here scopes for token request
export const loginRequest: PopupRequest | RedirectRequest = {
  scopes: ["User.Read"]
};

// Add here the endpoints for MS Graph API services
export const graphConfig = {
  graphMeEndpoint: "https://graph.microsoft.com/v1.0/me",
};

// Create and export the MSAL instance
export const pca = new PublicClientApplication(msalConfig);
