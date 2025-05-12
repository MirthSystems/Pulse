import { useAuth0 } from '@auth0/auth0-react';
import { Button } from '@mui/material';
import { Login as LoginIcon, Logout as LogoutIcon } from '@mui/icons-material';

const AuthButton = () => {
  const { isAuthenticated, loginWithRedirect, logout } = useAuth0();

  return isAuthenticated ? (
    <Button 
      color="inherit" 
      onClick={() => logout({ logoutParams: { returnTo: window.location.origin } })}
      endIcon={<LogoutIcon />}
    >
      Log Out
    </Button>
  ) : (
    <Button 
      color="inherit" 
      onClick={() => loginWithRedirect()}
      endIcon={<LoginIcon />}
    >
      Log In
    </Button>
  );
};

export default AuthButton;
