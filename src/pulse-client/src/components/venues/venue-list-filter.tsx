import { useState, useEffect, FormEvent } from 'react';
import { 
  Box, 
  TextField, 
  InputAdornment,
  IconButton,
  Grid,
  Paper,
  Stack
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import CloseIcon from '@mui/icons-material/Close';
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
  
  useEffect(() => {
    if (initialFilters) {
      setFilters(initialFilters);
    }
  }, [initialFilters]);
  
  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%' }}>
      <Paper sx={{ p: 2, mb: 3 }} elevation={0} variant="outlined">
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={3}>
            <TextField
              fullWidth
              placeholder="Search venues..."
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
          
          <Grid item xs={12} md={3}>
            <TextField
              fullWidth
              placeholder="City, state or zip"
              label="Location"
              variant="outlined"
              size="small"
              value={filters.address || ''}
              onChange={(e) => handleFilterChange('address', e.target.value)}
              InputProps={{
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
          
          <Grid item xs={12} md={3}>
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

          <Grid item xs={12} md={3}>
            <VenuesSearchButton onClick={handleSubmit} isLoading={isLoading} />
          </Grid>
        </Grid>
      </Paper>
    </Box>
  );
};
