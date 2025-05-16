import { type ThemeOptions } from '@mui/material/styles';

export const darkThemeOptions: ThemeOptions = {
  palette: {
    mode: 'dark',
    primary: {
      main: '#00E7F2', // Brighter aqua blue neon for dark mode
      light: '#5CF9FF',
      dark: '#00B3BC',
      contrastText: '#000000',
    },
    secondary: {
      main: '#FF9255', // Brighter orange tangerine sunset for dark mode
      light: '#FFB385',
      dark: '#E56B25',
      contrastText: '#000000',
    },
    background: {
      default: '#0D1217', // Dark blue-black
      paper: '#161C23', // Slightly lighter dark blue
    },
    text: {
      primary: 'rgba(255, 255, 255, 0.95)',
      secondary: 'rgba(255, 255, 255, 0.7)',
    },
    divider: 'rgba(0, 231, 242, 0.15)', // Subtle neon blue divider
    info: {
      main: '#00E7F2', // Matching primary for info
    },
    success: {
      main: '#4CD964', // Bright green
    },
    error: {
      main: '#FF453A', // Bright red
    },
    warning: {
      main: '#FF9255', // Matching secondary for warning
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
            boxShadow: '0 4px 15px rgba(0, 231, 242, 0.25)',
          },
        },
        containedPrimary: {
          backgroundImage: 'linear-gradient(135deg, #00E7F2 0%, #5CF9FF 100%)',
          color: '#000000',
          '&:hover': {
            backgroundImage: 'linear-gradient(135deg, #00F5FF 0%, #7FFFFF 100%)',
          }
        },
        containedSecondary: {
          backgroundImage: 'linear-gradient(135deg, #FF9255 0%, #FFB385 100%)',
          color: '#000000',
          '&:hover': {
            backgroundImage: 'linear-gradient(135deg, #FFA06C 0%, #FFC19E 100%)',
          }
        },
        outlined: {
          borderWidth: '2px',
          '&:hover': {
            borderWidth: '2px',
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
              borderColor: '#00E7F2',
            },
            '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
              borderColor: '#00E7F2',
              borderWidth: '2px',
            }
          },
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          borderRadius: 12,
          backgroundImage: 'none',
        },
        elevation1: {
          boxShadow: '0px 3px 15px rgba(0, 0, 0, 0.3)',
        },
        elevation2: {
          boxShadow: '0px 5px 20px rgba(0, 0, 0, 0.4)',
        },
      },
    },
    MuiSlider: {
      styleOverrides: {
        root: {
          height: 8,
          '& .MuiSlider-track': {
            backgroundImage: 'linear-gradient(90deg, #00E7F2 0%, #5CF9FF 100%)',
          },
          '& .MuiSlider-thumb': {
            width: 18,
            height: 18,
            backgroundColor: '#0D1217',
            border: '2px solid #00E7F2',
            boxShadow: '0px 2px 8px rgba(0, 231, 242, 0.5)',
            '&:focus, &:hover, &.Mui-active': {
              boxShadow: '0px 3px 10px rgba(0, 231, 242, 0.7)',
            },
          },
        },
      },
    },
    MuiSwitch: {
      styleOverrides: {
        switchBase: {
          color: '#555555',
          '&.Mui-checked': {
            color: '#00E7F2',
          },
          '&.Mui-checked + .MuiSwitch-track': {
            backgroundColor: '#00B3BC',
            opacity: 0.9,
          },
        },
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          background: 'linear-gradient(90deg, #161C23 0%, #1A232D 100%)',
          boxShadow: '0px 3px 10px rgba(0, 0, 0, 0.2)',
        },
      },
    },
    MuiChip: {
      styleOverrides: {
        root: {
          borderRadius: 8,
        },
        colorPrimary: {
          backgroundImage: 'linear-gradient(135deg, rgba(0, 231, 242, 0.9) 0%, rgba(92, 249, 255, 0.9) 100%)',
          color: '#000000',
        },
        colorSecondary: {
          backgroundImage: 'linear-gradient(135deg, rgba(255, 146, 85, 0.9) 0%, rgba(255, 179, 133, 0.9) 100%)',
          color: '#000000',
        },
      },
    },
  },
};