import { Card, CardContent, Typography, Chip, Box, CardActions, Button, IconButton } from '@mui/material';
import { LocalBar as DrinkIcon, Restaurant as FoodIcon, MusicNote as EntertainmentIcon, Edit as EditIcon } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { DateTime } from 'luxon';
import { SpecialItem as SpecialItemType, SpecialTypes } from '@models/special';
import { memo } from 'react';

interface SpecialItemProps {
  special: SpecialItemType;
  allowEdit?: boolean;
  compact?: boolean;
}

const SpecialItem = ({ special, allowEdit = false, compact = false }: SpecialItemProps) => {
  const navigate = useNavigate();
  
  const getSpecialIcon = () => {
    switch (special.type) {
      case SpecialTypes.Drink:
        return <DrinkIcon />;
      case SpecialTypes.Food:
        return <FoodIcon />;
      case SpecialTypes.Entertainment:
        return <EntertainmentIcon />;
      default:
        return null;
    }
  };

  const getSpecialColor = () => {
    switch (special.type) {
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

  const formatTimeRange = (): string => {
    const start = formatTime(special.startTime);
    const end = special.endTime ? formatTime(special.endTime) : null;
    
    return end ? `${start} - ${end}` : `Starting ${start}`;
  };

  const handleEdit = (e: React.MouseEvent) => {
    e.stopPropagation();
    navigate(`/specials/${special.id}/edit`);
  };

  if (compact) {
    return (
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          p: 1,
          border: '1px solid',
          borderColor: 'divider',
          borderRadius: 1,
          mb: 1
        }}
      >
        <Box>
          <Typography variant="body1" fontWeight="medium">{special.content}</Typography>
          <Typography variant="caption" color="text.secondary">{formatTimeRange()}</Typography>
        </Box>
        
        <Box display="flex" alignItems="center" gap={1}>
          <Chip 
            icon={getSpecialIcon()} 
            label={special.typeName}
            color={getSpecialColor()}
            size="small"
          />
          {special.isCurrentlyRunning && (
            <Chip label="Active" color="success" size="small" variant="outlined" />
          )}
        </Box>
      </Box>
    );
  }

  return (
    <Card 
      sx={{ 
        mb: 2,
        transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out',
        '&:hover': {
          boxShadow: 3,
          transform: 'translateY(-2px)'
        }
      }}
    >
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
          <Typography variant="h6" component="div">{special.content}</Typography>
          <Chip 
            icon={getSpecialIcon()} 
            label={special.typeName}
            color={getSpecialColor()}
            size="small"
            sx={{ ml: 1 }}
          />
        </Box>

        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 1 }}>
          <Typography variant="body2">
            <strong>Schedule:</strong> {formatTimeRange()}
          </Typography>
          
          {special.isCurrentlyRunning ? (
            <Chip label="Active Now" color="success" size="small" />
          ) : (
            <Chip label="Not Active" color="default" size="small" variant="outlined" />
          )}
        </Box>

        <Box sx={{ mt: 1 }}>
          <Typography variant="body2" color="text.secondary">
            <strong>Date:</strong> {special.startDate}
            {special.isRecurring && " (Recurring)"}
          </Typography>
        </Box>
      </CardContent>

      {allowEdit && (
        <CardActions sx={{ justifyContent: 'flex-end', pt: 0 }}>
          <Button 
            size="small" 
            startIcon={<EditIcon />} 
            onClick={handleEdit}
          >
            Edit
          </Button>
        </CardActions>
      )}
    </Card>
  );
};

export default memo(SpecialItem);
