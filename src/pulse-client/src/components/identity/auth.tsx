import React, { useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useAuthStore } from '../../store';
import { IconButton } from '@mui/material';
import { UserAvatar } from './user-avatar';
import { UserMenu } from './user-menu';

export const Auth = () => {
  const { isAuthenticated } = useAuthStore();
  const { loginWithRedirect, logout } = useAuth0();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleUserMenuClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => setAnchorEl(null);
  const handleLogout = () => {
    logout({ logoutParams: { returnTo: window.location.origin } });
    useAuthStore.getState().logout();
    handleClose();
    window.location.href = '/';
  };

  if (!isAuthenticated) {
    return (
      <button onClick={() => loginWithRedirect()} style={{ color: 'inherit', background: 'none', border: 'none', cursor: 'pointer', font: 'inherit' }}>
        Login
      </button>
    );
  }

  return (
    <>
      <IconButton
        onClick={handleUserMenuClick}
        size="small"
        sx={{ ml: 2 }}
        aria-controls={open ? 'account-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={open ? 'true' : undefined}
      >
        <UserAvatar />
      </IconButton>
      <UserMenu anchorEl={anchorEl} open={open} onClose={handleClose} onLogout={handleLogout} />
    </>
  );
};

