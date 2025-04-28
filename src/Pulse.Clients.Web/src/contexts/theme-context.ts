import { createContext } from 'react';
import { ThemeContextType } from '../types/theme-context-type';

// Create the context with a default value
export const ThemeContext = createContext<ThemeContextType | undefined>(undefined);
