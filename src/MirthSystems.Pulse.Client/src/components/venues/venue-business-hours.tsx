import { Box, Button, FormControlLabel, Switch, Typography } from '@mui/material';
import Grid from '@mui/material/Grid';
import { LocalizationProvider, TimePicker } from '@mui/x-date-pickers';
import { AdapterLuxon } from '@mui/x-date-pickers/AdapterLuxon';
import { DateTime } from 'luxon';
import { useEffect, useState } from 'react';
import { OperatingScheduleItem, type CreateOperatingScheduleRequest, type DayOfWeek, type UpdateOperatingScheduleRequest } from '../../models'; // Changed DayOfWeek to type-only import
import { useOperatingSchedulesStore } from '../../store';

const daysOfWeek = [
  'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'
];

export const VenueBusinessHours = ({ venueId, isEditing = false, listView = false }: { venueId: string, isEditing?: boolean, listView?: boolean }) => {
  const { venueSchedules, fetchVenueBusinessHours, updateSchedule, createSchedule, setError, error, isLoading: storeIsLoading } = useOperatingSchedulesStore();
  const [localSchedules, setLocalSchedules] = useState<OperatingScheduleItem[]>([]);
  const [changedItemIds, setChangedItemIds] = useState<Set<string>>(new Set());
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    if (venueId) {
      fetchVenueBusinessHours(venueId);
    }
  }, [venueId, fetchVenueBusinessHours]);

  useEffect(() => {
    const initializedSchedules = venueSchedules.map(s =>
      s instanceof OperatingScheduleItem ? s : new OperatingScheduleItem(s)
    );
    setLocalSchedules(initializedSchedules);
    setChangedItemIds(new Set());
  }, [venueSchedules]);

  const handleTimeChange = (dayOfWeekIndex: number, scheduleId: string, type: 'open' | 'close', value: DateTime | null) => {
    setLocalSchedules(currentSchedules =>
      currentSchedules.map(s => {
        if (s.dayOfWeek === dayOfWeekIndex && s.id === scheduleId) {
          setChangedItemIds(ids => new Set(ids).add(s.id));
          let newTime = value;
          if (newTime === null && !s.isClosed) {
            newTime = type === 'open'
              ? DateTime.fromObject({ hour: 9, minute: 0 })
              : DateTime.fromObject({ hour: 17, minute: 0 });
          }
          return new OperatingScheduleItem({
            ...s,
            id: s.id,
            dayName: s.dayName,
            dayOfWeek: s.dayOfWeek,
            openTime: type === 'open' ? (newTime?.toFormat('HH:mm') ?? '00:00') : (s.openTime?.toFormat('HH:mm') ?? '00:00'),
            closeTime: type === 'close' ? (newTime?.toFormat('HH:mm') ?? '00:00') : (s.closeTime?.toFormat('HH:mm') ?? '00:00'),
            isClosed: s.isClosed,
          });
        }
        return s;
      })
    );
  };

  const handleClosedChange = (dayOfWeekIndex: number, scheduleId: string, isNowClosed: boolean) => {
    setLocalSchedules(currentSchedules =>
      currentSchedules.map(s => {
        if (s.dayOfWeek === dayOfWeekIndex && s.id === scheduleId) {
          setChangedItemIds(ids => new Set(ids).add(s.id));
          let newOpenTime = s.openTime;
          let newCloseTime = s.closeTime;

          if (!isNowClosed) {
            if (!newOpenTime || !newOpenTime.isValid) {
              newOpenTime = DateTime.fromObject({ hour: 9, minute: 0 });
            }
            if (!newCloseTime || !newCloseTime.isValid) {
              newCloseTime = DateTime.fromObject({ hour: 17, minute: 0 });
            }
          }
          return new OperatingScheduleItem({
            ...s,
            id: s.id,
            dayName: s.dayName,
            dayOfWeek: s.dayOfWeek,
            openTime: newOpenTime?.toFormat('HH:mm') ?? '00:00',
            closeTime: newCloseTime?.toFormat('HH:mm') ?? '00:00',
            isClosed: isNowClosed,
          });
        }
        return s;
      })
    );
  };

  const handleAddHours = (dayOfWeekIndex: number) => {
    if (localSchedules.some(s => s.dayOfWeek === dayOfWeekIndex)) return;

    const tempId = `temp-${dayOfWeekIndex}-${Date.now()}`;
    const newSchedule = new OperatingScheduleItem({
      id: tempId,
      dayOfWeek: dayOfWeekIndex as DayOfWeek,
      dayName: daysOfWeek[dayOfWeekIndex],
      openTime: '09:00',
      closeTime: '17:00',
      isClosed: false,
    });

    setLocalSchedules(currentSchedules => [...currentSchedules, newSchedule].sort((a, b) => a.dayOfWeek - b.dayOfWeek));
    setChangedItemIds(ids => new Set(ids).add(tempId));
  };

  const handleSave = async () => {
    setIsSaving(true);
    setError(null);
    try {
      // Specify more precise Promise types
      const createPromises: Promise<string | null>[] = [];
      const updatePromises: Promise<boolean>[] = [];

      for (const s of localSchedules) {
        if (changedItemIds.has(s.id)) {
          let timeOfOpenStr = '00:00';
          let timeOfCloseStr = '00:00';

          if (s.openTime && s.openTime.isValid) {
            timeOfOpenStr = s.openTime.toFormat('HH:mm');
          } else if (!s.isClosed) {
            timeOfOpenStr = '09:00';
          }

          if (s.closeTime && s.closeTime.isValid) {
            timeOfCloseStr = s.closeTime.toFormat('HH:mm');
          } else if (!s.isClosed) {
            timeOfCloseStr = '17:00';
          }

          if (s.id.startsWith('temp-')) {
            const createRequest: CreateOperatingScheduleRequest = {
              venueId: venueId,
              dayOfWeek: s.dayOfWeek,
              timeOfOpen: timeOfOpenStr,
              timeOfClose: timeOfCloseStr,
              isClosed: s.isClosed,
            };
            createPromises.push(createSchedule(createRequest));
          } else {
            const updateRequest: UpdateOperatingScheduleRequest = {
              timeOfOpen: timeOfOpenStr,
              timeOfClose: timeOfCloseStr,
              isClosed: s.isClosed,
            };
            updatePromises.push(updateSchedule(s.id, updateRequest));
          }
        }
      }

      await Promise.all([...createPromises, ...updatePromises]);
      setChangedItemIds(new Set());
      await fetchVenueBusinessHours(venueId);
    } catch (err) {
      console.error("Error saving business hours:", err);
      setError(err instanceof Error ? err.message : 'Failed to save business hours');
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <Box>
      <Typography variant="h6">Business Hours</Typography>
      {error && <Typography color="error" gutterBottom>{error}</Typography>}
      <LocalizationProvider dateAdapter={AdapterLuxon}>
        <Grid container spacing={1} direction={listView ? 'column' : 'row'}>
          {daysOfWeek.map((dayName, idx) => {
            const schedule = localSchedules.find(s => s.dayOfWeek === idx);
            return (
              // Removed `item` prop, relying on `size` for item behavior within container
              <Grid key={idx} size={{ xs: 12 }} sx={listView ? { pb: 1 } : { mb: 1 }}>
                <Box display="flex" alignItems="center" gap={1} flexWrap="wrap">
                  <Typography sx={{ minWidth: 80, fontWeight: 'bold' }}>{dayName}</Typography>
                  {schedule ? (
                    <>
                      <FormControlLabel
                        control={
                          <Switch
                            checked={schedule.isClosed}
                            onChange={e => isEditing && handleClosedChange(idx, schedule.id, e.target.checked)}
                            color="primary"
                            disabled={!isEditing || isSaving || storeIsLoading}
                            size="small"
                          />
                        }
                        label="Closed"
                        sx={{ mr: 1 }}
                      />
                      {!schedule.isClosed && (
                        <>
                          <TimePicker
                            label="Open"
                            value={schedule.openTime}
                            onChange={val => isEditing && handleTimeChange(idx, schedule.id, 'open', val)}
                            ampm={false}
                            slotProps={{ textField: { size: 'small', sx: { minWidth: 90 }, disabled: !isEditing || isSaving || storeIsLoading } }}
                          />
                          <TimePicker
                            label="Close"
                            value={schedule.closeTime}
                            onChange={val => isEditing && handleTimeChange(idx, schedule.id, 'close', val)}
                            ampm={false}
                            slotProps={{ textField: { size: 'small', sx: { minWidth: 90 }, disabled: !isEditing || isSaving || storeIsLoading } }}
                          />
                        </>
                      )}
                    </>
                  ) : (
                    isEditing ? (
                      <Button
                        onClick={() => handleAddHours(idx)}
                        size="small"
                        variant="outlined"
                        disabled={isSaving || storeIsLoading}
                      >
                        Add Hours
                      </Button>
                    ) : (
                      <Typography color="text.secondary" sx={{ fontStyle: 'italic' }}>No hours set</Typography>
                    )
                  )}
                </Box>
              </Grid>
            );
          })}
        </Grid>
        {isEditing && (
          <Box mt={2} display="flex" justifyContent="flex-end">
            <Button
              variant="contained"
              color="primary"
              onClick={handleSave}
              disabled={isSaving || storeIsLoading || changedItemIds.size === 0}
            >
              {isSaving ? 'Saving...' : 'Save Changes'}
            </Button>
          </Box>
        )}
      </LocalizationProvider>
    </Box>
  );
};
