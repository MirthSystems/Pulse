import { Theme, createTheme, ThemeOptions } from '@mui/material/styles';
import { PaletteMode } from '@mui/material';
import { blue, teal, red, grey } from '@mui/material/colors';
import { CSSProperties } from 'react'; // Import CSSProperties

// Base theme options that are common to both light and dark modes
const getBaseThemeOptions = (): ThemeOptions => ({ // Add return type ThemeOptions
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
      '"Apple Color Emoji"',
      '"Segoe UI Emoji"',
      '"Segoe UI Symbol"',
    ].join(','),
    h1: {
      fontSize: '3.5rem',
      fontWeight: 700,
      lineHeight: 1.2,
    },
    h2: {
      fontSize: '2.75rem',
      fontWeight: 600,
      lineHeight: 1.2,
    },
    h3: {
      fontSize: '2.25rem',
      fontWeight: 600,
      lineHeight: 1.2,
    },
    h4: {
      fontSize: '1.75rem',
      fontWeight: 600,
      lineHeight: 1.2,
    },
    h5: {
      fontSize: '1.5rem',
      fontWeight: 500,
      lineHeight: 1.2,
    },
    h6: {
      fontSize: '1.25rem',
      fontWeight: 500,
      lineHeight: 1.2,
    },
    button: {
      textTransform: 'none' as CSSProperties['textTransform'], // Explicitly cast to allowed type
      fontWeight: 600,
    },
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 8,
          padding: '8px 16px',
        },
        contained: {
          boxShadow: 'none',
          '&:hover': {
            boxShadow: '0px 4px 8px rgba(0, 0, 0, 0.1)',
          },
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: 12,
          boxShadow: '0px 4px 20px rgba(0, 0, 0, 0.05)',
        },
      },
    },
  },
});

// Get palette configuration based on mode (light/dark)
const getPalette = (mode: PaletteMode) => ({
  mode,
  ...(mode === 'light'
    ? {
        // Light mode palette
        primary: {
          main: blue[700],
        },
        secondary: {
          main: teal[500],
        },
        error: {
          main: red.A400,
        },
        background: {
          default: '#fff',
          paper: '#fff',
        },
        text: {
          primary: grey[900],
          secondary: grey[700],
        },
      }
    : {
        // Dark mode palette
        primary: {
          main: blue[300],
        },
        secondary: {
          main: teal[300],
        },
        error: {
          main: red.A200,
        },
        background: {
          default: '#121212',
          paper: '#1e1e1e',
        },
        text: {
          primary: '#fff',
          secondary: grey[400],
        },
      }),
});

// Create theme with mode
export const getTheme = (mode: PaletteMode): Theme => {
  return createTheme({
    palette: getPalette(mode),
    ...getBaseThemeOptions(),
  });
};

// Default light theme for cases where context isn't needed
export const defaultTheme = getTheme('light');