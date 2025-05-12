import { Box, Typography, List, ListItem, ListItemText, Divider } from '@mui/material';
import { OperatingScheduleItem } from '@models/operatingSchedule';
import { DateTime } from 'luxon';

interface BusinessHoursDisplayProps {
  schedules: OperatingScheduleItem[];
}

const BusinessHoursDisplay = ({ schedules }: BusinessHoursDisplayProps) => {
  const currentDayNum = DateTime.local().weekday % 7; // Convert to 0-based (0=Sunday)
  
  // Sort the schedules by day of week, starting from Sunday (0)
  const sortedSchedules = [...schedules].sort((a, b) => a.dayOfWeek - b.dayOfWeek);
  
  return (
    <List dense sx={{ width: '100%', bgcolor: 'background.paper' }}>
      {sortedSchedules.map((schedule) => {
        const isToday = schedule.dayOfWeek === currentDayNum;
        
        return (
          <Box key={schedule.id}>
            <ListItem sx={{ 
              py: 1.5, 
              backgroundColor: isToday ? 'rgba(0, 0, 0, 0.04)' : 'transparent',
              borderRadius: 1
            }}>
              <ListItemText
                primary={
                  <Typography 
                    variant="body1" 
                    fontWeight={isToday ? 'bold' : 'regular'}
                  >
                    {schedule.dayName} {isToday && '(Today)'}
                  </Typography>
                }
                secondary={
                  schedule.isClosed ? (
                    <Typography variant="body2" color="error">
                      Closed
                    </Typography>
                  ) : (
                    <Typography variant="body2">
                      {formatTimeString(schedule.openTime)} - {formatTimeString(schedule.closeTime)}
                    </Typography>
                  )
                }
              />
            </ListItem>
            <Divider variant="middle" component="li" />
          </Box>
        );
      })}
    </List>
  );
};

// Helper to format time string from 24h to 12h format
const formatTimeString = (timeString: string): string => {
  try {
    const [hours, minutes] = timeString.split(':').map(Number);
    const date = DateTime.fromObject({ hour: hours, minute: minutes });
    return date.toFormat('h:mm a'); // e.g. "7:30 PM"
  } catch (error) {
    console.error("Error formatting time:", error);
    return timeString;
  }
};

export default BusinessHoursDisplay;
