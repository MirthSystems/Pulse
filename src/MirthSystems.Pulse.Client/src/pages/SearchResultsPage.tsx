import { MyLocation as MyLocationIcon, Search as SearchIcon } from '@mui/icons-material';
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  Divider,
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
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, useSearchParams } from 'react-router-dom';

import SpecialsList from '@components/specials/SpecialsList';
import { clearSpecialsError } from '@store/specialSlice';
import { SpecialSearchParams, SpecialTypes, PagedResult, SearchSpecialsResult } from '@models/special';
import { RootState } from '@store/index';
import { SpecialService } from '@services/specialService';

const SearchResultsPage = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  
  // Add missing state variables
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [results, setResults] = useState<PagedResult<SearchSpecialsResult> | null>(null);

  // Search parameters
  const [address, setAddress] = useState<string>(searchParams.get('address') || '');
  const [radius, setRadius] = useState<number>(Number(searchParams.get('radius')) || 5);
  const [specialType, setSpecialType] = useState<number | string>(searchParams.get('type') || '');
  const [searchTerm, setSearchTerm] = useState<string>(searchParams.get('term') || '');
  const [activeOnly] = useState<boolean>(searchParams.get('active') !== 'false');
  const [page, setPage] = useState<number>(Number(searchParams.get('page')) || 1);

  useEffect(() => {
    // If we have search params, perform search automatically
    if (address) {
      performSearch();
    }
  }, []);

  const performSearch = async () => {
    setLoading(true);
    setError(null);

    try {
      if (!address) {
        setError('Location is required for searching specials.');
        setLoading(false);
        return;
      }

      const params: SpecialSearchParams = {
        address,
        radius,
        page,
        pageSize: 6,
        term: searchTerm || undefined,
        type: typeof specialType === 'number' ? specialType : undefined,
        active: activeOnly
      };

      // Use the service method instead of direct API call
      const response = await SpecialService.searchSpecials(params);
      setResults(response);
    } catch (error: any) {
      console.error('Error fetching search results:', error);

      if (error.status === 404) {
        setError('No results found for your search criteria. Try adjusting your filters.');
      } else if (error.status === 400) {
        setError('Invalid search parameters. Please check your inputs and try again.');
      } else {
        setError('Failed to load results. Please try again later.');
      }
    } finally {
      setLoading(false);
    }

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
    // After setting the page, trigger a new search
    setTimeout(performSearch, 0);
  };

  const handleGetCurrentLocation = () => {
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
    setError(null);
  };

  return (
    <Box>
      <Paper component="form" elevation={2} sx={{ p: 3, mb: 4 }} onSubmit={handleSearch}>
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={3}>
            <TextField
              fullWidth
              required
              label="Location"
              value={address}
              onChange={(e) => setAddress(e.target.value)}
              placeholder="Address, city, or ZIP"
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton onClick={handleGetCurrentLocation} size="small">
                      <MyLocationIcon />
                    </IconButton>
                  </InputAdornment>
                ),
              }}
            />
          </Grid>

          <Grid item xs={6} md={2}>
            <TextField
              fullWidth
              type="number"
              label="Radius (miles)"
              value={radius}
              onChange={(e) => setRadius(Number(e.target.value))}
              inputProps={{ min: 1, max: 50 }}
            />
          </Grid>

          <Grid item xs={6} md={2}>
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

          <Grid item xs={12} md={3}>
            <TextField
              fullWidth
              label="Search Term"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Keywords"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon />
                  </InputAdornment>
                ),
              }}
            />
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
        </Grid>
      </Paper>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
          <Button
            color="inherit"
            size="small"
            onClick={handleClearError}
            sx={{ ml: 2 }}
          >
            Dismiss
          </Button>
        </Alert>
      )}

      {loading ? (
        <Box display="flex" justifyContent="center" my={4}>
          <CircularProgress />
        </Box>
      ) : results?.items?.length > 0 ? (
        <Box>
          <Typography variant="h5" gutterBottom>
            Found {results?.pagingInfo?.totalCount || 0} Results
          </Typography>

          {results.items.map((result) => (
            <Paper
              key={result.venue?.id || `venue-${Math.random()}`}
              elevation={1}
              sx={{
                mb: 4,
                overflow: 'hidden',
                cursor: 'pointer',
                transition: 'transform 0.2s ease, box-shadow 0.2s ease',
                '&:hover': {
                  transform: 'translateY(-4px)',
                  boxShadow: 3
                }
              }}
              onClick={() => navigate(`/venue/${result.venue?.id || ''}`)}
            >
              <Grid container>
                <Grid item xs={12} md={4} sx={{
                  backgroundImage: `url(${result.venue?.profileImage || '/img/default-venue.jpg'})`,
                  backgroundSize: 'cover',
                  backgroundPosition: 'center',
                  minHeight: { xs: 150, md: 'auto' }
                }} />

                <Grid item xs={12} md={8}>
                  <Box sx={{ p: 3 }}>
                    <Typography variant="h5" component="h2">
                      {result.venue?.name}
                    </Typography>

                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                      {result.venue?.locality}, {result.venue?.region}
                    </Typography>

                    {result.venue?.description && (
                      <Typography variant="body2" sx={{ mb: 2 }}>
                        {result.venue?.description}
                      </Typography>
                    )}

                    <Divider sx={{ mb: 2 }} />

                    <Typography variant="subtitle1" gutterBottom>
                      Current Specials:
                    </Typography>

                    <SpecialsList specials={result.specials?.items} showVenueName={false} />
                  </Box>
                </Grid>
              </Grid>
            </Paper>
          ))}

          {results.pagingInfo && results.pagingInfo.totalPages > 1 && (
            <Box display="flex" justifyContent="center" my={4}>
              <Pagination
                count={results.pagingInfo.totalPages}
                page={results.pagingInfo.currentPage}
                onChange={handlePageChange}
                color="primary"
                disabled={loading}
              />
            </Box>
          )}
        </Box>
      ) : address && !loading ? (
        <Typography variant="h6" textAlign="center" py={6}>
          No specials found matching your criteria. Try adjusting your search.
        </Typography>
      ) : null}
    </Box>
  );
};

export default SearchResultsPage;
