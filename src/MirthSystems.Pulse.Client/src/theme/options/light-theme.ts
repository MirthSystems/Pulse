import { type ThemeOptions } from '@mui/material/styles';

export const lightThemeOptions: ThemeOptions = {
  palette: {
    mode: 'light',
    primary: {
      main: '#00C2CB', // Aqua blue neon
      light: '#47E5ED',
      dark: '#009BA3',
      contrastText: '#ffffff',
    },
    secondary: {
      main: '#FF7E45', // Orange tangerine sunset
      light: '#FFA27A',
      dark: '#D35A2A',
      contrastText: '#ffffff',
    },
    background: {
      default: '#ffffff',
      paper: '#f9f9f9',
    },
    text: {
      primary: '#213547',
      secondary: '#666666',
    },
    divider: 'rgba(0, 194, 203, 0.1)', // Light aqua blue for dividers
    info: {
      main: '#00C2CB', // Matching primary for info
    },
    success: {
      main: '#4CD964', // Bright green
    },
    error: {
      main: '#FF3B30', // Bright red
    },
    warning: {
      main: '#FF7E45', // Matching secondary for warning
    }
  },
  typography: {
    fontFamily: '"Inter", "Roboto", "system-ui", "Helvetica", "Arial", sans-serif',
    h1: {
      fontSize: '3.2rem',
      lineHeight: 1.1,
      fontWeight: 700,
      letterSpacing: '-0.02em',
    },
    h2: {
      fontWeight: 700,
      letterSpacing: '-0.01em',
    },
    h3: {
      fontWeight: 700,
      letterSpacing: '-0.01em',
    },
    h4: {
      fontWeight: 600,
    },
    h5: {
      fontWeight: 600,
    },
    h6: {
      fontWeight: 600,
    },
    button: {
      fontWeight: 600,
      letterSpacing: '0.02em',
    },
    subtitle1: {
      letterSpacing: '0.01em',
    },
    subtitle2: {
      fontWeight: 500,
    }
  },
  shape: {
    borderRadius: 12
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 10,
          textTransform: 'none',
          fontWeight: 600,
          transition: 'all 0.2s ease-in-out',
          boxShadow: 'none',
          '&:hover': {
            transform: 'translateY(-2px)',
            boxShadow: '0 4px 12px rgba(0, 194, 203, 0.25)',
          },
        },
        containedPrimary: {
          backgroundImage: 'linear-gradient(135deg, #00C2CB 0%, #47E5ED 100%)',
          '&:hover': {
            backgroundImage: 'linear-gradient(135deg, #00D6E0 0%, #5CEFF7 100%)',
          }
        },
        containedSecondary: {
          backgroundImage: 'linear-gradient(135deg, #FF7E45 0%, #FFA27A 100%)',
          '&:hover': {
            backgroundImage: 'linear-gradient(135deg, #FF8D5A 0%, #FFAE8E 100%)',
          }
        },
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: {
          '& .MuiOutlinedInput-root': {
            borderRadius: 10,
            '&:hover .MuiOutlinedInput-notchedOutline': {
              borderColor: '#00C2CB',
            },
          },
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          borderRadius: 12,
        },
        elevation1: {
          boxShadow: '0px 3px 15px rgba(0, 0, 0, 0.05)',
        },
        elevation2: {
          boxShadow: '0px 5px 20px rgba(0, 0, 0, 0.08)',
        },
      },
    },
    MuiSlider: {
      styleOverrides: {
        root: {
          height: 8,
          '& .MuiSlider-track': {
            backgroundImage: 'linear-gradient(90deg, #00C2CB 0%, #47E5ED 100%)',
          },
          '& .MuiSlider-thumb': {
            width: 18,
            height: 18,
            backgroundColor: '#ffffff',
            border: '2px solid #00C2CB',
            boxShadow: '0px 2px 6px rgba(0, 194, 203, 0.3)',
            '&:focus, &:hover, &.Mui-active': {
              boxShadow: '0px 3px 8px rgba(0, 194, 203, 0.5)',
            },
          },
        },
      },
    },
    MuiSwitch: {
      styleOverrides: {
        switchBase: {
          color: '#cccccc',
          '&.Mui-checked': {
            color: '#00C2CB',
          },
          '&.Mui-checked + .MuiSwitch-track': {
            backgroundColor: '#47E5ED',
            opacity: 0.8,
          },
        },
      },
    },
  },
};