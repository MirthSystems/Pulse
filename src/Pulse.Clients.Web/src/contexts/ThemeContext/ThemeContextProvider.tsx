import React, { useState, useEffect, ReactNode, useMemo, useCallback } from 'react';
import { ThemeProvider as MuiThemeProvider } from '@mui/material/styles';
import { PaletteMode, CssBaseline } from '@mui/material';
import { getTheme } from '../../theme';
import ThemeContext, { ThemeContextType } from './index';
import { ThemePreference } from '../../types/theme-preference';

const THEME_STORAGE_KEY = 'pulse-theme-mode';
const USER_PREFERENCE_KEY = 'pulse-theme-preference';

/**
 * Props for the ThemeProvider component.
 * @property children - React children to be wrapped by the theme provider.
 * @property defaultMode - Optional default theme mode if nothing is stored
 */
interface ThemeProviderProps {
  children: ReactNode;
  defaultMode?: PaletteMode;
}

/**
 * Provides theme context and Material UI theming to the application.
 * Handles palette mode (light/dark), system preference, and localStorage persistence.
 * Also adds performance optimization with useMemo and useCallback.
 * 
 * @param props - ThemeProviderProps
 * @returns JSX.Element
 */
export const ThemeContextProvider: React.FC<ThemeProviderProps> = ({ 
  children, 
  defaultMode = 'light' 
}) => {
  // Get initial mode with preference handling
  const getInitialMode = (): PaletteMode => {
    // First check if user has explicitly set a preference
    const userPreference = localStorage.getItem(USER_PREFERENCE_KEY);
    
    if (userPreference === 'system') {
      // Use system preference
      const savedMode = localStorage.getItem(THEME_STORAGE_KEY);
      if (savedMode && (savedMode === 'light' || savedMode === 'dark')) {
        return savedMode as PaletteMode;
      }
      
      // Check system preference
      if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        return 'dark';
      }
      return defaultMode;
    } else if (userPreference === 'light' || userPreference === 'dark') {
      // User has explicitly chosen light or dark
      return userPreference as PaletteMode;
    }
    
    // Fallback to system preference if no explicit user preference
    const savedMode = localStorage.getItem(THEME_STORAGE_KEY);
    if (savedMode && (savedMode === 'light' || savedMode === 'dark')) {
      return savedMode as PaletteMode;
    }
    
    // Check system preference
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return 'dark';
    }
    
    // Default
    return defaultMode;
  };

  const [mode, setMode] = useState<PaletteMode>(getInitialMode);
  const [preference, setPreference] = useState<ThemePreference>(
    localStorage.getItem(USER_PREFERENCE_KEY) as ThemePreference || 'system'
  );

  /**
   * Toggles between light and dark color modes.
   */
  const toggleColorMode = useCallback(() => {
    setMode((prevMode) => {
      const newMode = prevMode === 'light' ? 'dark' : 'light';
      setPreference(newMode);
      localStorage.setItem(USER_PREFERENCE_KEY, newMode);
      return newMode;
    });
  }, []);

  /**
   * Set theme mode explicitly
   */
  const setThemeMode = useCallback((newMode: PaletteMode) => {
    setMode(newMode);
    setPreference(newMode);
    localStorage.setItem(USER_PREFERENCE_KEY, newMode);
  }, []);

  /**
   * Set to use system preference
   */
  const useSystemPreference = useCallback(() => {
    const systemMode = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches
      ? 'dark'
      : 'light';
    
    setMode(systemMode);
    setPreference('system');
    localStorage.setItem(USER_PREFERENCE_KEY, 'system');
  }, []);

  // Effect to store theme preference
  useEffect(() => {
    localStorage.setItem(THEME_STORAGE_KEY, mode);
    
    // Update document elements for theme
    document.body.style.backgroundColor = mode === 'light' ? '#fff' : '#121212';
    
    // Set color-scheme property for better native element styling
    document.documentElement.style.setProperty('color-scheme', mode);
    
    // Set theme color meta tag for PWA
    const metaThemeColor = document.querySelector('meta[name="theme-color"]');
    if (metaThemeColor) {
      metaThemeColor.setAttribute(
        'content', 
        mode === 'light' ? '#ffffff' : '#121212'
      );
    }
  }, [mode]);

  // Effect to handle system preference changes
  useEffect(() => {
    if (preference !== 'system') return;
    
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    
    const handleChange = (e: MediaQueryListEvent) => {
      setMode(e.matches ? 'dark' : 'light');
    };
    
    // Add event listener
    mediaQuery.addEventListener('change', handleChange);
    
    // Cleanup
    return () => mediaQuery.removeEventListener('change', handleChange);
  }, [preference]);

  // Memoize theme creation to prevent unnecessary re-renders
  const theme = useMemo(() => getTheme(mode), [mode]);

  // Memoize context value
  const contextValue = useMemo((): ThemeContextType => ({
    mode,
    toggleColorMode,
    setThemeMode,
    useSystemPreference,
    isDarkMode: mode === 'dark',
    preference,
  }), [mode, toggleColorMode, setThemeMode, useSystemPreference, preference]);

  return (
    <ThemeContext.Provider value={contextValue}>
      <MuiThemeProvider theme={theme}>
        {/* CssBaseline provides consistent, cross-browser styling */}
        <CssBaseline />
        {children}
      </MuiThemeProvider>
    </ThemeContext.Provider>
  );
};