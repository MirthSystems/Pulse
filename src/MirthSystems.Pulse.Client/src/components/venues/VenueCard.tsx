import { Card, CardMedia, CardContent, CardActionArea, Typography, Box, Chip, Stack } from '@mui/material';
import { Place as PlaceIcon, LocalBar as DrinkIcon, Restaurant as FoodIcon, MusicNote as EntertainmentIcon } from '@mui/icons-material';
import { VenueItem } from '@models/venue';
import { SpecialItem, SpecialTypes } from '@models/special';

interface VenueCardProps {
  venue: VenueItem;
  specials?: SpecialItem[];
  onClick?: () => void;
}

const VenueCard = ({ venue, specials, onClick }: VenueCardProps) => {
  const defaultImage = '/img/default-venue.jpg';
  
  // Group specials by type if provided
  const hasSpecials = specials && specials.length > 0;
  const specialsByType = hasSpecials 
    ? specials?.reduce((acc, special) => {
        const key = special.type as number;
        acc[key] = (acc[key] || 0) + 1;
        return acc;
      }, {} as Record<number, number>)
    : {};
  
  return (
    <Card 
      elevation={2} 
      sx={{ 
        height: '100%', 
        display: 'flex', 
        flexDirection: 'column',
        transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: 6,
        }
      }}
    >
      <CardActionArea onClick={onClick} sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column', alignItems: 'stretch' }}>
        <CardMedia
          component="img"
          height="160"
          image={venue.profileImage || defaultImage}
          alt={venue.name}
          sx={{ objectFit: 'cover' }}
        />
        <CardContent sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
          <Typography variant="h6" component="div" gutterBottom noWrap>
            {venue.name}
          </Typography>
          
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
            <PlaceIcon fontSize="small" color="action" sx={{ mr: 0.5 }} />
            <Typography variant="body2" color="text.secondary" noWrap>
              {venue.locality}, {venue.region}
            </Typography>
          </Box>
          
          {venue.description && (
            <Typography 
              variant="body2" 
              color="text.secondary"
              sx={{
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                display: '-webkit-box',
                WebkitLineClamp: 2,
                WebkitBoxOrient: 'vertical',
                mb: 1.5,
              }}
            >
              {venue.description}
            </Typography>
          )}
          
          {hasSpecials && (
            <Box sx={{ mt: 'auto' }}>
              <Typography variant="subtitle2" sx={{ mb: 1 }}>
                Current Specials
              </Typography>
              <Stack direction="row" spacing={1}>
                {specialsByType[SpecialTypes.Drink] && (
                  <Chip 
                    icon={<DrinkIcon />} 
                    label={`${specialsByType[SpecialTypes.Drink]} Drink`} 
                    size="small" 
                    color="secondary" 
                    variant="outlined"
                  />
                )}
                {specialsByType[SpecialTypes.Food] && (
                  <Chip 
                    icon={<FoodIcon />} 
                    label={`${specialsByType[SpecialTypes.Food]} Food`} 
                    size="small" 
                    color="primary" 
                    variant="outlined"
                  />
                )}
                {specialsByType[SpecialTypes.Entertainment] && (
                  <Chip 
                    icon={<EntertainmentIcon />} 
                    label={`${specialsByType[SpecialTypes.Entertainment]} Entertainment`} 
                    size="small" 
                    color="info" 
                    variant="outlined"
                  />
                )}
              </Stack>
            </Box>
          )}
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default VenueCard;
