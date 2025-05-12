import { useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { Link as RouterLink } from 'react-router-dom';
import { 
  Box, 
  IconButton, 
  Menu, 
  MenuItem, 
  ListItemIcon, 
  ListItemText, 
  Avatar, 
  Tooltip, 
  Divider,
  Button 
} from '@mui/material';
import { 
  AccountCircle, 
  ExitToApp, 
  BusinessCenter as BackofficeIcon 
} from '@mui/icons-material';

interface UserMenuProps {
  isBackoffice?: boolean;
}

const UserMenu = ({ isBackoffice = false }: UserMenuProps) => {
  const { user, logout, loginWithRedirect, isAuthenticated } = useAuth0();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  
  const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    logout({ logoutParams: { returnTo: window.location.origin } });
    handleClose();
  };

  if (!isAuthenticated) {
    return (
      <Button 
        color="inherit" 
        onClick={() => loginWithRedirect()}
      >
        Log In
      </Button>
    );
  }

  return (
    <Box>
      <Tooltip title="Account settings">
        <IconButton
          onClick={handleMenu}
          size="small"
          aria-controls={Boolean(anchorEl) ? 'account-menu' : undefined}
          aria-haspopup="true"
          aria-expanded={Boolean(anchorEl) ? 'true' : undefined}
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
        open={Boolean(anchorEl)}
        onClose={handleClose}
        PaperProps={{
          elevation: 2,
          sx: {
            overflow: 'visible',
            filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.15))',
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
        <MenuItem onClick={handleClose}>
          <Avatar src={user?.picture} /> 
          {user?.name || 'User Profile'}
        </MenuItem>
        
        <Divider />
        
        <MenuItem 
          component={RouterLink} 
          to={isBackoffice ? '/' : '/backoffice'} 
          onClick={handleClose}
        >
          <ListItemIcon>
            <BackofficeIcon fontSize="small" />
          </ListItemIcon>
          <ListItemText>
            {isBackoffice ? 'Main Site' : 'Backoffice'}
          </ListItemText>
        </MenuItem>
        
        <MenuItem onClick={handleLogout}>
          <ListItemIcon>
            <ExitToApp fontSize="small" />
          </ListItemIcon>
          <ListItemText>Logout</ListItemText>
        </MenuItem>
      </Menu>
    </Box>
  );
};

export default UserMenu;
