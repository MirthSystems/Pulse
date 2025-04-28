import React from 'react';
import { IconButton, Tooltip } from '@mui/material';
import Brightness4Icon from '@mui/icons-material/Brightness4'; // Moon icon
import Brightness7Icon from '@mui/icons-material/Brightness7'; // Sun icon
import { useTheme } from '../hooks/useTheme'; // Fixed import path

interface ThemeToggleProps {
  size?: 'small' | 'medium' | 'large';
  tooltip?: boolean;
}

/**
 * A button that toggles between light and dark themes
 */
const ThemeToggle: React.FC<ThemeToggleProps> = ({ 
  size = 'medium',
  tooltip = true 
}) => {
  const { toggleColorMode, mode } = useTheme();
  
  const icon = mode === 'light' ? <Brightness4Icon /> : <Brightness7Icon />;
  const tooltipTitle = mode === 'light' ? 'Switch to dark mode' : 'Switch to light mode';
  
  const button = (
    <IconButton
      onClick={toggleColorMode}
      color="inherit"
      size={size}
      aria-label={tooltipTitle}
    >
      {icon}
    </IconButton>
  );
  
  if (tooltip) {
    return <Tooltip title={tooltipTitle}>{button}</Tooltip>;
  }
  
  return button;
};

export default ThemeToggle;