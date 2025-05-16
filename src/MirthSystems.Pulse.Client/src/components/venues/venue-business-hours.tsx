import { Box, Typography, Grid } from '@mui/material';
import { useEffect, useState } from 'react';
import { useOperatingSchedulesStore } from '../../store';

const daysOfWeek = [
  'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'
];

export const VenueBusinessHours = ({ venueId }: { venueId: string }) => {
  const { venueSchedules, fetchVenueBusinessHours } = useOperatingSchedulesStore();
  const [localSchedules, setLocalSchedules] = useState<OperatingScheduleItem[]>([]);

  useEffect(() => {
    if (venueId) {
      fetchVenueBusinessHours(venueId);
    }
  }, [venueId, fetchVenueBusinessHours]);

  useEffect(() => {
    setLocalSchedules(venueSchedules);
  }, [venueSchedules]);

  return (
    <Box>
      <Typography variant="h6">Business Hours</Typography>
      <Grid container spacing={1}>
        {daysOfWeek.map((day, idx) => {
          const schedule = localSchedules.find(s => s.dayOfWeek === idx);
          return (
            <Grid item xs={12} key={day}>
              <Box display="flex" alignItems="center" gap={1}>
                <Typography sx={{ minWidth: 80 }}>{day}</Typography>
                {schedule ? (
                  schedule.isClosed ? (
                    <Typography color="text.secondary">Closed</Typography>
                  ) : (
                    <Typography>
                      {typeof schedule.openTime === 'string' ? schedule.openTime : schedule.openTime.toFormat('HH:mm')} - {typeof schedule.closeTime === 'string' ? schedule.closeTime : schedule.closeTime.toFormat('HH:mm')}
                    </Typography>
                  )
                ) : (
                  <Typography color="text.secondary">No hours set</Typography>
                )}
              </Box>
            </Grid>
          );
        })}
      </Grid>
    </Box>
  );
};
