import { PaletteMode } from '@mui/material';

export interface ThemeContextType {
  mode: PaletteMode;
  toggleColorMode: () => void;
  isDarkMode: boolean;
}