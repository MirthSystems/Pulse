import { createContext } from 'react';
import { ThemeContextType } from '../types/theme-context-type';

/**
 * React context for theme state and actions.
 * Provides current palette mode, toggle function, and dark mode status.
 * @see ThemeContextType for context value shape
 */
export const ThemeContext = createContext<ThemeContextType | undefined>(undefined);
