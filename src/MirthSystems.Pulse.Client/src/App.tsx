import { CssBaseline, ThemeProvider } from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterLuxon } from '@mui/x-date-pickers/AdapterLuxon';
import { Auth0Provider } from '@auth0/auth0-react';
import { AppRouter } from './router';
import { useThemeStore } from './store';
import { lightTheme, darkTheme } from './theme';

function App() {
  const { isDarkMode } = useThemeStore();
  const theme = isDarkMode ? darkTheme : lightTheme;

  const domain = import.meta.env.VITE_AUTH0_DOMAIN as string;
  const clientId = import.meta.env.VITE_AUTH0_CLIENT_ID as string;
  const audience = import.meta.env.VITE_AUTH0_AUDIENCE as string;
  const redirectUri = `${window.location.origin}/callback`;

  return (
    <Auth0Provider
      domain={domain}
      clientId={clientId}
      authorizationParams={{
        redirect_uri: redirectUri,
        audience: audience,
      }}
    >
      <ThemeProvider theme={theme}>
        <LocalizationProvider dateAdapter={AdapterLuxon}>
          <CssBaseline />
          <AppRouter />
        </LocalizationProvider>
      </ThemeProvider>
    </Auth0Provider>
  );
}

export default App;
