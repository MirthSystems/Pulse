import React from 'react';
import { 
  Typography, 
  Container, 
  Grid, 
  Card, 
  CardContent, 
  CardMedia, 
  Chip, 
  Box,
  Stack
} from '@mui/material';
import { Special } from '../types/special';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import { getTagIcon } from '../utils/getTagIcon';

// Sample data - would come from API in a real implementation
const sampleSpecials: Special[] = [
  {
    id: 1,
    title: 'Half-Price Wings Night',
    venue: 'The Flying Duck',
    location: 'Downtown',
    tag: 'Wings Night',
    image: 'https://source.unsplash.com/random/300x200/?wings',
    status: 'active',
    views: 245
  },
  {
    id: 2,
    title: '$5 Craft Beer Special',
    venue: 'Hops & Barley',
    location: 'Westside',
    tag: 'Happy Hour',
    image: 'https://source.unsplash.com/random/300x200/?beer',
    status: 'active',
    views: 189
  },
  {
    id: 3,
    title: 'Live Jazz & Cocktails',
    venue: 'Blue Note Lounge',
    location: 'Arts District',
    tag: 'Live Music',
    image: 'https://source.unsplash.com/random/300x200/?jazz',
    status: 'active',
    views: 312
  },
  {
    id: 4,
    title: 'Taco Tuesday 2-for-1',
    venue: 'El Camino',
    location: 'Midtown',
    tag: 'Taco Tuesday',
    image: 'https://source.unsplash.com/random/300x200/?tacos',
    status: 'scheduled',
    scheduledDate: '2025-05-06'
  },
  {
    id: 5,
    title: 'Wine Down Wednesday',
    venue: 'The Vineyard',
    location: 'North End',
    tag: 'Wine Tasting',
    image: 'https://source.unsplash.com/random/300x200/?wine',
    status: 'scheduled',
    scheduledDate: '2025-05-07'
  },
  {
    id: 6,
    title: 'Thursday Trivia Night',
    venue: 'Brainy Brew',
    location: 'College District',
    tag: 'Trivia',
    image: 'https://source.unsplash.com/random/300x200/?trivia',
    status: 'active',
    views: 178
  }
];

const SpecialsList: React.FC = () => {
  return (
    <Container maxWidth="lg" sx={{ py: 8 }}>
      <Typography variant="h3" component="h1" gutterBottom fontWeight={600}>
        Specials Near You
      </Typography>
      <Typography variant="h6" component="h2" color="text.secondary" paragraph sx={{ mb: 6 }}>
        Check out these popular special offers happening in your area
      </Typography>
      
      <Grid container spacing={4}>
        {sampleSpecials.map((special) => (
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
                  icon={getTagIcon(special.tag)}
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
                {special.status === 'scheduled' && (
                  <Box
                    sx={{
                      position: 'absolute',
                      top: 12,
                      left: 12,
                      bgcolor: 'rgba(0,0,0,0.7)',
                      color: 'white',
                      borderRadius: 1,
                      px: 1,
                      py: 0.5,
                      fontSize: '0.75rem'
                    }}
                  >
                    Coming {special.scheduledDate}
                  </Box>
                )}
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
                {special.status === 'active' && special.views && (
                  <Box sx={{ mt: 2, display: 'flex', justifyContent: 'flex-end' }}>
                    <Chip 
                      label={`${special.views} views`} 
                      size="small" 
                      variant="outlined" 
                      color="primary"
                    />
                  </Box>
                )}
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
};

export default SpecialsList;