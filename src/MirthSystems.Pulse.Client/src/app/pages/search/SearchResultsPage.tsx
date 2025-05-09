import { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
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
  Select,
  List,
  ListItem,
  Card,
  CardContent,
  CardActions,
  Divider,
  Chip,
  CircularProgress
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import PlaceIcon from '@mui/icons-material/Place';
import ScheduleIcon from '@mui/icons-material/Schedule';
import LocalOfferIcon from '@mui/icons-material/LocalOffer';

// Mock data for demonstration
const mockSpecials = [
  {
    id: 1,
    title: 'Half-Price Wings',
    description: 'Enjoy our signature wings at half price all day!',
    venue: 'The Local Pub & Grill',
    location: 'Downtown',
    days: ['monday', 'wednesday'],
    type: 'food'
  },
  {
    id: 2,
    title: '$5 Craft Beer',
    description: 'All craft beers just $5 during happy hour',
    venue: 'Brew Haven',
    location: 'Midtown',
    days: ['tuesday', 'thursday', 'friday'],
    type: 'drink'
  },
  {
    id: 3,
    title: 'Live Music + Dinner Special',
    description: 'Enjoy live music with a special dinner menu',
    venue: 'The Jazz Lounge',
    location: 'Uptown',
    days: ['saturday'],
    type: 'event'
  }
];

const SearchResultsPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const [searchQuery, setSearchQuery] = useState(searchParams.get('q') || '');
  const [specialType, setSpecialType] = useState(searchParams.get('type') || 'all');
  const [dayOfWeek, setDayOfWeek] = useState(searchParams.get('day') || 'any');
  
  const [specials, setSpecials] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Simulate API fetch with a small delay
    setLoading(true);
    
    setTimeout(() => {
      // Filter mock specials based on search parameters
      let filteredSpecials = [...mockSpecials];
      
      if (searchQuery) {
        const query = searchQuery.toLowerCase();
        filteredSpecials = filteredSpecials.filter(special => 
          special.title.toLowerCase().includes(query) || 
          special.venue.toLowerCase().includes(query) || 
          special.location.toLowerCase().includes(query) ||
          special.description.toLowerCase().includes(query)
        );
      }
      
      if (specialType !== 'all') {
        filteredSpecials = filteredSpecials.filter(special => 
          special.type === specialType
        );
      }
      
      if (dayOfWeek !== 'any') {
        filteredSpecials = filteredSpecials.filter(special => 
          special.days.includes(dayOfWeek)
        );
      }
      
      setSpecials(filteredSpecials);
      setLoading(false);
    }, 800);
  }, [searchParams]);

  const handleSearch = (event: React.FormEvent) => {
    event.preventDefault();
    // Update search params
    const params = new URLSearchParams();
    if (searchQuery) params.append('q', searchQuery);
    if (specialType !== 'all') params.append('type', specialType);
    if (dayOfWeek !== 'any') params.append('day', dayOfWeek);
    
    setSearchParams(params);
  };

  return (
    <Box>
      {/* Search Form */}
      <Paper elevation={2} sx={{ p: 3, mb: 4 }}>
        <form onSubmit={handleSearch}>
          <Grid container spacing={2} alignItems="center">
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                label="Search"
                variant="outlined"
                size="small"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </Grid>
            
            <Grid item xs={6} md={3}>
              <FormControl fullWidth size="small">
                <InputLabel id="special-type-label">Type</InputLabel>
                <Select
                  labelId="special-type-label"
                  id="special-type"
                  value={specialType}
                  label="Type"
                  onChange={(e) => setSpecialType(e.target.value)}
                >
                  <MenuItem value="all">All Types</MenuItem>
                  <MenuItem value="food">Food</MenuItem>
                  <MenuItem value="drink">Drink</MenuItem>
                  <MenuItem value="event">Event</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            
            <Grid item xs={6} md={3}>
              <FormControl fullWidth size="small">
                <InputLabel id="day-of-week-label">Day</InputLabel>
                <Select
                  labelId="day-of-week-label"
                  id="day-of-week"
                  value={dayOfWeek}
                  label="Day"
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
            
            <Grid item xs={12} md={2}>
              <Button 
                type="submit" 
                variant="contained" 
                fullWidth
                startIcon={<SearchIcon />}
              >
                Search
              </Button>
            </Grid>
          </Grid>
        </form>
      </Paper>

      {/* Search Results */}
      <Box>
        <Typography variant="h5" component="h2" gutterBottom>
          {loading ? 'Searching...' : `Search Results (${specials.length})`}
        </Typography>

        {loading ? (
          <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
            <CircularProgress />
          </Box>
        ) : specials.length === 0 ? (
          <Paper elevation={2} sx={{ p: 4, textAlign: 'center' }}>
            <Typography variant="h6">No specials found</Typography>
            <Typography color="textSecondary">
              Try adjusting your search criteria
            </Typography>
          </Paper>
        ) : (
          <List>
            {specials.map((special) => (
              <ListItem key={special.id} sx={{ mb: 2, p: 0 }}>
                <Card variant="outlined" sx={{ width: '100%' }}>
                  <CardContent>
                    <Grid container spacing={2}>
                      <Grid item xs={12}>
                        <Typography variant="h6" component="div">
                          {special.title}
                        </Typography>
                        <Typography color="textSecondary" gutterBottom>
                          <PlaceIcon sx={{ fontSize: 16, verticalAlign: 'middle', mr: 0.5 }} />
                          {special.venue} • {special.location}
                        </Typography>
                      </Grid>
                      
                      <Grid item xs={12}>
                        <Typography variant="body2">
                          {special.description}
                        </Typography>
                      </Grid>
                      
                      <Grid item xs={12}>
                        <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                          <Chip 
                            size="small" 
                            icon={<LocalOfferIcon />} 
                            label={special.type.charAt(0).toUpperCase() + special.type.slice(1)} 
                          />
                          <Chip 
                            size="small" 
                            icon={<ScheduleIcon />} 
                            label={special.days.map(day => day.charAt(0).toUpperCase() + day.slice(1)).join(', ')} 
                          />
                        </Box>
                      </Grid>
                    </Grid>
                  </CardContent>
                  <CardActions>
                    <Button size="small">View Details</Button>
                  </CardActions>
                </Card>
              </ListItem>
            ))}
          </List>
        )}
      </Box>
    </Box>
  );
};

export default SearchResultsPage;