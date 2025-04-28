import React, { useState, useEffect, ReactNode } from 'react';
import { ThemeProvider as MuiThemeProvider } from '@mui/material/styles';
import { PaletteMode } from '@mui/material';
import { getTheme } from '../../theme';
import { ThemeContextType } from '../../types/theme-context-type';
import { ThemeContext } from '../theme-context';

/**
 * Props for the ThemeProvider component.
 * @property children - React children to be wrapped by the theme provider.
 */
interface ThemeProviderProps {
  children: ReactNode;
}

/**
 * Provides theme context and Material UI theming to the application.
 * Handles palette mode (light/dark), system preference, and localStorage persistence.
 * @param props - ThemeProviderProps
 * @returns JSX.Element
 */
export const ThemeProvider: React.FC<ThemeProviderProps> = ({ children }) => {
  const getInitialMode = (): PaletteMode => {
    const savedMode = localStorage.getItem('themeMode');
    if (savedMode && (savedMode === 'light' || savedMode === 'dark')) {
      return savedMode as PaletteMode;
    }
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return 'dark';
    }
    return 'light';
  };

  const [mode, setMode] = useState<PaletteMode>(getInitialMode);

  /**
   * Toggles between light and dark color modes.
   */
  const toggleColorMode = () => {
    setMode((prevMode) => (prevMode === 'light' ? 'dark' : 'light'));
  };

  useEffect(() => {
    localStorage.setItem('themeMode', mode);
    document.body.style.backgroundColor = mode === 'light' ? '#fff' : '#121212';
  }, [mode]);

  useEffect(() => {
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    const handleChange = (e: MediaQueryListEvent) => {
      if (!localStorage.getItem('themeMode')) {
        setMode(e.matches ? 'dark' : 'light');
      }
    };
    mediaQuery.addEventListener('change', handleChange);
    return () => mediaQuery.removeEventListener('change', handleChange);
  }, []);

  const theme = getTheme(mode);

  const contextValue: ThemeContextType = {
    mode,
    toggleColorMode,
    isDarkMode: mode === 'dark',
  };

  return (
    <ThemeContext.Provider value={contextValue}>
      <MuiThemeProvider theme={theme}>
        {children}
      </MuiThemeProvider>
    </ThemeContext.Provider>
  );
};