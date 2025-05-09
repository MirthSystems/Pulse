import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Box, 
  IconButton,
  Menu,
  MenuItem,
  ListItemIcon,
  ListItemText,
  Divider,
  Avatar,
  Button,
  Tooltip
} from '@mui/material';
import {
  AccountCircle as AccountCircleIcon,
  Dashboard as DashboardIcon,
  AdminPanelSettings as AdminIcon,
  Logout as LogoutIcon,
  Login as LoginIcon
} from '@mui/icons-material';
import { useAuth } from '../hooks';

export const ProfileMenu = () => {
  const { isAuthenticated, isAdmin, isVenueManager, user, loginWithRedirect, logout } = useAuth();
  const navigate = useNavigate();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleOpenMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleCloseMenu = () => {
    setAnchorEl(null);
  };

  const handleDashboard = () => {
    handleCloseMenu();
    navigate('/dashboard');
  };

  const handleAccount = () => {
    handleCloseMenu();
    navigate('/dashboard/account');
  };

  const handleAdmin = () => {
    handleCloseMenu();
    navigate('/dashboard/administration');
  };

  const handleHome = () => {
    handleCloseMenu();
    navigate('/');
  };

  const handleLogin = async () => {
    handleCloseMenu();
    await loginWithRedirect();
  };

  const handleLogout = () => {
    handleCloseMenu();
    logout({ returnTo: window.location.origin });
  };

  return (
    <>
      {isAuthenticated ? (
        <>
          <Tooltip title="Account">
            <IconButton
              onClick={handleOpenMenu}
              size="small"
              sx={{ ml: 2 }}
              aria-controls={open ? 'account-menu' : undefined}
              aria-haspopup="true"
              aria-expanded={open ? 'true' : undefined}
            >
              {user?.picture ? (
                <Avatar 
                  src={user.picture} 
                  alt={user.name || 'User'} 
                  sx={{ width: 32, height: 32 }}
                />
              ) : (
                <Avatar sx={{ width: 32, height: 32, bgcolor: 'primary.main' }}>
                  {user?.name?.[0] || 'U'}
                </Avatar>
              )}
            </IconButton>
          </Tooltip>
          <Menu
            anchorEl={anchorEl}
            id="account-menu"
            open={open}
            onClose={handleCloseMenu}
            PaperProps={{
              elevation: 0,
              sx: {
                overflow: 'visible',
                filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
                mt: 1.5,
                '& .MuiAvatar-root': {
                  width: 32,
                  height: 32,
                  ml: -0.5,
                  mr: 1,
                },
                '&:before': {
                  content: '""',
                  display: 'block',
                  position: 'absolute',
                  top: 0,
                  right: 14,
                  width: 10,
                  height: 10,
                  bgcolor: 'background.paper',
                  transform: 'translateY(-50%) rotate(45deg)',
                  zIndex: 0,
                },
              },
            }}
            transformOrigin={{ horizontal: 'right', vertical: 'top' }}
            anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
          >
            <MenuItem onClick={handleHome}>
              <ListItemIcon>
                <AccountCircleIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText primary="Home" />
            </MenuItem>
            
            <MenuItem onClick={handleDashboard}>
              <ListItemIcon>
                <DashboardIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText primary="Dashboard" />
            </MenuItem>
            
            <MenuItem onClick={handleAccount}>
              <ListItemIcon>
                <AccountCircleIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText primary="My Account" />
            </MenuItem>
            
            {(isAdmin() || isVenueManager()) && (
              <MenuItem onClick={handleAdmin}>
                <ListItemIcon>
                  <AdminIcon fontSize="small" />
                </ListItemIcon>
                <ListItemText primary="Administration" />
              </MenuItem>
            )}
            
            <Divider />
            
            <MenuItem onClick={handleLogout}>
              <ListItemIcon>
                <LogoutIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText primary="Logout" />
            </MenuItem>
          </Menu>
        </>
      ) : (
        <Button
          color="inherit"
          startIcon={<LoginIcon />}
          onClick={handleLogin}
        >
          Login
        </Button>
      )}
    </>
  );
};