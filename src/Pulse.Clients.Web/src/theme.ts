import { Theme, createTheme, ThemeOptions, responsiveFontSizes, Components } from '@mui/material/styles';
import { PaletteMode, PaletteOptions } from '@mui/material';
import { blue, teal, red, grey, amber } from '@mui/material/colors';
import { CSSProperties } from 'react';

// Define component overrides that work for both light and dark modes
const getComponentOverrides = (mode: PaletteMode): Components<Omit<Theme, "components">> => ({
  MuiCssBaseline: {
    styleOverrides: {
      body: {
        scrollbarWidth: 'thin',
        '&::-webkit-scrollbar': {
          width: '8px',
          height: '8px',
        },
        '&::-webkit-scrollbar-track': {
          background: mode === 'light' ? '#f1f1f1' : '#292929',
        },
        '&::-webkit-scrollbar-thumb': {
          background: mode === 'light' ? '#c1c1c1' : '#6b6b6b',
          borderRadius: '4px',
        },
        '&::-webkit-scrollbar-thumb:hover': {
          background: mode === 'light' ? '#a8a8a8' : '#848484',
        },
      },
    },
  },
  MuiButton: {
    styleOverrides: {
      root: {
        borderRadius: 8,
        padding: '8px 16px',
        textTransform: 'none',
        fontWeight: 600,
        // Improve focus visibility for accessibility
        '&.Mui-focusVisible': {
          boxShadow: `0 0 0 3px ${mode === 'light' ? 'rgba(0, 123, 255, 0.4)' : 'rgba(77, 171, 245, 0.5)'}`,
        },
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
        boxShadow: mode === 'light' 
          ? '0px 4px 20px rgba(0, 0, 0, 0.05)'
          : '0px 4px 20px rgba(0, 0, 0, 0.2)',
      },
    },
  },
  MuiDialog: {
    styleOverrides: {
      paper: {
        borderRadius: 12,
      },
    },
  },
  MuiTextField: {
    styleOverrides: {
      root: {
        '& .MuiOutlinedInput-root': {
          borderRadius: 8,
        },
      },
    },
  },
  MuiTableCell: {
    styleOverrides: {
      root: {
        padding: '16px',
      },
      head: {
        fontWeight: 600,
        backgroundColor: mode === 'light' ? '#f5f5f5' : '#333',
      },
    },
  },
  MuiTab: {
    styleOverrides: {
      root: {
        textTransform: 'none',
        fontWeight: 500,
        '&.Mui-selected': {
          fontWeight: 600,
        },
      },
    },
  },
  MuiChip: {
    styleOverrides: {
      root: {
        borderRadius: 6,
      },
    },
  },
  MuiTooltip: {
    styleOverrides: {
      tooltip: {
        borderRadius: 6,
        fontSize: '0.8rem',
      },
    },
  },
  // Improve focus styles for interactive elements
  MuiIconButton: {
    styleOverrides: {
      root: {
        '&.Mui-focusVisible': {
          outline: `2px solid ${mode === 'light' ? blue[600] : blue[300]}`,
          outlineOffset: 2,
        },
      },
    },
  },
  MuiMenuItem: {
    styleOverrides: {
      root: {
        '&.Mui-focusVisible': {
          backgroundColor: mode === 'light' ? 'rgba(0, 0, 0, 0.08)' : 'rgba(255, 255, 255, 0.12)',
        },
      },
    },
  },
});

// Define typography settings that work for both modes
const getTypographyOptions = () => ({
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
    textTransform: 'none' as CSSProperties['textTransform'],
    fontWeight: 600,
  },
  body1: {
    fontSize: '1rem',
    lineHeight: 1.5,
  },
  body2: {
    fontSize: '0.875rem',
    lineHeight: 1.5,
  },
});

// Get palette configuration based on mode (light/dark)
const getPalette = (mode: PaletteMode): PaletteOptions => {
  // Common palette values for both modes
  const commonPalette = {
    mode,
    primary: {
      main: blue[700],
      ...(mode === 'dark' && { main: blue[400] }),
    },
    secondary: {
      main: teal[500],
      ...(mode === 'dark' && { main: teal[300] }),
    },
    error: {
      main: red[600],
      ...(mode === 'dark' && { main: red[400] }),
    },
    warning: {
      main: amber[700],
      ...(mode === 'dark' && { main: amber[500] }),
    },
    info: {
      main: blue[600],
      ...(mode === 'dark' && { main: blue[400] }),
    },
    success: {
      main: teal[600],
      ...(mode === 'dark' && { main: teal[400] }),
    },
  };

  if (mode === 'light') {
    return {
      ...commonPalette,
      background: {
        default: '#fff',
        paper: '#fff',
      },
      text: {
        primary: grey[900],
        secondary: grey[700],
        disabled: grey[500],
      },
      divider: 'rgba(0, 0, 0, 0.12)',
    };
  }

  // Dark mode specific palette
  return {
    ...commonPalette,
    background: {
      default: '#121212',
      paper: '#1e1e1e',
    },
    text: {
      primary: '#fff',
      secondary: grey[400],
      disabled: grey[600],
    },
    divider: 'rgba(255, 255, 255, 0.12)',
    action: {
      active: '#fff',
      hover: 'rgba(255, 255, 255, 0.08)',
      selected: 'rgba(255, 255, 255, 0.16)',
      disabled: 'rgba(255, 255, 255, 0.3)',
      disabledBackground: 'rgba(255, 255, 255, 0.12)',
    },
  };
};

// Create theme with mode
export const getTheme = (mode: PaletteMode): Theme => {
  const baseThemeOptions: ThemeOptions = {
    palette: getPalette(mode),
    typography: getTypographyOptions(),
    components: getComponentOverrides(mode),
    shape: {
      borderRadius: 8,
    },
    // Improve spacing for a more balanced look
    spacing: (factor: number) => `${0.25 * factor}rem`,
  };

  // Create the base theme
  let theme = createTheme(baseThemeOptions);
  
  // Apply responsive font sizes for better mobile experience
  theme = responsiveFontSizes(theme, {
    breakpoints: ['xs', 'sm', 'md', 'lg', 'xl'],
    factor: 2, // Slightly more aggressive scaling
  });

  return theme;
};

// Default light theme for cases where context isn't needed
export const defaultTheme = getTheme('light');