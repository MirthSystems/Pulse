import { ReactNode } from 'react';
import { Auth0Provider } from '@auth0/auth0-react';

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const domain = import.meta.env.VITE_AUTH0_DOMAIN || '';
  const clientId = import.meta.env.VITE_AUTH0_CLIENT_ID || '';
  const redirectUri = import.meta.env.VITE_AUTH0_CALLBACK_URL || window.location.origin;
  const audience = import.meta.env.VITE_AUTH0_AUDIENCE || '';

  if (!domain || !clientId) {
    console.warn('Auth0 domain or client ID is missing. Authentication will not work properly.');
    // Return children without Auth0 provider to avoid breaking the app
    return <>{children}</>;
  }

  return (
    <Auth0Provider
      domain={domain}
      clientId={clientId}
      authorizationParams={{
        redirect_uri: redirectUri,
        audience: audience,
      }}
    >
      {children}
    </Auth0Provider>
  );
};