import { useState, useEffect } from 'react';
import { 
  Box, 
  TextField, 
  MenuItem, 
  FormControlLabel, 
  Checkbox,
  IconButton,
  InputAdornment,
  Collapse,
  Paper
} from '@mui/material';
import { Grid } from '@mui/material';
import FilterListIcon from '@mui/icons-material/FilterList';
import SearchIcon from '@mui/icons-material/Search';
import CloseIcon from '@mui/icons-material/Close';
import { type GetVenuesRequest } from '../../models';

interface VenueListFilterProps {
  onChange: (filters: GetVenuesRequest) => void;
  initialFilters?: GetVenuesRequest;
}

export const VenueListFilter = ({ onChange, initialFilters }: VenueListFilterProps) => {
  const [filtersExpanded, setFiltersExpanded] = useState(false);
  const [filters, setFilters] = useState<GetVenuesRequest>(initialFilters || {
    page: 1,
    pageSize: 10,
    searchText: '',
    address: '',
    radiusInMiles: 10,
    hasActiveSpecials: false
  });
  
  const handleFilterChange = (field: keyof GetVenuesRequest, value: string | number | boolean) => {
    const newFilters = { ...filters, [field]: value };
    setFilters(newFilters);
  };
  
  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    onChange(filters);
  };
  
  const clearFilters = () => {
    const resetFilters = {
      page: 1,
      pageSize: 10,
      searchText: '',
      address: '',
      radiusInMiles: 10,
      hasActiveSpecials: false
    };
    setFilters(resetFilters);
    onChange(resetFilters);
  };
  
  useEffect(() => {
    // When initialFilters change externally, update local state
    if (initialFilters) {
      setFilters(initialFilters);
    }
  }, [initialFilters]);
  
  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%' }}>
      {/* Always visible search field */}
      <Box sx={{ display: 'flex', mb: 2 }}>
        <TextField
          fullWidth
          placeholder="Search venues..."
          variant="outlined"
          size="small"
          value={filters.searchText || ''}
          onChange={(e) => handleFilterChange('searchText', e.target.value)}
          slotProps={{
            input: {
              startAdornment: (
                <InputAdornment position="start">
                  <SearchIcon />
                </InputAdornment>
              ),
              endAdornment: filters.searchText ? (
                <InputAdornment position="end">
                  <IconButton
                    size="small"
                    onClick={() => handleFilterChange('searchText', '')}
                  >
                    <CloseIcon fontSize="small" />
                  </IconButton>
                </InputAdornment>
              ) : null
            }
          }}
        />
        <IconButton 
          sx={{ ml: 1 }}
          onClick={() => setFiltersExpanded(!filtersExpanded)}
          color={filtersExpanded ? 'primary' : 'default'}
        >
          <FilterListIcon />
        </IconButton>
      </Box>
      
      {/* Expandable advanced filters */}
      <Collapse in={filtersExpanded}>
        <Paper sx={{ p: 2, mb: 2 }}>
          <Grid container spacing={2}>
            <Grid gridSize={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Location"
                placeholder="City, state or zip"
                size="small"
                value={filters.address || ''}
                onChange={(e) => handleFilterChange('address', e.target.value)}
              />
            </Grid>
            
            <Grid gridSize={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Radius (miles)"
                type="number"
                size="small"
                value={filters.radiusInMiles || ''}
                onChange={(e) => handleFilterChange('radiusInMiles', parseInt(e.target.value) || 0)}
                inputProps={{ min: 0, max: 100 }}
              />
            </Grid>
            
            <Grid gridSize={{ xs: 12, sm: 6 }}>
              <TextField
                select
                fullWidth
                label="Open on day"
                size="small"
                value={filters.openOnDayOfWeek || ''}
                onChange={(e) => handleFilterChange('openOnDayOfWeek', parseInt(e.target.value))}
              >
                <MenuItem value="">Any day</MenuItem>
                <MenuItem value={0}>Sunday</MenuItem>
                <MenuItem value={1}>Monday</MenuItem>
                <MenuItem value={2}>Tuesday</MenuItem>
                <MenuItem value={3}>Wednesday</MenuItem>
                <MenuItem value={4}>Thursday</MenuItem>
                <MenuItem value={5}>Friday</MenuItem>
                <MenuItem value={6}>Saturday</MenuItem>
              </TextField>
            </Grid>
            
            <Grid gridSize={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Open at time"
                type="time"
                size="small"
                value={filters.timeOfDay || ''}
                onChange={(e) => handleFilterChange('timeOfDay', e.target.value)}
                slotProps={{
                  input: {},
                  inputLabel: { shrink: true }
                }}
              />
            </Grid>
            
            <Grid gridSize={{ xs: 12 }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <FormControlLabel 
                  control={
                    <Checkbox 
                      checked={filters.hasActiveSpecials || false} 
                      onChange={(e) => handleFilterChange('hasActiveSpecials', e.target.checked)}
                    />
                  }
                  label="Has active specials"
                />
                
                <Box>
                  <IconButton size="small" onClick={clearFilters}>
                    <CloseIcon fontSize="small" />
                  </IconButton>
                </Box>
              </Box>
            </Grid>
          </Grid>
        </Paper>
      </Collapse>
    </Box>
  );
};
