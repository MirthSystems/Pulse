import { useState, useEffect } from 'react';
import type { FormEvent } from 'react';
import { 
  Box, 
  TextField, 
  InputAdornment,
  IconButton,
  Grid,
  Paper,
  Tooltip,
  CircularProgress
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import CloseIcon from '@mui/icons-material/Close';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import { type GetVenuesRequest } from '../../models';
import { VenuesSearchButton } from './venues-search-button';

interface VenueListFilterProps {
  onChange: (filters: GetVenuesRequest) => void;
  initialFilters?: GetVenuesRequest;
  isLoading?: boolean;
}

export const VenueListFilter = ({ onChange, initialFilters, isLoading = false }: VenueListFilterProps) => {
  const [filters, setFilters] = useState<GetVenuesRequest>(initialFilters || {
    page: 1,
    pageSize: 10,
    searchText: '',
    address: '',
    radiusInMiles: 10,
    hasActiveSpecials: false,
    includeAddressDetails: false,
    includeBusinessHours: false,
    sortOrder: 0
  });
  const [gettingLocation, setGettingLocation] = useState(false);
  
  const handleFilterChange = (field: keyof GetVenuesRequest, value: string | number | boolean | undefined) => {
    const newFilters = { ...filters, [field]: value, page: 1 }; // Reset to page 1 on filter change
    setFilters(newFilters);
  };
  
  const handleSubmit = (e?: FormEvent) => {
    if (e) e.preventDefault();
    onChange(filters);
  };
  
  const clearSearchText = () => {
    handleFilterChange('searchText', '');
  };
  
  const clearLocation = () => {
    handleFilterChange('address', '');
  };

  const getCurrentLocation = () => {
    if (!navigator.geolocation) {
      alert('Geolocation is not supported by your browser');
      return;
    }
    
    setGettingLocation(true);
    
    navigator.geolocation.getCurrentPosition(
      async (position) => {
        try {
          // Use reverse geocoding to get address from coordinates
          const { latitude, longitude } = position.coords;
          
          // Using a simple reverse geocoding approach with Nominatim (OpenStreetMap)
          const response = await fetch(
            `https://nominatim.openstreetmap.org/reverse?format=json&lat=${latitude}&lon=${longitude}`
          );
          
          if (!response.ok) {
            throw new Error('Failed to get address from coordinates');
          }
          
          const data = await response.json();
          let address = '';
          
          if (data && data.display_name) {
            // Extract city, state, zip from the address
            const addressParts = data.address || {};
            const city = addressParts.city || addressParts.town || addressParts.village || '';
            const state = addressParts.state || '';
            const postcode = addressParts.postcode || '';
            
            address = [city, state, postcode].filter(Boolean).join(', ');
            
            // If we couldn't extract structured data, fall back to the full address
            if (!address) {
              address = data.display_name;
            }
          }
          
          handleFilterChange('address', address);
        } catch (error) {
          console.error('Error getting location:', error);
          alert('Unable to retrieve your location. Please enter it manually.');
        } finally {
          setGettingLocation(false);
        }
      },
      (error) => {
        console.error('Geolocation error:', error);
        setGettingLocation(false);
        alert('Unable to retrieve your location. Please enter it manually.');
      }
    );
  };
  
  useEffect(() => {
    if (initialFilters) {
      setFilters(initialFilters);
    }
  }, [initialFilters]);
  
  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%' }}>
      <Paper sx={{ p: 3, mb: 0 }} elevation={0} variant="outlined">
        <Grid container spacing={2} sx={{ alignItems: 'center', width: '100%' }}>
          <Grid sx={{ gridColumn: { xs: 'span 12', md: 'span 5' } }}>
            <TextField
              fullWidth
              placeholder="Search venues by name, description, or tags..."
              variant="outlined"
              size="small"
              value={filters.searchText || ''}
              onChange={(e) => handleFilterChange('searchText', e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon />
                  </InputAdornment>
                ),
                endAdornment: filters.searchText ? (
                  <InputAdornment position="end">
                    <IconButton
                      size="small"
                      onClick={clearSearchText}
                      edge="end"
                    >
                      <CloseIcon fontSize="small" />
                    </IconButton>
                  </InputAdornment>
                ) : null
              }}
            />
          </Grid>
          
          <Grid sx={{ gridColumn: { xs: 'span 12', md: 'span 4' } }}>
            <TextField
              fullWidth
              placeholder="Enter city, state or full address"
              label="Location"
              variant="outlined"
              size="small"
              value={filters.address || ''}
              onChange={(e) => handleFilterChange('address', e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <Tooltip title="Use current location">
                      <IconButton 
                        size="small" 
                        onClick={getCurrentLocation}
                        disabled={gettingLocation}
                      >
                        {gettingLocation ? (
                          <CircularProgress size={20} />
                        ) : (
                          <LocationOnIcon fontSize="small" />
                        )}
                      </IconButton>
                    </Tooltip>
                  </InputAdornment>
                ),
                endAdornment: filters.address ? (
                  <InputAdornment position="end">
                    <IconButton
                      size="small"
                      onClick={clearLocation}
                      edge="end"
                    >
                      <CloseIcon fontSize="small" />
                    </IconButton>
                  </InputAdornment>
                ) : null
              }}
            />
          </Grid>
          
          <Grid sx={{ gridColumn: { xs: 'span 6', md: 'span 1.5' } }}>
            <TextField
              fullWidth
              label="Radius (miles)"
              type="number"
              variant="outlined"
              size="small"
              value={filters.radiusInMiles || ''}
              onChange={(e) => handleFilterChange('radiusInMiles', parseInt(e.target.value) || 0)}
              inputProps={{ min: 0, max: 100 }}
            />
          </Grid>

          <Grid sx={{ gridColumn: { xs: 'span 6', md: 'span 1.5' } }}>
            <Box sx={{ height: '100%' }}>
              <VenuesSearchButton onClick={handleSubmit} isLoading={isLoading} />
            </Box>
          </Grid>
        </Grid>
      </Paper>
    </Box>
  );
};
