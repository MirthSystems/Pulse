import React from 'react';
import { 
  Typography,
  Grid,
  Card,
  CardContent,
  CardMedia,
  Box,
  Button,
  Stack,
  Container,
  Chip
} from '@mui/material';
import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";
import { Link as RouterLink } from "react-router-dom";

// Icons
import LocalBarIcon from '@mui/icons-material/LocalBar';
import RestaurantIcon from '@mui/icons-material/Restaurant';
import MusicNoteIcon from '@mui/icons-material/MusicNote';
import LocationOnIcon from '@mui/icons-material/LocationOn';

// Sample feature data
const features = [
  {
    title: 'Real-Time Updates',
    description: 'Get live information about venues as it happens. No more outdated reviews.',
    icon: '🕒'
  },
  {
    title: 'Community-Sourced',
    description: 'Authentic insights from real people who are there right now.',
    icon: '👥'
  },
  {
    title: 'Special Offers',
    description: 'Discover time-sensitive deals, happy hours, and events nearby.',
    icon: '🎁'
  }
];

// Sample trending specials data
const trendingSpecials = [
  {
    id: 1,
    title: 'Half-Price Wings Night',
    venue: 'The Flying Duck',
    location: 'Downtown',
    tag: 'Wings Night',
    image: 'https://source.unsplash.com/random/300x200/?wings',
    icon: <RestaurantIcon fontSize="small" />
  },
  {
    id: 2,
    title: '$5 Craft Beer Special',
    venue: 'Hops & Barley',
    location: 'Westside',
    tag: 'Happy Hour',
    image: 'https://source.unsplash.com/random/300x200/?beer',
    icon: <LocalBarIcon fontSize="small" />
  },
  {
    id: 3,
    title: 'Live Jazz & Cocktails',
    venue: 'Blue Note Lounge',
    location: 'Arts District',
    tag: 'Live Music',
    image: 'https://source.unsplash.com/random/300x200/?jazz',
    icon: <MusicNoteIcon fontSize="small" />
  }
];

