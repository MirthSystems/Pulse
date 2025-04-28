import React from 'react';
import { Box, Typography, Button, Paper } from '@mui/material';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import { useNavigate } from 'react-router-dom';

interface ErrorComponentProps {
  title?: string;
  message?: string;
  showRetry?: boolean;
  showHome?: boolean;
}

const ErrorComponent: React.FC<ErrorComponentProps> = ({
  title = 'Error',
  message = 'Something went wrong. Please try again later.',
  showRetry = true,
  showHome = true
}) => {
  const navigate = useNavigate();

  return (
    <Paper
      elevation={0}
      sx={{
        p: 4,
        borderRadius: 2,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        textAlign: 'center',
        maxWidth: 500,
        mx: 'auto',
        my: 4,
        border: '1px solid',
        borderColor: 'divider'
      }}
    >
      <ErrorOutlineIcon color="error" sx={{ fontSize: 64, mb: 2 }} />
      <Typography variant="h5" component="h2" gutterBottom sx={{ fontWeight: 600 }}>
        {title}
      </Typography>
      <Typography variant="body1" color="text.secondary" paragraph>
        {message}
      </Typography>
      <Box sx={{ mt: 2, display: 'flex', gap: 2 }}>
        {showRetry && (
          <Button
            variant="contained"
            color="primary"
            onClick={() => window.location.reload()}
          >
            Retry
          </Button>
        )}
        {showHome && (
          <Button
            variant="outlined"
            onClick={() => navigate('/')}
          >
            Go Home
          </Button>
        )}
      </Box>
    </Paper>
  );
};

export default ErrorComponent;