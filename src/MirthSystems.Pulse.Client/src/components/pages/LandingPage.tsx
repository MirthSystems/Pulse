import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Box, 
  Typography, 
  Paper, 
  TextField, 
  Button, 
  Grid,
  Container,
  InputAdornment,
  IconButton,
  Slider,
  FormControl,
  InputLabel,
  Select,
  MenuItem
} from '@mui/material';
import { Search as SearchIcon, MyLocation as MyLocationIcon } from '@mui/icons-material';
import { SpecialTypes } from '@models/special';

const LandingPage = () => {
  const navigate = useNavigate();
  
  const [address, setAddress] = useState<string>('');
  const [radius, setRadius] = useState<number>(5);
  const [specialType, setSpecialType] = useState<number | string>('');
  const [searchTerm, setSearchTerm] = useState<string>('');

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!address) return;
    
    const searchParams = new URLSearchParams();
    searchParams.set('address', address);
    searchParams.set('radius', radius.toString());
    if (specialType) searchParams.set('type', specialType.toString());
    if (searchTerm) searchParams.set('term', searchTerm);
    searchParams.set('active', 'true');
    searchParams.set('page', '1');
    
    navigate(`/results?${searchParams.toString()}`);
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

  return (
    <Container maxWidth="lg">
      <Box sx={{ 
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        minHeight: '70vh',
        textAlign: 'center',
        py: 4
      }}>
        <Typography variant="h2" component="h1" gutterBottom sx={{ fontWeight: 'bold' }}>
          Discover Special Events & Promotions
        </Typography>
        
        <Typography variant="h5" color="text.secondary" sx={{ mb: 6, maxWidth: 700 }}>
          Find the best deals, happy hours, and events near you
        </Typography>
        
        <Paper 
          elevation={3}
          sx={{ 
            p: 4, 
            width: '100%', 
            maxWidth: 800,
            borderRadius: 2,
            backgroundColor: 'white',
          }}
          component="form"
          onSubmit={handleSearch}
        >
          <Grid container spacing={3}>
            <Grid item xs={12} md={12}>
              <TextField
                fullWidth
                required
                label="Location"
                placeholder="Enter address, city, or ZIP"
                value={address}
                onChange={(e) => setAddress(e.target.value)}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton onClick={handleGetCurrentLocation}>
                        <MyLocationIcon />
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
            </Grid>
            
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                label="Keywords"
                placeholder="e.g. happy hour, pizza"
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
            
            <Grid item xs={12} md={4}>
              <FormControl fullWidth>
                <InputLabel>Special Type</InputLabel>
                <Select
                  value={specialType}
                  label="Special Type"
                  onChange={(e) => setSpecialType(e.target.value)}
                >
                  <MenuItem value="">Any Type</MenuItem>
                  <MenuItem value={SpecialTypes.Food}>Food</MenuItem>
                  <MenuItem value={SpecialTypes.Drink}>Drink</MenuItem>
                  <MenuItem value={SpecialTypes.Entertainment}>Entertainment</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            
            <Grid item xs={12} md={4}>
              <Button 
                fullWidth
                type="submit"
                variant="contained" 
                color="primary"
                size="large"
                sx={{ 
                  height: '56px',
                }}
                disabled={!address}
              >
                Search Specials
              </Button>
            </Grid>
            
            <Grid item xs={12}>
              <Typography gutterBottom>Search Radius: {radius} miles</Typography>
              <Slider
                value={radius}
                onChange={(_, value) => setRadius(value as number)}
                min={1}
                max={25}
                valueLabelDisplay="auto"
              />
            </Grid>
          </Grid>
        </Paper>
        
        <Box sx={{ mt: 8, width: '100%' }}>
          <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
            Why Use Pulse?
          </Typography>
          
          <Grid container spacing={4}>
            <Grid item xs={12} md={4}>
              <Paper sx={{ p: 3, height: '100%' }} elevation={2}>
                <Typography variant="h6" gutterBottom>
                  Discover New Places
                </Typography>
                <Typography>
                  Find hidden gems and popular spots around you with our comprehensive venue database.
                </Typography>
              </Paper>
            </Grid>
            
            <Grid item xs={12} md={4}>
              <Paper sx={{ p: 3, height: '100%' }} elevation={2}>
                <Typography variant="h6" gutterBottom>
                  Never Miss a Deal
                </Typography>
                <Typography>
                  Stay updated on the latest promotions, happy hours, and special events in your area.
                </Typography>
              </Paper>
            </Grid>
            
            <Grid item xs={12} md={4}>
              <Paper sx={{ p: 3, height: '100%' }} elevation={2}>
                <Typography variant="h6" gutterBottom>
                  Plan Your Evening
                </Typography>
                <Typography>
                  Find venues open now with current specials to make the most of your night out.
                </Typography>
              </Paper>
            </Grid>
          </Grid>
        </Box>
      </Box>
    </Container>
  );
};

export default LandingPage;
