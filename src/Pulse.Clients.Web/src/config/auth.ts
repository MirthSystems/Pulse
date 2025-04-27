import { Configuration } from "@azure/msal-browser";

const msalConfig: Configuration = {
  auth: {
    clientId: import.meta.env.VITE_AUTH_CLIENT_ID,
    authority: import.meta.env.VITE_AUTH_AUTHORITY,
    redirectUri: window.location.origin,
  },
  cache: {
    cacheLocation: "localStorage",
    storeAuthStateInCookie: false,
  },
};

const loginRequest = {
  scopes: ["openid", "profile", "User.Read"],
};

export { msalConfig, loginRequest };
