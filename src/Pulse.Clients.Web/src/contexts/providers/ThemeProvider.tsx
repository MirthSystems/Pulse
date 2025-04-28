import React, { useState, useEffect, ReactNode } from 'react';
import { ThemeProvider as MuiThemeProvider } from '@mui/material/styles';
import { PaletteMode } from '@mui/material';
import { getTheme } from '../../theme';
import { ThemeContextType } from '../../types/theme-context-type';
import { ThemeContext } from '../theme-context';

// Props for the provider
interface ThemeProviderProps {
  children: ReactNode;
}

// Theme provider component
export const ThemeProvider: React.FC<ThemeProviderProps> = ({ children }) => {
  // Get saved theme mode from localStorage or use system preference
  const getInitialMode = (): PaletteMode => {
    const savedMode = localStorage.getItem('themeMode');
    if (savedMode && (savedMode === 'light' || savedMode === 'dark')) {
      return savedMode as PaletteMode;
    }
    
    // Check system preference if no saved preference
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return 'dark';
    }
    
    return 'light';
  };
  
  const [mode, setMode] = useState<PaletteMode>(getInitialMode);
  
  // Toggle between light and dark mode
  const toggleColorMode = () => {
    setMode((prevMode) => (prevMode === 'light' ? 'dark' : 'light'));
  };
  
  // Update theme when mode changes
  useEffect(() => {
    localStorage.setItem('themeMode', mode);
    
    // Update body background to match theme
    document.body.style.backgroundColor = 
      mode === 'light' ? '#fff' : '#121212';
  }, [mode]);
  
  // Listen for system theme preference changes
  useEffect(() => {
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    
    const handleChange = (e: MediaQueryListEvent) => {
      // Only change if user hasn't explicitly set a preference
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