import { useAuth0 } from '@auth0/auth0-react';
import { Box, Avatar, Typography, Paper, CircularProgress } from '@mui/material';

export const Profile = () => {
  const { user, isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return (
      <Box display="flex" justifyContent="center" padding={2}>
        <CircularProgress />
      </Box>
    );
  }

  if (!isAuthenticated || !user) {
    return null;
  }

  return (
    <Paper 
      elevation={2}
      sx={{ 
        p: 2, 
        m: 2, 
        display: 'flex', 
        flexDirection: 'column', 
        alignItems: 'center' 
      }}
    >
      <Avatar 
        src={user.picture} 
        alt={user.name}
        sx={{ width: 80, height: 80, mb: 2 }}
      />
      <Typography variant="h5" gutterBottom>
        {user.name}
      </Typography>
      <Typography variant="body1" color="text.secondary">
        {user.email}
      </Typography>
    </Paper>
  );
};