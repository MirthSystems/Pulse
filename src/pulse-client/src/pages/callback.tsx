import { useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useNavigate } from 'react-router-dom';
import { CircularProgress, Box } from '@mui/material';
import { useAuthStore } from '../store';

export const CallbackPage = () => {
  const { isLoading, error, user, getAccessTokenSilently, isAuthenticated } = useAuth0();
  const navigate = useNavigate();
  const setAuthState = useAuthStore((s) => s.setAuthState);

  useEffect(() => {
    const syncAuth = async () => {
      if (isAuthenticated && user) {
        const token = await getAccessTokenSilently();
        setAuthState(true, user, token);
        navigate('/', { replace: true });
      }
    };
    syncAuth();
  }, [isAuthenticated, user, getAccessTokenSilently, setAuthState, navigate]);

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }

  return null;
};
