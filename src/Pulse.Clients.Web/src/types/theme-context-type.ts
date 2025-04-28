import { PaletteMode } from '@mui/material';

/**
 * Interface for theme context value.
 * Provides palette mode, toggle function, and dark mode status.
 */
export interface ThemeContextType {
  /** The current palette mode ('light' or 'dark'). */
  mode: PaletteMode;
  /** Function to toggle between light and dark mode. */
  toggleColorMode: () => void;
  /** Indicates if the current mode is dark. */
  isDarkMode: boolean;
}