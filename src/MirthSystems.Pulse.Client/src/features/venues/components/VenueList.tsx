import React, { useState, useEffect } from 'react';
import { Box, CircularProgress, Typography, List, ListItem, ListItemText, Alert, Button } from '@mui/material';
import { apiClient, IVenueResponse } from '../../api';
import { useApi, useApiRequest } from '../../api/hooks';

export const VenueList: React.FC = () => {
  const [venues, setVenues] = useState<IVenueResponse[]>([]);
  const { isAuthenticated } = useApi();

  // Use our custom hook to handle API requests with Redux integration
  const { isLoading, error, execute: fetchVenues, lastFetched, isStale } = useApiRequest<IVenueResponse[]>(
    'venues-list', // Unique key for this API request
    () => apiClient.venues.getVenues(), // The API call to make
    [isAuthenticated], // Dependencies - refetch when authentication changes
    {
      cacheDuration: 30000, // Consider data stale after 30 seconds
    }
  );

  // Update local state when we receive venue data
  useEffect(() => {
    const getVenues = async () => {
      const result = await fetchVenues();
      if (result) {
        setVenues(result);
      }
    };
    
    getVenues();
  }, [fetchVenues]);

  // Loading state
  if (isLoading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" p={2}>
        <CircularProgress />
      </Box>
    );
  }

  // Error state
  if (error) {
    return (
      <Alert 
        severity="error"
        action={
          <Button color="inherit" size="small" onClick={() => fetchVenues()}>
            Retry
          </Button>
        }
      >
        {error}
      </Alert>
    );
  }

  // Empty state
  if (venues.length === 0) {
    return (
      <Box p={2}>
        <Typography variant="body1">No venues found</Typography>
        <Button 
          variant="contained" 
          sx={{ mt: 2 }}
          onClick={() => fetchVenues()}
        >
          Refresh
        </Button>
      </Box>
    );
  }

  // Format the last fetched time for display
  const lastFetchedText = lastFetched 
    ? new Date(lastFetched).toLocaleTimeString() 
    : 'Never';

  // Success state with data
  return (
    <Box>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
        <Typography variant="h6" component="h2" gutterBottom>
          Venues {isAuthenticated ? '(Authenticated)' : ''}
        </Typography>
        
        <Button 
          variant="outlined" 
          size="small"
          onClick={() => fetchVenues()}
          disabled={isLoading}
        >
          {isStale ? "Refresh" : "Refresh Again"}
        </Button>
      </Box>
      
      <Typography variant="caption" color="text.secondary" display="block" mb={2}>
        Last updated: {lastFetchedText}
      </Typography>
      
      <List>
        {venues.map((venue) => (
          <ListItem key={venue.id}>
            <ListItemText 
              primary={venue.name} 
              secondary={
                venue.address 
                  ? `${venue.address.locality}, ${venue.address.region}` 
                  : 'No address provided'
              } 
            />
          </ListItem>
        ))}
      </List>
    </Box>
  );
};