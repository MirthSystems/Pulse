import React, { useState } from 'react';
import { useMsal } from '@azure/msal-react';
import { useIsAuthenticated } from '@azure/msal-react';
import { loginRequest } from '../../configs/auth';
import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';

const SignInSignOutButton: React.FC = () => {
  const { instance } = useMsal();
  const isAuthenticated = useIsAuthenticated();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleLogin = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleLoginRedirect = () => {
    instance.loginRedirect(loginRequest).catch(e => {
      console.error(e);
    });
    setAnchorEl(null);
  };

  const handleLoginPopup = () => {
    instance.loginPopup(loginRequest).catch(e => {
      console.error(e);
    });
    setAnchorEl(null);
  };

  const handleLogoutRedirect = () => {
    instance.logoutRedirect({
      postLogoutRedirectUri: "/"
    });
  };

  const handleLogoutPopup = () => {
    instance.logoutPopup({
      postLogoutRedirectUri: "/"
    });
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <>
      {isAuthenticated ? (
        <div>
          <Button
            variant="contained"
            color="primary"
            onClick={handleLogin}
          >
            Logout
          </Button>
          <Menu
            id="logout-menu"
            anchorEl={anchorEl}
            open={open}
            onClose={handleClose}
          >
            <MenuItem onClick={handleLogoutRedirect}>Logout using Redirect</MenuItem>
            <MenuItem onClick={handleLogoutPopup}>Logout using Popup</MenuItem>
          </Menu>
        </div>
      ) : (
        <div>
          <Button
            variant="contained"
            color="primary"
            onClick={handleLogin}
          >
            Login
          </Button>
          <Menu
            id="login-menu"
            anchorEl={anchorEl}
            open={open}
            onClose={handleClose}
          >
            <MenuItem onClick={handleLoginRedirect}>Sign in using Redirect</MenuItem>
            <MenuItem onClick={handleLoginPopup}>Sign in using Popup</MenuItem>
          </Menu>
        </div>
      )}
    </>
  );
};

export default SignInSignOutButton;