export default function Home() {
  return (
    <>
      {/* Features Section */}
      <Box sx={{ py: 8 }}>
        <Container>
          <Typography variant="h3" component="h2" align="center" gutterBottom fontWeight={600}>
            Find the Perfect Vibe
          </Typography>
          <Typography variant="h6" align="center" color="text.secondary" paragraph sx={{ mb: 6, maxWidth: 800, mx: 'auto' }}>
            Pulse gives you real-time insights about what's happening at venues around you right now, 
            not what happened last week or last month.
          </Typography>
          
          <Grid container spacing={4}>
            {features.map((feature, index) => (
              <Grid item key={index} xs={12} md={4}>
                <Card 
                  sx={{ 
                    height: '100%', 
                    display: 'flex', 
                    flexDirection: 'column',
                    border: 'none',
                    boxShadow: 'none',
                    bgcolor: 'transparent'
                  }}
                >
                  <Box
                    sx={{
                      display: 'flex',
                      justifyContent: 'center',
                      alignItems: 'center',
                      mb: 2,
                      fontSize: '3rem',
                    }}
                  >
                    {feature.icon}
                  </Box>
                  <CardContent sx={{ flexGrow: 1, textAlign: 'center' }}>
                    <Typography gutterBottom variant="h5" component="h3" fontWeight={600}>
                      {feature.title}
                    </Typography>
                    <Typography color="text.secondary">
                      {feature.description}
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Container>
      </Box>

      {/* Trending Specials Section */}
      <Box sx={{ py: 8, bgcolor: 'grey.50' }}>
        <Container>
          <Typography variant="h3" component="h2" align="center" gutterBottom fontWeight={600}>
            Trending Now
          </Typography>
          <Typography variant="h6" align="center" color="text.secondary" paragraph sx={{ mb: 6, maxWidth: 800, mx: 'auto' }}>
            Discover popular specials and events happening right now in your area.
          </Typography>
          
          <Grid container spacing={4}>
            {trendingSpecials.map((special) => (
              <Grid item key={special.id} xs={12} sm={6} md={4}>
                <Card 
                  sx={{ 
                    height: '100%', 
                    display: 'flex', 
                    flexDirection: 'column',
                    transition: '0.3s',
                    '&:hover': { 
                      transform: 'translateY(-8px)',
                      boxShadow: '0 12px 20px -10px rgba(0,0,0,0.1)'
                    },
                  }}
                >
                  <CardMedia
                    component="div"
                    sx={{
                      pt: '56.25%', // 16:9 aspect ratio
                      position: 'relative'
                    }}
                    image={special.image}
                  >
                    <Chip
                      icon={special.icon}
                      label={special.tag}
                      color="primary"
                      size="small"
                      sx={{ 
                        position: 'absolute', 
                        top: 12, 
                        right: 12,
                        bgcolor: 'rgba(255,255,255,0.9)',
                        color: 'primary.main',
                        '& .MuiChip-icon': {
                          color: 'primary.main',
                        }
                      }}
                    />
                  </CardMedia>
                  <CardContent sx={{ flexGrow: 1 }}>
                    <Typography gutterBottom variant="h5" component="h2" fontWeight={500}>
                      {special.title}
                    </Typography>
                    <Typography variant="body1" fontWeight={500} gutterBottom>
                      {special.venue}
                    </Typography>
                    <Stack direction="row" spacing={1} alignItems="center">
                      <LocationOnIcon fontSize="small" color="action" />
                      <Typography variant="body2" color="text.secondary">
                        {special.location}
                      </Typography>
                    </Stack>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
          
          <Box sx={{ mt: 6, textAlign: 'center' }}>
            <Button 
              variant="outlined" 
              color="primary" 
              size="large"
              component={RouterLink}
              to="/specials/list"
              sx={{ borderRadius: 28, px: 4 }}
            >
              View All Specials
            </Button>
          </Box>
        </Container>
      </Box>

      {/* CTA Section */}
      <Box 
        sx={{ 
          py: 10, 
          bgcolor: 'primary.main', 
          color: 'white',
          position: 'relative',
          overflow: 'hidden'
        }}
      >
        {/* Background pattern */}
        <Box 
          sx={{ 
            position: 'absolute', 
            top: 0, 
            left: 0, 
            right: 0, 
            bottom: 0, 
            opacity: 0.1,
            backgroundImage: 'radial-gradient(circle, rgba(255,255,255,0.3) 2px, transparent 0)',
            backgroundSize: '30px 30px',
          }} 
        />
        
        <Container>
          <AuthenticatedTemplate>
            <Typography variant="h3" align="center" gutterBottom>
              Ready to discover more?
            </Typography>
            <Typography variant="h6" align="center" paragraph sx={{ mb: 4, maxWidth: 800, mx: 'auto', opacity: 0.9 }}>
              Head to your dashboard to manage venues and specials.
            </Typography>
            <Box sx={{ display: 'flex', justifyContent: 'center' }}>
              <Button 
                variant="contained" 
                component={RouterLink} 
                to="/dashboard" 
                color="secondary"
                size="large"
                sx={{ 
                  borderRadius: 28,
                  px: 4,
                  py: 1.5,
                  fontSize: '1rem'
                }}
              >
                Go to Dashboard
              </Button>
            </Box>
          </AuthenticatedTemplate>

          <UnauthenticatedTemplate>
            <Typography variant="h3" align="center" gutterBottom>
              Ready to get started?
            </Typography>
            <Typography variant="h6" align="center" paragraph sx={{ mb: 4, maxWidth: 800, mx: 'auto', opacity: 0.9 }}>
              Sign in to save your favorite venues, receive notifications, and more.
            </Typography>
            <Box sx={{ display: 'flex', justifyContent: 'center' }}>
              <Button 
                variant="contained" 
                color="secondary"
                size="large"
                sx={{ 
                  borderRadius: 28,
                  px: 4,
                  py: 1.5,
                  fontSize: '1rem'
                }}
              >
                Sign Up Now
              </Button>
            </Box>
          </UnauthenticatedTemplate>
        </Container>
      </Box>
    </>
  );
}
