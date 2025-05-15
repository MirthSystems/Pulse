import React, { ReactNode } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { Box, CircularProgress, Typography } from '@mui/material';

interface AuthStateManagerProps {
  children: ReactNode;
}

export const AuthStateManager = ({ children }: AuthStateManagerProps) => {
  const { isLoading, error } = useAuth0();
  
  if (isLoading) {
    return (
      <Box display="flex" flexDirection="column" alignItems="center" justifyContent="center" minHeight="200px">
        <CircularProgress />
        <Typography variant="body2" color="text.secondary" mt={2}>
          Setting up authentication...
        </Typography>
      </Box>
    );
  }
  
  if (error) {
    return (
      <Box p={3} textAlign="center">
        <Typography color="error" gutterBottom>
          Authentication Error
        </Typography>
        <Typography color="text.secondary">
          {error.message || "An error occurred during authentication setup."}
        </Typography>
      </Box>
    );
  }
  
  return <>{children}</>;
};

export default AuthStateManager;
