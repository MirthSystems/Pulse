import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import {
  IconButton,
  Menu,
  MenuItem,
  ListItemIcon,
  ListItemText,
  Divider,
  Tooltip,
  ListItemSecondaryAction,
  Switch
} from '@mui/material';
import DashboardIcon from '@mui/icons-material/Dashboard';
import LogoutIcon from '@mui/icons-material/Logout';
import LocalFireDepartmentIcon from '@mui/icons-material/LocalFireDepartment';
import DarkModeIcon from '@mui/icons-material/DarkMode';
import LightModeIcon from '@mui/icons-material/LightMode';
import UserAvatar from './UserAvatar';
import { useAuth } from '../../hooks/useAuth';
import { useTheme } from '../../hooks/useTheme';

const UserMenu: React.FC = () => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const { account, logout } = useAuth();
  const { mode, toggleColorMode } = useTheme();
  const location = useLocation();
  
  const open = Boolean(anchorEl);
  const isDarkMode = mode === 'dark';
  
  // Dashboard route check using regex for better maintainability
  const isDashboard = /^\/(?:dashboard|profile|specials|venues|settings)/.test(location.pathname);
  
  const name = account?.name || '';

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    logout("/").catch((error: Error) => {
      console.error('Logout failed:', error);
    });
    setAnchorEl(null);
  };

  const handleThemeToggle = (e: React.MouseEvent) => {
    e.stopPropagation();
    toggleColorMode();
  };

  return (
    <>
      <Tooltip title="Account settings">
        <IconButton
          onClick={handleMenuOpen}
          size="small"
          sx={{ 
            bgcolor: 'primary.main', 
            color: 'white',
            '&:hover': { bgcolor: 'primary.dark' } 
          }}
          aria-controls={open ? 'account-menu' : undefined}
          aria-haspopup="true"
          aria-expanded={open ? 'true' : undefined}
        >
          <UserAvatar displayName={name} />
        </IconButton>
      </Tooltip>
      <Menu
        anchorEl={anchorEl}
        id="account-menu"
        open={open}
        onClose={handleMenuClose}
        onClick={handleMenuClose}
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
        PaperProps={{
          elevation: 0,
          sx: {
            overflow: 'visible',
            filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.15))',
            mt: 1.5,
            width: 220,
            '& .MuiAvatar-root': {
              width: 32,
              height: 32,
              ml: -0.5,
              mr: 1,
            },
          },
        }}
      >
        {isDashboard ? (
          <MenuItem component={Link} to="/">
            <ListItemIcon>
              <LocalFireDepartmentIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText primary="Main Site" />
          </MenuItem>
        ) : (
          <MenuItem component={Link} to="/dashboard">
            <ListItemIcon>
              <DashboardIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText primary="Dashboard" />
          </MenuItem>
        )}
        
        <Divider />
        
        <MenuItem onClick={handleThemeToggle}>
          <ListItemIcon>
            {isDarkMode ? <LightModeIcon fontSize="small" /> : <DarkModeIcon fontSize="small" />}
          </ListItemIcon>
          <ListItemText primary={isDarkMode ? "Light Mode" : "Dark Mode"} />
          <ListItemSecondaryAction>
            <Switch 
              edge="end"
              size="small"
              checked={isDarkMode}
              onClick={(e) => e.stopPropagation()}
              onChange={toggleColorMode}
            />
          </ListItemSecondaryAction>
        </MenuItem>
        
        <MenuItem onClick={handleLogout}>
          <ListItemIcon>
            <LogoutIcon fontSize="small" />
          </ListItemIcon>
          <ListItemText primary="Sign out" />
        </MenuItem>
      </Menu>
    </>
  );
};

export default UserMenu;