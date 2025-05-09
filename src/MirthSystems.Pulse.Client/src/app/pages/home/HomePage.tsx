import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Box, 
  Typography, 
  Paper,
  TextField,
  Button,
  Grid,
  MenuItem,
  FormControl,
  InputLabel,
  Select
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';

const HomePage = () => {
  const navigate = useNavigate();
  const [searchQuery, setSearchQuery] = useState('');
  const [specialType, setSpecialType] = useState('all');
  const [dayOfWeek, setDayOfWeek] = useState('any');

  const handleSearch = (event: React.FormEvent) => {
    event.preventDefault();
    // Add search params and navigate to search results page
    const searchParams = new URLSearchParams();
    if (searchQuery) searchParams.append('q', searchQuery);
    if (specialType !== 'all') searchParams.append('type', specialType);
    if (dayOfWeek !== 'any') searchParams.append('day', dayOfWeek);
    
    navigate(`/search?${searchParams.toString()}`);
  };

  return (
    <Box>
      <Box sx={{ textAlign: 'center', my: 4 }}>
        <Typography variant="h3" component="h1" gutterBottom>
          Welcome to Pulse
        </Typography>
        <Typography variant="h5" sx={{ mb: 4 }}>
          Find the best food and drink specials in your area
        </Typography>
      </Box>

      <Paper elevation={3} sx={{ p: 4, maxWidth: '800px', mx: 'auto' }}>
        <form onSubmit={handleSearch}>
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Search specials by name, venue or location"
                variant="outlined"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                InputProps={{
                  endAdornment: (
                    <SearchIcon color="action" />
                  ),
                }}
              />
            </Grid>
            
            <Grid item xs={12} md={6}>
              <FormControl fullWidth>
                <InputLabel id="special-type-label">Special Type</InputLabel>
                <Select
                  labelId="special-type-label"
                  id="special-type"
                  value={specialType}
                  label="Special Type"
                  onChange={(e) => setSpecialType(e.target.value)}
                >
                  <MenuItem value="all">All Types</MenuItem>
                  <MenuItem value="food">Food</MenuItem>
                  <MenuItem value="drink">Drink</MenuItem>
                  <MenuItem value="event">Event</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            
            <Grid item xs={12} md={6}>
              <FormControl fullWidth>
                <InputLabel id="day-of-week-label">Day of Week</InputLabel>
                <Select
                  labelId="day-of-week-label"
                  id="day-of-week"
                  value={dayOfWeek}
                  label="Day of Week"
                  onChange={(e) => setDayOfWeek(e.target.value)}
                >
                  <MenuItem value="any">Any Day</MenuItem>
                  <MenuItem value="monday">Monday</MenuItem>
                  <MenuItem value="tuesday">Tuesday</MenuItem>
                  <MenuItem value="wednesday">Wednesday</MenuItem>
                  <MenuItem value="thursday">Thursday</MenuItem>
                  <MenuItem value="friday">Friday</MenuItem>
                  <MenuItem value="saturday">Saturday</MenuItem>
                  <MenuItem value="sunday">Sunday</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            
            <Grid item xs={12} sx={{ textAlign: 'center' }}>
              <Button 
                type="submit" 
                variant="contained" 
                size="large"
                startIcon={<SearchIcon />}
              >
                Search Specials
              </Button>
            </Grid>
          </Grid>
        </form>
      </Paper>
    </Box>
  );
};

export default HomePage;