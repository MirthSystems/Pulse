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
  MenuItem,
  Card,
  CardContent,
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
          Find Tonight's Best Specials
        </Typography>
        
        <Typography variant="h5" color="text.secondary" sx={{ mb: 6, maxWidth: 700 }}>
          Discover food, drink, and entertainment specials at venues near you
        </Typography>
        
        <Paper 
          elevation={3}
          sx={{ 
            p: 4, 
            width: '100%', 
            maxWidth: 800,
            borderRadius: 2,
            background: theme => `linear-gradient(45deg, ${theme.palette.primary.dark} 0%, ${theme.palette.primary.main} 50%, ${theme.palette.secondary.main} 100%)`
          }}
          component="form"
          onSubmit={handleSearch}
        >
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                required
                label="Your Location"
                placeholder="Address, City, or ZIP"
                value={address}
                onChange={(e) => setAddress(e.target.value)}
                variant="filled"
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton onClick={handleGetCurrentLocation} sx={{ color: 'white' }}>
                        <MyLocationIcon />
                      </IconButton>
                    </InputAdornment>
                  ),
                  style: { color: 'white' }
                }}
                InputLabelProps={{ style: { color: 'rgba(255,255,255,0.8)' } }}
                sx={{
                  '& .MuiFilledInput-root': {
                    bgcolor: 'rgba(255,255,255,0.15)',
                    '&:hover': {
                      bgcolor: 'rgba(255,255,255,0.25)',
                    },
                    '&.Mui-focused': {
                      bgcolor: 'rgba(255,255,255,0.25)',
                    }
                  }
                }}
              />
            </Grid>
            
            <Grid item xs={12} md={4}>
              <Typography color="white" gutterBottom>
                Search Radius: {radius} miles
              </Typography>
              <Slider
                value={radius}
                onChange={(_, value) => setRadius(value as number)}
                min={1}
                max={25}
                valueLabelDisplay="auto"
                sx={{ color: 'white' }}
              />
            </Grid>
            
            <Grid item xs={12} md={4}>
              <FormControl variant="filled" fullWidth sx={{
                '& .MuiFilledInput-root': {
                  bgcolor: 'rgba(255,255,255,0.15)',
                  '&:hover': {
                    bgcolor: 'rgba(255,255,255,0.25)',
                  },
                  '&.Mui-focused': {
                    bgcolor: 'rgba(255,255,255,0.25)',
                  }
                }
              }}>
                <InputLabel sx={{ color: 'rgba(255,255,255,0.8)' }}>Special Type</InputLabel>
                <Select
                  value={specialType}
                  onChange={(e) => setSpecialType(e.target.value)}
                  inputProps={{ style: { color: 'white' } }}
                >
                  <MenuItem value="">Any Type</MenuItem>
                  <MenuItem value={SpecialTypes.Food}>Food</MenuItem>
                  <MenuItem value={SpecialTypes.Drink}>Drink</MenuItem>
                  <MenuItem value={SpecialTypes.Entertainment}>Entertainment</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                label="Keywords"
                placeholder="e.g. happy hour, pizza"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                variant="filled"
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <SearchIcon sx={{ color: 'rgba(255,255,255,0.8)' }} />
                    </InputAdornment>
                  ),
                  style: { color: 'white' }
                }}
                InputLabelProps={{ style: { color: 'rgba(255,255,255,0.8)' } }}
                sx={{
                  '& .MuiFilledInput-root': {
                    bgcolor: 'rgba(255,255,255,0.15)',
                    '&:hover': {
                      bgcolor: 'rgba(255,255,255,0.25)',
                    },
                    '&.Mui-focused': {
                      bgcolor: 'rgba(255,255,255,0.25)',
                    }
                  }
                }}
              />
            </Grid>
            
            <Grid item xs={12}>
              <Button 
                type="submit"
                variant="contained" 
                size="large"
                fullWidth
                color="secondary"
                sx={{ 
                  py: 1.5,
                  fontSize: '1.2rem',
                  boxShadow: 4,
                  '&:hover': {
                    boxShadow: 6,
                  }
                }}
                disabled={!address}
              >
                Find Specials
              </Button>
            </Grid>
          </Grid>
        </Paper>
        
        <Box sx={{ mt: 8, width: '100%' }}>
          <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
            Why Use Pulse?
          </Typography>
          
          <Grid container spacing={4}>
            <Grid item xs={12} md={4}>
              <Card sx={{ height: '100%' }}>
                <CardContent>
                  <Typography variant="h5" gutterBottom>
                    Discover New Places
                  </Typography>
                  <Typography>
                    Find hidden gems and popular spots around you with our comprehensive venue database.
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            
            <Grid item xs={12} md={4}>
              <Card sx={{ height: '100%' }}>
                <CardContent>
                  <Typography variant="h5" gutterBottom>
                    Never Miss a Deal
                  </Typography>
                  <Typography>
                    Stay updated on the latest promotions, happy hours, and special events in your area.
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            
            <Grid item xs={12} md={4}>
              <Card sx={{ height: '100%' }}>
                <CardContent>
                  <Typography variant="h5" gutterBottom>
                    Plan Your Evening
                  </Typography>
                  <Typography>
                    Find venues open now with current specials to make the most of your night out.
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
        </Box>
      </Box>
    </Container>
  );
};

export default LandingPage;
