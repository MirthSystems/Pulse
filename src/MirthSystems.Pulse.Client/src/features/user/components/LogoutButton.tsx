import { useAuth0 } from '@auth0/auth0-react';
import { Button } from '@mui/material';

export const LogoutButton = () => {
  const { logout, isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return null;
  }

  if (!isAuthenticated) {
    return null;
  }

  return (
    <Button 
      variant="outlined" 
      color="primary" 
      onClick={() => logout({ logoutParams: { returnTo: window.location.origin } })}
    >
      Log Out
    </Button>
  );
};