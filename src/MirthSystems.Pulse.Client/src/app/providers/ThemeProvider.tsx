import { ReactNode, useMemo } from 'react';
import { CssBaseline, Theme, ThemeProvider as MuiThemeProvider, createTheme } from '@mui/material';
import { useAppSelector } from '../hooks';

// Create a context to provide theme information to components
export interface ThemeContextProps {
  theme: Theme;
}

interface AppThemeProviderProps {
  children: ReactNode;
}

// Define our custom theme with light and dark mode color palettes
const getTheme = (mode: 'light' | 'dark') => createTheme({
  palette: {
    mode,
    primary: {
      main: '#1976d2', // Blue primary color
    },
    secondary: {
      main: '#dc004e', // Pink secondary color
    },
    background: {
      default: mode === 'light' ? '#f5f5f5' : '#121212',
      paper: mode === 'light' ? '#ffffff' : '#1e1e1e',
    },
    text: {
      primary: mode === 'light' ? 'rgba(0, 0, 0, 0.87)' : 'rgba(255, 255, 255, 0.87)',
      secondary: mode === 'light' ? 'rgba(0, 0, 0, 0.6)' : 'rgba(255, 255, 255, 0.6)',
    },
  },
  typography: {
    fontFamily: [
      'Inter',
      '-apple-system',
      'BlinkMacSystemFont',
      '"Segoe UI"',
      'Roboto',
      '"Helvetica Neue"',
      'Arial',
      'sans-serif',
    ].join(','),
    h1: {
      fontWeight: 700,
    },
    h2: {
      fontWeight: 600,
    },
    h3: {
      fontWeight: 600,
    },
  },
  shape: {
    borderRadius: 8,
  },
  components: {
    MuiAppBar: {
      defaultProps: {
        elevation: 0,
      },
      styleOverrides: {
        root: {
          backgroundColor: mode === 'light' ? '#1976d2' : '#272727',
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: 'none',
          fontWeight: 600,
        },
      },
    },
    MuiCard: {
      defaultProps: {
        elevation: mode === 'light' ? 1 : 2,
      },
    },
    MuiPaper: {
      defaultProps: {
        elevation: 0,
      },
      styleOverrides: {
        root: {
          backgroundImage: 'none',
        },
      },
    },
  },
});

export const AppThemeProvider = ({ children }: AppThemeProviderProps) => {
  // Get the theme mode from Redux store
  const { mode } = useAppSelector((state) => state.theme);

  // Create theme based on current mode
  const theme = useMemo(() => getTheme(mode), [mode]);

  return (
      <MuiThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </MuiThemeProvider>
  );
};