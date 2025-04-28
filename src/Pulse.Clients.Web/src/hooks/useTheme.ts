import { useContext } from 'react';
import { ThemeContextType } from '../types/theme-context-type';
import { ThemeContext } from '../contexts/theme-context';

/**
 * Custom hook to access the theme context.
 * Throws if used outside a ThemeProvider.
 * @returns ThemeContextType
 */
export const useTheme = (): ThemeContextType => {
  const context = useContext(ThemeContext);
  if (context === undefined) {
    throw new Error('useTheme must be used within a ThemeProvider');
  }
  return context;
};