import { useAuth0 } from '@auth0/auth0-react';
import { Button } from '@mui/material';

export const LoginButton = () => {
  const { loginWithRedirect, isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return null;
  }

  if (isAuthenticated) {
    return null;
  }

  return (
    <Button 
      variant="contained" 
      color="primary" 
      onClick={() => loginWithRedirect()}
    >
      Log In
    </Button>
  );
};