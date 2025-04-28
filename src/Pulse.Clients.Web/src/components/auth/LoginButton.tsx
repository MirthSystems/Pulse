import React from 'react';
import { Box, Button } from '@mui/material';
import UserMenu from '../user/UserMenu';
import { useAuth } from '../../hooks/useAuth';

const LoginButton: React.FC = () => {
  const { isAuthenticated, loginRedirect } = useAuth();

  const handleLoginRedirect = () => {
    loginRedirect().catch((e: Error) => {
      console.error(e);
    });
  };

  return (
    <Box sx={{ position: 'absolute', top: 16, right: 24, zIndex: 1100 }}>
      {isAuthenticated ? (
        <UserMenu />
      ) : (
        <Button
          variant="contained"
          color="primary"
          onClick={handleLoginRedirect}
          sx={{ 
            borderRadius: 28,
            px: 3,
            py: 1,
            fontWeight: 600,
            boxShadow: 2,
            '&:hover': {
              boxShadow: 4,
            },
          }}
        >
          Sign in
        </Button>
      )}
    </Box>
  );
};

export default LoginButton;