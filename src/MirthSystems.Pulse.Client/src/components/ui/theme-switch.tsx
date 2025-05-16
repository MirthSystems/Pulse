import { IconButton } from '@mui/material';
import Brightness7Icon from '@mui/icons-material/Brightness7';
import Brightness4Icon from '@mui/icons-material/Brightness4';
import { useThemeStore } from '../../store';

export const ThemeSwitch = () => {
  const { isDarkMode, toggleTheme } = useThemeStore();
  return (
    <IconButton onClick={toggleTheme} color="inherit" aria-label="Toggle theme">
      {isDarkMode ? <Brightness7Icon /> : <Brightness4Icon />}
    </IconButton>
  );
};
