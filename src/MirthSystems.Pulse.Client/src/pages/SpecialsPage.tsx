import { useAuth0 } from '@auth0/auth0-react';
import {
  Add as AddIcon,
  MyLocation as MyLocationIcon,
  Search as SearchIcon
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
import { useNavigate, useSearchParams } from 'react-router-dom';

import SpecialsList from '@components/specials/SpecialsList';
import { clearSpecialsError, searchSpecials } from '@store/specialSlice';
import { SpecialSearchParams, SpecialTypes } from '@models/special';
import { RootState } from '@store/index';

const SpecialsPage = () => {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const dispatch = useDispatch();
  const { isAuthenticated } = useAuth0();
  const { searchResults, loading, error, pagingInfo } = useSelector((state: RootState) => state.specials);

  // Search parameters
  const [address, setAddress] = useState<string>('');
  const [radius, setRadius] = useState<number>(5);
  const [specialType, setSpecialType] = useState<number | string>('');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [activeOnly, setActiveOnly] = useState<boolean>(true);
  const [page, setPage] = useState<number>(1);

  // Initialize from URL parameters if present
  useEffect(() => {
    const addressParam = searchParams.get('address');
    const radiusParam = searchParams.get('radius');
    const typeParam = searchParams.get('type');
    const termParam = searchParams.get('term');
    const activeParam = searchParams.get('active');
    const pageParam = searchParams.get('page');

    if (addressParam) setAddress(addressParam);
    if (radiusParam) setRadius(Number(radiusParam));
    if (typeParam) setSpecialType(Number(typeParam));
    if (termParam) setSearchTerm(termParam);
    if (activeParam) setActiveOnly(activeParam === 'true');
    if (pageParam) setPage(Number(pageParam));

    // If we have an address, automatically perform search
    if (addressParam) {
      performSearch();
    }
  }, []);

  const performSearch = () => {
    if (!address) {
      return; // Require address for searching
    }

    const params: SpecialSearchParams = {
      address,
      radius,
      page,
      pageSize: 12,
      term: searchTerm || undefined,
      type: typeof specialType === 'number' ? specialType : undefined,
      active: activeOnly,
      dateTime: DateTime.now().toISO()
    };

    dispatch(searchSpecials(params) as any);

    // Update URL parameters for shareable links
    const newSearchParams = new URLSearchParams();
    newSearchParams.set('address', address);
    newSearchParams.set('radius', radius.toString());
    if (specialType) newSearchParams.set('type', specialType.toString());
    if (searchTerm) newSearchParams.set('term', searchTerm);
    newSearchParams.set('active', activeOnly.toString());
    newSearchParams.set('page', page.toString());
    setSearchParams(newSearchParams);
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    setPage(1); // Reset to first page on new search
    performSearch();
  };

  const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
    performSearch();
  };

  const handleCurrentLocation = () => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          setAddress(`${position.coords.latitude}, ${position.coords.longitude}`);
        },
        (error) => {
          console.error("Error getting location:", error);
        }
      );
    }
  };

  const handleClearError = () => {
    dispatch(clearSpecialsError());
  };

  return (
    <Container maxWidth="lg">
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Find Specials
        </Typography>
        {isAuthenticated && (
          <Button
            variant="contained"
            color="primary"
            startIcon={<AddIcon />}
            onClick={() => navigate('/specials/new')}
          >
            Add New Special
          </Button>
        )}
      </Box>

      <Paper component="form" onSubmit={handleSearch} elevation={3} sx={{ p: 3, mb: 4 }}>
        <Grid container spacing={2}>
          <Grid item xs={12} md={4}>
            <TextField
              fullWidth
              label="Location"
              placeholder="Enter address, city, or ZIP"
              value={address}
              onChange={(e) => setAddress(e.target.value)}
              required
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
            <TextField
              fullWidth
              label="Radius (miles)"
              type="number"
              value={radius}
              onChange={(e) => setRadius(Number(e.target.value))}
              inputProps={{ min: 1, max: 50, step: 1 }}
            />
          </Grid>

          <Grid item xs={12} md={2}>
            <FormControl fullWidth>
              <InputLabel>Special Type</InputLabel>
              <Select
                value={specialType}
                label="Special Type"
                onChange={(e) => setSpecialType(e.target.value)}
              >
                <MenuItem value="">All Types</MenuItem>
                <MenuItem value={SpecialTypes.Food}>Food</MenuItem>
                <MenuItem value={SpecialTypes.Drink}>Drink</MenuItem>
                <MenuItem value={SpecialTypes.Entertainment}>Entertainment</MenuItem>
              </Select>
            </FormControl>
          </Grid>

          <Grid item xs={12} md={2}>
            <FormControl fullWidth>
              <InputLabel>Status</InputLabel>
              <Select
                value={activeOnly}
                label="Status"
                onChange={(e) => setActiveOnly(e.target.value === 'true')}
              >
                <MenuItem value="true">Currently Active</MenuItem>
                <MenuItem value="false">All Specials</MenuItem>
              </Select>
            </FormControl>
          </Grid>

          <Grid item xs={12} md={2}>
            <Button
              fullWidth
              type="submit"
              variant="contained"
              color="primary"
              disabled={loading || !address}
              sx={{ height: '56px' }}
            >
              {loading ? <CircularProgress size={24} color="inherit" /> : "Search"}
            </Button>
          </Grid>

          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Search Specials"
              placeholder="Keywords in description"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon />
                  </InputAdornment>
                ),
              }}
            />
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
      ) : searchResults?.items?.length > 0 ? (
        <Box>
          <Typography variant="h5" component="h2" gutterBottom>
            Found {searchResults?.pagingInfo?.totalCount || 0} Results
          </Typography>

          {searchResults.items.map((result) => (
            <Paper key={result.venue?.id || `venue-${Math.random()}`} sx={{ mb: 4, p: 3 }}>
              <Box
                sx={{
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  mb: 2,
                  cursor: 'pointer'
                }}
                onClick={() => navigate(`/venues/${result.venue?.id || ''}`)}
              >
                <Typography variant="h5">{result.venue?.name || 'Unknown Venue'}</Typography>
                <Typography variant="body2" color="text.secondary">
                  {result.venue?.locality || ''}{result.venue?.locality && result.venue?.region ? ', ' : ''}{result.venue?.region || ''}
                </Typography>
              </Box>

              <Typography variant="subtitle1" gutterBottom>
                Current Specials ({result.specials?.items?.length || 0})
              </Typography>

              <SpecialsList specials={result.specials?.items || []} showVenueName={false} />
            </Paper>
          ))}

          <Box display="flex" justifyContent="center" my={4}>
            <Pagination
              count={pagingInfo.totalPages}
              page={pagingInfo.currentPage}
              onChange={handlePageChange}
              color="primary"
              disabled={loading}
            />
          </Box>
        </Box>
      ) : address && !loading ? (
        <Box textAlign="center" py={6}>
          <Typography variant="h6" gutterBottom>
            No specials found matching your criteria
          </Typography>
          <Typography variant="body1" color="text.secondary">
            Try adjusting your search radius, location, or filters
          </Typography>
        </Box>
      ) : (
        <Box textAlign="center" py={6}>
          <Typography variant="h6" gutterBottom>
            Enter a location to find specials
          </Typography>
          <Typography variant="body1" color="text.secondary">
            Search for food, drink, and entertainment specials near you
          </Typography>
        </Box>
      )}
    </Container>
  );
};

export default SpecialsPage;
