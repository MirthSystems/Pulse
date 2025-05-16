import { type ThemeOptions } from '@mui/material/styles';

export const darkThemeOptions: ThemeOptions = {
  palette: {
    mode: 'dark',
    primary: {
      main: '#646cff',
      light: '#747bff',
      dark: '#535bf2',
    },
    secondary: {
      main: '#888',
    },
    background: {
      default: '#242424',
      paper: '#1a1a1a',
    },
    text: {
      primary: 'rgba(255, 255, 255, 0.87)',
      secondary: '#888',
    },
  },
  typography: {
    fontFamily: '"system-ui", "Avenir", "Helvetica", "Arial", sans-serif',
    h1: {
      fontSize: '3.2em',
      lineHeight: 1.1,
    },
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 8,
          textTransform: 'none',
          fontWeight: 500,
          transition: 'border-color 0.25s',
          '&:hover': {
            borderColor: '#646cff',
          },
        },
      },
    },
  },
};