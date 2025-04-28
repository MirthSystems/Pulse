import React, { useState } from 'react';
import { 
  IconButton, 
  Tooltip, 
  Menu, 
  MenuItem, 
  ListItemIcon, 
  ListItemText,
  useMediaQuery 
} from '@mui/material';
import Brightness4Icon from '@mui/icons-material/Brightness4'; // Moon icon
import Brightness7Icon from '@mui/icons-material/Brightness7'; // Sun icon
import SettingsBrightnessIcon from '@mui/icons-material/SettingsBrightness'; // Auto icon
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import CheckIcon from '@mui/icons-material/Check';
import { useTheme } from '../hooks/useTheme';
import { ThemePreference } from '../types/theme-context-type';

interface ThemeToggleProps {
  size?: 'small' | 'medium' | 'large';
  tooltip?: boolean;
  /** Whether to show a simple toggle (true) or dropdown menu (false) */
  simpleToggle?: boolean;
}

/**
 * A component that allows toggling or selecting theme options
 */
const ThemeToggle: React.FC<ThemeToggleProps> = ({ 
  size = 'medium',
  tooltip = true,
  simpleToggle = false
}) => {
  const { toggleColorMode, mode, preference, setThemeMode, useSystemPreference } = useTheme();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const prefersDarkMode = useMediaQuery('(prefers-color-scheme: dark)');
  
  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    if (simpleToggle) {
      toggleColorMode();
    } else {
      setAnchorEl(event.currentTarget);
    }
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleThemeChange = (newPreference: ThemePreference) => {
    if (newPreference === 'system') {
      useSystemPreference();
    } else {
      setThemeMode(newPreference);
    }
    handleClose();
  };

  // Determine current icon to display
  const getCurrentIcon = () => {
    if (simpleToggle) {
      return mode === 'light' ? <Brightness4Icon /> : <Brightness7Icon />;
    }
    
    // For dropdown toggle, show icon based on preference
    switch (preference) {
      case 'light':
        return <Brightness7Icon />;
      case 'dark':
        return <Brightness4Icon />;
      case 'system':
        return <SettingsBrightnessIcon />;
      default:
        return mode === 'light' ? <Brightness7Icon /> : <Brightness4Icon />;
    }
  };

  const tooltipTitle = simpleToggle 
    ? (mode === 'light' ? 'Switch to dark mode' : 'Switch to light mode')
    : 'Theme settings';
  
  const button = (
    <IconButton
      onClick={handleClick}
      color="inherit"
      size={size}
      aria-label={tooltipTitle}
      aria-controls={anchorEl ? 'theme-menu' : undefined}
      aria-haspopup={!simpleToggle}
      aria-expanded={!simpleToggle && Boolean(anchorEl) ? 'true' : undefined}
      sx={{ 
        display: 'flex', 
        alignItems: 'center',
        gap: '2px'
      }}
    >
      {getCurrentIcon()}
      {!simpleToggle && <ArrowDropDownIcon fontSize="small" />}
    </IconButton>
  );
  
  const wrappedButton = tooltip ? <Tooltip title={tooltipTitle}>{button}</Tooltip> : button;
  
  if (simpleToggle) {
    return wrappedButton;
  }
  
  return (
    <>
      {wrappedButton}
      <Menu
        id="theme-menu"
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'right',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'right',
        }}
      >
        <MenuItem 
          onClick={() => handleThemeChange('light')}
          selected={preference === 'light'}
        >
          <ListItemIcon>
            <Brightness7Icon fontSize="small" />
          </ListItemIcon>
          <ListItemText>Light</ListItemText>
          {preference === 'light' && <CheckIcon fontSize="small" color="primary" />}
        </MenuItem>
        
        <MenuItem 
          onClick={() => handleThemeChange('dark')}
          selected={preference === 'dark'}
        >
          <ListItemIcon>
            <Brightness4Icon fontSize="small" />
          </ListItemIcon>
          <ListItemText>Dark</ListItemText>
          {preference === 'dark' && <CheckIcon fontSize="small" color="primary" />}
        </MenuItem>
        
        <MenuItem 
          onClick={() => handleThemeChange('system')}
          selected={preference === 'system'}
        >
          <ListItemIcon>
            <SettingsBrightnessIcon fontSize="small" />
          </ListItemIcon>
          <ListItemText primary="System" secondary={`(${prefersDarkMode ? 'Dark' : 'Light'})`} />
          {preference === 'system' && <CheckIcon fontSize="small" color="primary" />}
        </MenuItem>
      </Menu>
    </>
  );
};

export default ThemeToggle;