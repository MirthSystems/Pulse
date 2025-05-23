// Custom type declarations for Auth0 + React Router integration
import { AppState } from "@auth0/auth0-react";

// Add returnTo to the appState type
declare module "@auth0/auth0-react" {
  interface Auth0RedirectState extends AppState {
    returnTo?: string;
  }
}
