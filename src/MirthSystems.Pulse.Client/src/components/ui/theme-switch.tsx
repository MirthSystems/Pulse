import DarkModeIcon from '@mui/icons-material/DarkMode';
import LightModeIcon from '@mui/icons-material/LightMode';
import { Box, IconButton, Tooltip, useTheme } from '@mui/material';
import { useThemeStore } from '../../store';

export const ThemeSwitch = () => {
  const theme = useTheme();
  const { isDarkMode, toggleTheme } = useThemeStore();

  return (
    <Tooltip title={isDarkMode ? "Switch to Light Mode" : "Switch to Dark Mode"}>
      <IconButton
        onClick={toggleTheme}
        aria-label="Toggle theme"
        sx={{
          backgroundColor: theme.palette.mode === 'light'
            ? 'rgba(0, 194, 203, 0.1)'
            : 'rgba(0, 231, 242, 0.1)',
          borderRadius: '50%',
          width: 40,
          height: 40,
          transition: 'all 0.2s ease',
          '&:hover': {
            backgroundColor: theme.palette.mode === 'light'
              ? 'rgba(0, 194, 203, 0.2)'
              : 'rgba(0, 231, 242, 0.2)',
            transform: 'rotate(30deg)'
          }
        }}
      >
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: theme.palette.mode === 'light'
              ? theme.palette.primary.main
              : theme.palette.primary.light
          }}
        >
          {isDarkMode ? (
            <LightModeIcon sx={{ fontSize: '1.25rem' }} />
          ) : (
            <DarkModeIcon sx={{ fontSize: '1.25rem' }} />
          )}
        </Box>
      </IconButton>
    </Tooltip>
  );
};
