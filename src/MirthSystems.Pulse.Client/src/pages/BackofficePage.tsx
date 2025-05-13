import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { 
  Box, 
  Typography, 
  Paper, 
  TextField, 
  Button, 
  Card, 
  CardContent,
  CardActions,
  Grid,
  Divider,
  CircularProgress,
  Alert,
  Pagination
} from '@mui/material';
import { Add as AddIcon, Place as PlaceIcon } from '@mui/icons-material';

import { RootState } from '@store/index';
import { fetchVenues, clearVenueError } from '@store/venueSlice';
import { VenueSearchParams } from '@models/venue';

const BackofficePage = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { venues, loading, error, pagingInfo } = useSelector((state: RootState) => state.venues);
  
  const [searchText, setSearchText] = useState<string>('');
  const [page, setPage] = useState<number>(1);

  useEffect(() => {
    loadVenues();
  }, [page]);

  const loadVenues = () => {
    const params: VenueSearchParams = {
      page,
      pageSize: 10,
      searchText: searchText || undefined
    };
    
    dispatch(fetchVenues(params) as any);
  };

  const handleSearch = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setPage(1); // Reset to page 1 when searching
    loadVenues();
  };

  const handleCreateVenue = () => {
    navigate('/venues/new');
  };

  const handleViewVenue = (id: string) => {
    navigate(`/venues/${id}`);
  };

  const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
  };

  return (
    <Box>
      <Box sx={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center', 
        mb: 3 
      }}>
        <Typography variant="h4" component="h1">
          Venue Management
        </Typography>
        <Button 
          variant="contained" 
          color="primary" 
          startIcon={<AddIcon />}
          onClick={handleCreateVenue}
        >
          Create New Venue
        </Button>
      </Box>

      <Paper component="form" onSubmit={handleSearch} sx={{ p: 2, mb: 4 }}>
        <Box sx={{ display: 'flex', gap: 2 }}>
          <TextField
            fullWidth
            label="Search Venues"
            value={searchText}
            onChange={(e) => setSearchText(e.target.value)}
            placeholder="Search by name or location"
            size="small"
          />
          <Button 
            type="submit" 
            variant="contained" 
            color="primary"
            disabled={loading}
          >
            Search
          </Button>
        </Box>
      </Paper>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      {loading ? (
        <Box display="flex" justifyContent="center" my={4}>
          <CircularProgress />
        </Box>
      ) : venues.length > 0 ? (
        <>
          {venues.map((venue) => (
            <Card key={venue.id} sx={{ mb: 2 }}>
              <CardContent>
                <Typography variant="h6" component="h2">
                  {venue.name}
                </Typography>
                <Box sx={{ display: 'flex', alignItems: 'center', mt: 1, color: 'text.secondary' }}>
                  <PlaceIcon fontSize="small" sx={{ mr: 0.5 }} />
                  <Typography variant="body2">
                    {venue.locality}, {venue.region}
                  </Typography>
                </Box>
                {venue.description && (
                  <Typography variant="body2" sx={{ mt: 1 }}>
                    {venue.description}
                  </Typography>
                )}
              </CardContent>
              <Divider />
              <CardActions>
                <Button 
                  size="small" 
                  onClick={() => handleViewVenue(venue.id!)}
                >
                  Details
                </Button>
                <Button 
                  size="small" 
                  color="error"
                >
                  Delete
                </Button>
              </CardActions>
            </Card>
          ))}

          <Box display="flex" justifyContent="center" mt={4}>
            <Pagination 
              count={pagingInfo.totalPages} 
              page={pagingInfo.currentPage} 
              onChange={handlePageChange} 
              color="primary"
            />
          </Box>
        </>
      ) : (
        <Typography variant="body1" textAlign="center" py={4}>
          No venues found. Create your first venue to get started.
        </Typography>
      )}
    </Box>
  );
};

export default BackofficePage;
