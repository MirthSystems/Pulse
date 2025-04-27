import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  TextField,
  Button,
  InputAdornment,
  FormControl,
  Select,
  MenuItem,
  Paper,
  Typography,
  Grid,
  Chip,
  SelectChangeEvent
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import LocalBarIcon from '@mui/icons-material/LocalBar';
import RestaurantIcon from '@mui/icons-material/Restaurant';
import MusicNoteIcon from '@mui/icons-material/MusicNote';
import { VenueType } from '../../types/venue-type';
import { SpecialSearchParams } from '../../types/special-search-params';

// Venue types for filter options
const venueTypes = [
  { value: 'all', label: 'All Venues' },
  { value: 'bar', label: 'Bars' },
  { value: 'restaurant', label: 'Restaurants' },
  { value: 'cafe', label: 'Cafés' },
  { value: 'club', label: 'Clubs' },
  { value: 'brewery', label: 'Breweries' },
  { value: 'concert', label: 'Concert Venues' }
];

// Popular tags for quick search
const popularTags = [
  { id: 'happyhour', label: 'Happy Hour', icon: <LocalBarIcon fontSize="small" /> },
  { id: 'livemusic', label: 'Live Music', icon: <MusicNoteIcon fontSize="small" /> },
  { id: 'wingsnight', label: 'Wings Night', icon: <RestaurantIcon fontSize="small" /> },
  { id: 'tacotuesday', label: 'Taco Tuesday', icon: <RestaurantIcon fontSize="small" /> }
];

const SpecialsSearch: React.FC = () => {
  const navigate = useNavigate();
  const [location, setLocation] = useState('');
  const [keyword, setKeyword] = useState('');
  const [venueType, setVenueType] = useState<VenueType>('all');
  const [selectedTags, setSelectedTags] = useState<string[]>([]);

  const handleVenueTypeChange = (event: SelectChangeEvent) => {
    setVenueType(event.target.value as VenueType);
  };

  const handleTagClick = (tagId: string) => {
    if (selectedTags.includes(tagId)) {
      setSelectedTags(selectedTags.filter(tag => tag !== tagId));
    } else {
      setSelectedTags([...selectedTags, tagId]);
    }
  };

  const handleSearch = (event: React.FormEvent) => {
    event.preventDefault();
    
    // Prepare search parameters
    const searchParams: SpecialSearchParams = {
      location: location || undefined,
      keyword: keyword || undefined,
      venueType: venueType !== 'all' ? venueType : undefined,
      tags: selectedTags.length > 0 ? selectedTags : undefined
    };
    
    // In a real implementation, you would pass these params to your search API
    console.log('Search params:', searchParams);
    
    // Navigate to specials list page
    navigate('/specials/list');
  };

  const handleUseMyLocation = () => {
    // This would normally trigger geolocation API
    setLocation('Current Location');
  };

  return (
    <Paper 
      elevation={0} 
      sx={{ 
        p: { xs: 2, sm: 4 }, 
        borderRadius: 4, 
        backgroundColor: 'rgba(255, 255, 255, 0.9)',
        backdropFilter: 'blur(5px)',
        border: '1px solid rgba(0, 0, 0, 0.05)'
      }}
    >
      <Typography variant="h5" component="h2" gutterBottom sx={{ fontWeight: 600, mb: 3 }}>
        Find Specials Happening Now
      </Typography>
      
      <Box component="form" onSubmit={handleSearch} noValidate>
        <Grid container spacing={2}>
          <Grid item xs={12} md={4}>
            <TextField
              fullWidth
              value={location}
              onChange={(e) => setLocation(e.target.value)}
              placeholder="Enter location"
              variant="outlined"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <LocationOnIcon color="action" />
                  </InputAdornment>
                ),
                endAdornment: (
                  <InputAdornment position="end">
                    <Button 
                      onClick={handleUseMyLocation}
                      variant="text" 
                      size="small"
                      sx={{ minWidth: 0, whiteSpace: 'nowrap', fontSize: '0.75rem' }}
                    >
                      Use my location
                    </Button>
                  </InputAdornment>
                ),
              }}
            />
          </Grid>
          
          <Grid item xs={12} md={3}>
            <FormControl fullWidth>
              <Select
                value={venueType}
                onChange={handleVenueTypeChange}
                displayEmpty
                variant="outlined"
              >
                {venueTypes.map((type) => (
                  <MenuItem key={type.value} value={type.value}>
                    {type.label}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>
          
          <Grid item xs={12} md={3}>
            <TextField
              fullWidth
              value={keyword}
              onChange={(e) => setKeyword(e.target.value)}
              placeholder="Search by keyword, tag or vibe"
              variant="outlined"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon color="action" />
                  </InputAdornment>
                ),
              }}
            />
          </Grid>
          
          <Grid item xs={12} md={2}>
            <Button 
              type="submit"
              variant="contained" 
              color="primary"
              fullWidth
              size="large"
              sx={{ height: '100%' }}
            >
              Search
            </Button>
          </Grid>
          
          <Grid item xs={12}>
            <Box sx={{ mt: 2, display: 'flex', flexWrap: 'wrap', gap: 1 }}>
              <Typography variant="body2" sx={{ mr: 1, mt: 0.5 }}>
                Popular:
              </Typography>
              {popularTags.map((tag) => (
                <Chip
                  key={tag.id}
                  icon={tag.icon}
                  label={tag.label}
                  onClick={() => handleTagClick(tag.id)}
                  color={selectedTags.includes(tag.id) ? "primary" : "default"}
                  variant={selectedTags.includes(tag.id) ? "filled" : "outlined"}
                  sx={{ 
                    borderRadius: 2,
                    '& .MuiChip-icon': {
                      color: selectedTags.includes(tag.id) ? 'inherit' : undefined
                    }
                  }}
                />
              ))}
            </Box>
          </Grid>
        </Grid>
      </Box>
    </Paper>
  );
};

export default SpecialsSearch;