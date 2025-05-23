import React from "react";
import { HeroUIProvider } from "@heroui/system";
import { Auth0Provider } from "@auth0/auth0-react";
import { useNavigate, useHref } from "react-router";

// This component should only be rendered INSIDE a Router
export function AuthProvider({ children }: { children: React.ReactNode }) {
  const navigate = useNavigate();
  const href = useHref;
  
  const onRedirectCallback = (appState: any) => {
    // Navigate to the route the user was trying to access before authentication
    // or to the home page if no returnTo was specified
    navigate(appState?.returnTo || window.location.pathname);
  };

  return (
    <Auth0Provider
      domain={import.meta.env.VITE_AUTH0_DOMAIN}
      clientId={import.meta.env.VITE_AUTH0_CLIENT_ID}
      authorizationParams={{
        redirect_uri: window.location.origin,
        audience: import.meta.env.VITE_AUTH0_AUDIENCE,
      }}
      onRedirectCallback={onRedirectCallback}
    >
      <HeroUIProvider navigate={navigate} useHref={href}>
        {children}
      </HeroUIProvider>
    </Auth0Provider>
  );
}

// Use this provider for components outside of the Router (like error pages)
// This ensures error pages respect the user's theme preference
export function Provider({ children }: { children: React.ReactNode }) {
  return (
    <HeroUIProvider>
      {children}
    </HeroUIProvider>
  );
}

// Theme-aware wrapper for error pages that preserves user's theme choice
export function ErrorProvider({ children }: { children: React.ReactNode }) {
  React.useEffect(() => {
    // Apply the stored theme to the document element
    const getStoredTheme = () => {
      if (typeof window !== 'undefined') {
        const stored = localStorage.getItem('heroui-theme');
        if (stored) return stored;
        
        // Check system preference
        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
      }
      return 'light';
    };

    const theme = getStoredTheme();
    document.documentElement.className = theme;
  }, []);
  
  return (
    <HeroUIProvider>
      {children}
    </HeroUIProvider>
  );
}
