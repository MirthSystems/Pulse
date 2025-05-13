import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Box, 
  Card, 
  CardContent, 
  Typography, 
  Chip, 
  Grid, 
  Divider,
  Button
} from '@mui/material';
import { LocalBar as DrinkIcon, Restaurant as FoodIcon, MusicNote as EntertainmentIcon, DeviceUnknown as UnknownIcon } from '@mui/icons-material';
import { SpecialItem, SpecialTypes } from '@models/special';
import { DateTime } from 'luxon';

interface SpecialsListProps {
  specials: SpecialItem[];
  showVenueName?: boolean;
}

const SpecialsList = ({ specials, showVenueName = false }: SpecialsListProps) => {
  const navigate = useNavigate();
  
  if (!specials || specials.length === 0) {
    return (
      <Box sx={{ textAlign: 'center', py: 2 }}>
        <Typography color="text.secondary">No specials available</Typography>
      </Box>
    );
  }

  const getSpecialIcon = (type: SpecialTypes) => {
    switch (type) {
      case SpecialTypes.Drink:
        return <DrinkIcon />;
      case SpecialTypes.Food:
        return <FoodIcon />;
      case SpecialTypes.Entertainment:
        return <EntertainmentIcon />;
      default:
        return <UnknownIcon />;
    }
  };

  const getSpecialColor = (type: SpecialTypes) => {
    switch (type) {
      case SpecialTypes.Drink:
        return 'secondary';
      case SpecialTypes.Food:
        return 'primary';
      case SpecialTypes.Entertainment:
        return 'info';
      default:
        return 'default';
    }
  };

  const formatTime = (timeString: string): string => {
    try {
      const [hours, minutes] = timeString.split(':').map(Number);
      return DateTime.fromObject({ hour: hours, minute: minutes }).toFormat('h:mm a');
    } catch (error) {
      return timeString;
    }
  };

  const formatTimeRange = (special: SpecialItem): string => {
    const start = formatTime(special.startTime);
    const end = special.endTime ? formatTime(special.endTime) : null;
    
    return end ? `${start} - ${end}` : `Starting ${start}`;
  };

  const handleSpecialClick = (id: string) => {
    navigate(`/specials/${id}`);
  };

  return (
    <Grid container spacing={2}>
      {specials.map((special) => (
        <Grid item xs={12} md={6} key={special.id}>
          <Card 
            variant="outlined" 
            sx={{ 
              height: '100%',
              cursor: 'pointer',
              transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out',
              '&:hover': {
                transform: 'translateY(-2px)',
                boxShadow: 2
              }
            }}
            onClick={() => handleSpecialClick(special.id as string)}
          >
            <CardContent>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 1 }}>
                <Typography variant="h6" component="div">
                  {special.content}
                </Typography>
                <Chip 
                  icon={getSpecialIcon(special.type)}
                  label={special.typeName}
                  color={getSpecialColor(special.type)}
                  size="small"
                  sx={{ ml: 1 }}
                />
              </Box>
              
              {showVenueName && special.venueName && (
                <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 1 }}>
                  {special.venueName}
                </Typography>
              )}

              <Divider sx={{ my: 1 }} />
              
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="body2" color="text.secondary">
                  {formatTimeRange(special)}
                </Typography>
                
                {special.isCurrentlyRunning && (
                  <Chip 
                    label="Active Now" 
                    color="success" 
                    size="small" 
                    variant="outlined"
                  />
                )}
              </Box>
              
              {special.isRecurring && (
                <Typography variant="body2" color="text.secondary" sx={{ mt: 1, fontSize: '0.75rem' }}>
                  Recurring special
                </Typography>
              )}
            </CardContent>
          </Card>
        </Grid>
      ))}
    </Grid>
  );
};

export default SpecialsList;
