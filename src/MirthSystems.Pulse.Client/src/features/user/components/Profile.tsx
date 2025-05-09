import { useState, MouseEvent } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { 
  Avatar, 
  Box, 
  IconButton, 
  Menu, 
  MenuItem, 
  Tooltip, 
  Typography,
  Divider,
  ListItemIcon
} from '@mui/material';
import { 
  AccountCircle, 
  Logout, 
  Person,
  Dashboard
} from '@mui/icons-material';
import { useNavigate, useLocation } from 'react-router-dom';

export const Profile: React.FC = () => {
  const { user, isAuthenticated, loginWithRedirect, logout, isLoading } = useAuth0();
  const navigate = useNavigate();
  const location = useLocation();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  
  // Check if we're in the MyAccount section
  const isInMyAccount = location.pathname.startsWith('/my-account');

  const handleClick = (event: MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    handleClose();
    logout({ logoutParams: { returnTo: window.location.origin } });
  };

  const handleLogin = () => {
    loginWithRedirect();
  };

  const toggleLayout = () => {
    handleClose();
    if (isInMyAccount) {
      navigate('/');
    } else {
      navigate('/my-account');
    }
  };

  const handleProfile = () => {
    handleClose();
    navigate(isInMyAccount ? '/my-account/profile' : '/profile');
  };

  if (isLoading) {
    return null;
  }

  if (!isAuthenticated) {
    return (
      <Tooltip title="Log in">
        <IconButton onClick={handleLogin} color="inherit">
          <AccountCircle />
        </IconButton>
      </Tooltip>
    );
  }

  return (
    <Box>
      <Tooltip title="Account settings">
        <IconButton
          onClick={handleClick}
          size="small"
          aria-controls={open ? 'account-menu' : undefined}
          aria-haspopup="true"
          aria-expanded={open ? 'true' : undefined}
          color="inherit"
        >
          {user?.picture ? (
            <Avatar 
              src={user.picture} 
              alt={user.name || 'User'} 
              sx={{ width: 32, height: 32 }} 
            />
          ) : (
            <AccountCircle />
          )}
        </IconButton>
      </Tooltip>
      <Menu
        anchorEl={anchorEl}
        id="account-menu"
        open={open}
        onClose={handleClose}
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
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
      >
        <Box sx={{ px: 2, py: 1 }}>
          <Typography variant="subtitle1" noWrap>
            {user?.name}
          </Typography>
          <Typography variant="body2" sx={{ opacity: 0.7 }} noWrap>
            {user?.email}
          </Typography>
        </Box>
        <Divider />
        <MenuItem onClick={handleProfile}>
          <ListItemIcon>
            <Person fontSize="small" />
          </ListItemIcon>
          My Profile
        </MenuItem>
        <MenuItem onClick={toggleLayout}>
          <ListItemIcon>
            <Dashboard fontSize="small" />
          </ListItemIcon>
          {isInMyAccount ? 'Main Site' : 'My Account'}
        </MenuItem>
        <Divider />
        <MenuItem onClick={handleLogout}>
          <ListItemIcon>
            <Logout fontSize="small" />
          </ListItemIcon>
          Logout
        </MenuItem>
      </Menu>
    </Box>
  );
};