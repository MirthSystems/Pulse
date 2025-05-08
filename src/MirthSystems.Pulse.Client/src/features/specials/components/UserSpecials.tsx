import React, { useState, useEffect } from 'react';
import { 
  Box, 
  Typography, 
  Button, 
  Card, 
  CardContent, 
  CardActions,
  Chip,
  Stack,
  Alert,
  CircularProgress
} from '@mui/material';
import { apiClient, ISpecialResponse, SpecialTypes } from '../../api';
import { useApi, useApiRequest } from '../../api/hooks';

export const UserSpecials: React.FC = () => {
  const [specials, setSpecials] = useState<ISpecialResponse[]>([]);
  const { isAuthenticated, hasRole } = useApi();
  
  const isAdmin = hasRole('System.Administrator');
  const isContentManager = hasRole('Content.Manager');
  const canManageSpecials = isAdmin || isContentManager;

  const { isLoading, error, execute: fetchSpecials } = useApiRequest<ISpecialResponse[]>(
    'user-specials',
    () => apiClient.specials.getSpecials({
      address: 'Current Location',
      radius: 10,
      isCurrentlyRunning: true
    }),
    [isAuthenticated],
    {
      skipInitialCall: !isAuthenticated 
    }
  );

  useEffect(() => {
    const getSpecials = async () => {
      if (!isAuthenticated) return;
      
      const result = await fetchSpecials();
      if (result) {
        setSpecials(result);
      }
    };
    
    getSpecials();
  }, [fetchSpecials, isAuthenticated]);

  if (!isAuthenticated) {
    return (
      <Alert severity="info">
        Please sign in to view your specials
      </Alert>
    );
  }

  if (isLoading) {
    return <CircularProgress />;
  }

  if (error) {
    return (
      <Alert 
        severity="error"
        action={
          <Button color="inherit" size="small" onClick={() => fetchSpecials()}>
            Retry
          </Button>
        }
      >
        {error}
      </Alert>
    );
  }

  if (specials.length === 0) {
    return (
      <Box>
        <Typography variant="h6">My Specials</Typography>
        <Alert severity="info">No specials found</Alert>
        {canManageSpecials && (
          <Button 
            variant="contained" 
            color="primary" 
            sx={{ mt: 2 }}
            onClick={() => console.log('Create special clicked')}
          >
            Create New Special
          </Button>
        )}
      </Box>
    );
  }

  return (
    <Box>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
        <Typography variant="h6" gutterBottom>
          My Specials
        </Typography>
        
        <Button 
          variant="outlined" 
          size="small"
          onClick={() => fetchSpecials()}
          disabled={isLoading}
        >
          Refresh
        </Button>
      </Box>
      
      {canManageSpecials && (
        <Button 
          variant="contained" 
          color="primary" 
          sx={{ mb: 2 }}
          onClick={() => console.log('Create special clicked')}
        >
          Create New Special
        </Button>
      )}
      
      <Stack spacing={2}>
        {specials.map(special => (
          <Card key={special.id}>
            <CardContent>
              <Typography variant="h6">{special.venueName}</Typography>
              <Typography variant="body1">{special.content}</Typography>
              <Typography variant="body2" color="text.secondary">
                {special.startDate} at {special.startTime}
                {special.endTime && ` - ${special.endTime}`}
              </Typography>
              <Box sx={{ mt: 1 }}>
                <Chip 
                  label={special.typeName} 
                  color={
                    special.type === SpecialTypes.Drink ? 'primary' : 
                    special.type === SpecialTypes.Food ? 'success' : 
                    special.type === SpecialTypes.Entertainment ? 'info' : 
                    'default'
                  }
                  size="small"
                  sx={{ mr: 1 }}
                />
                {/* Check if today's date matches the special's date */}
                {new Date().toISOString().split('T')[0] === special.startDate && (
                  <Chip label="Today" color="error" size="small" />
                )}
              </Box>
            </CardContent>
            {canManageSpecials && (
              <CardActions>
                <Button size="small">Edit</Button>
                <Button size="small" color="error">Delete</Button>
              </CardActions>
            )}
          </Card>
        ))}
      </Stack>
    </Box>
  );
};