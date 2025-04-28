import { createContext } from 'react';
import { PaletteMode } from '@mui/material';
import { ThemePreference } from '../../types/theme-preference';

/**
 * Interface for theme context value.
 * Provides palette mode, toggle function, and dark mode status.
 */
export type ThemeContextType = {
  /** The current palette mode ('light' or 'dark'). */
  mode: PaletteMode;
  
  /** Function to toggle between light and dark mode. */
  toggleColorMode: () => void;
  
  /** Function to set theme mode explicitly to light or dark. */
  setThemeMode: (mode: PaletteMode) => void;
  
  /** Function to set theme to follow system preference. */
  useSystemPreference: () => void;
  
  /** Indicates if the current mode is dark. */
  isDarkMode: boolean;
  
  /** The user's theme preference setting (light, dark, or system). */
  preference: ThemePreference;
};

/**
 * React context for theme state and actions.
 * Provides current palette mode, toggle function, and dark mode status.
 */
const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export default ThemeContext;