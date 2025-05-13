import { useAuth0 } from '@auth0/auth0-react';
import {
  Add as AddIcon,
  MyLocation as MyLocationIcon,
  Search as SearchIcon,
} from '@mui/icons-material';
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  Container,
  FormControl,
  Grid,
  IconButton,
  InputAdornment,
  InputLabel,
  MenuItem,
  Pagination,
  Paper,
  Select,
  TextField,
  Typography
} from '@mui/material';
import { DateTime } from 'luxon';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { clearVenueError, fetchVenues } from '@/store/venueSlice';
import VenueCard from '@components/venues/VenueCard';
import { VenueSearchParams } from '@models/venue';
import { RootState } from '@store/index';

const VenuesPage = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { isAuthenticated } = useAuth0();
  const { venues, loading, error, pagingInfo } = useSelector((state: RootState) => state.venues);

  const [searchParams, setSearchParams] = useState<VenueSearchParams>({
    page: 1,
    pageSize: 12,
    searchText: '',
    address: '',
    radius: 5,
    includeAddress: true,
    includeHours: false,
    sort: 0,
  });

  useEffect(() => {
    loadVenues();
  }, [searchParams.page]);

  useEffect(() => {
    return () => {
      dispatch(clearVenueError());
    };
  }, [dispatch]);

  const loadVenues = () => {
    dispatch(fetchVenues(searchParams) as any);
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    setSearchParams({ ...searchParams, page: 1 }); // Reset to first page
    loadVenues();
  };

  const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
    setSearchParams({ ...searchParams, page: value });
  };

  const handleCurrentLocation = () => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          setSearchParams({
            ...searchParams,
            address: `${position.coords.latitude}, ${position.coords.longitude}`
          });
        },
        (error) => {
          console.error("Error getting location:", error);
        }
      );
    }
  };

  const handleClearError = () => {
    dispatch(clearVenueError());
  };

  return (
    <Container maxWidth="lg">
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Venues
        </Typography>
        {isAuthenticated && (
          <Button
            variant="contained"
            color="primary"
            startIcon={<AddIcon />}
            onClick={() => navigate('/venues/new')}
          >
            Add New Venue
          </Button>
        )}
      </Box>

      <Paper component="form" elevation={3} sx={{ p: 3, mb: 4 }} onSubmit={handleSearch}>
        <Grid container spacing={2} alignItems="flex-end">
          <Grid item xs={12} md={3}>
            <TextField
              fullWidth
              label="Search Venues"
              placeholder="Name or description"
              value={searchParams.searchText || ''}
              onChange={(e) => setSearchParams({ ...searchParams, searchText: e.target.value })}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <SearchIcon />
                  </InputAdornment>
                ),
              }}
            />
          </Grid>

          <Grid item xs={12} md={3}>
            <TextField
              fullWidth
              label="Location"
              placeholder="Address, city, or ZIP"
              value={searchParams.address || ''}
              onChange={(e) => setSearchParams({ ...searchParams, address: e.target.value })}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton onClick={handleCurrentLocation} size="small">
                      <MyLocationIcon />
                    </IconButton>
                  </InputAdornment>
                ),
              }}
            />
          </Grid>

          <Grid item xs={12} md={2}>
            <FormControl fullWidth>
              <InputLabel>Sort By</InputLabel>
              <Select
                value={searchParams.sort || 0}
                label="Sort By"
                onChange={(e) => setSearchParams({ ...searchParams, sort: Number(e.target.value) })}
              >
                <MenuItem value={0}>Name (A-Z)</MenuItem>
                <MenuItem value={1}>Name (Z-A)</MenuItem>
                <MenuItem value={2}>Distance (Near to Far)</MenuItem>
                <MenuItem value={3}>Most Specials</MenuItem>
              </Select>
            </FormControl>
          </Grid>

          <Grid item xs={12} md={2}>
            <FormControl fullWidth>
              <InputLabel>Open Now</InputLabel>
              <Select
                value={searchParams.openDay !== undefined ? 'now' : ''}
                label="Open Now"
                onChange={(e) => {
                  if (e.target.value === 'now') {
                    const now = DateTime.local();
                    setSearchParams({
                      ...searchParams,
                      openDay: now.weekday % 7, // Convert to 0-based weekday (0=Sunday)
                      openTime: now.toFormat('HH:mm')
                    });
                  } else {
                    setSearchParams({
                      ...searchParams,
                      openDay: undefined,
                      openTime: undefined
                    });
                  }
                }}
              >
                <MenuItem value="">All Hours</MenuItem>
                <MenuItem value="now">Open Now</MenuItem>
              </Select>
            </FormControl>
          </Grid>

          <Grid item xs={12} md={2}>
            <Button
              fullWidth
              type="submit"
              variant="contained"
              color="primary"
              disabled={loading}
              sx={{ height: '56px' }}
            >
              {loading ? <CircularProgress size={24} color="inherit" /> : "Search"}
            </Button>
          </Grid>
        </Grid>
      </Paper>

      {error && (
        <Alert severity="error" onClose={handleClearError} sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      {loading ? (
        <Box display="flex" justifyContent="center" my={4}>
          <CircularProgress />
        </Box>
      ) : venues.length > 0 ? (
        <>
          <Grid container spacing={3} sx={{ mb: 4 }}>
            {venues.map((venue) => (
              <Grid item key={venue.id} xs={12} sm={6} md={4}>
                <VenueCard
                  venue={venue}
                  onClick={() => navigate(`/venues/${venue.id}`)}
                />
              </Grid>
            ))}
          </Grid>

          <Box display="flex" justifyContent="center" my={4}>
            <Pagination
              count={pagingInfo.totalPages}
              page={pagingInfo.currentPage}
              onChange={handlePageChange}
              color="primary"
              disabled={loading}
            />
          </Box>
        </>
      ) : (
        <Box textAlign="center" py={6}>
          <Typography variant="h6" gutterBottom>
            No venues found
          </Typography>
          <Typography color="text.secondary">
            Try adjusting your search criteria or add a new venue
          </Typography>
        </Box>
      )}
    </Container>
  );
};

export default VenuesPage;
