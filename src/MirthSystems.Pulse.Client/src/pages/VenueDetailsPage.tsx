import { useAuth0 } from '@auth0/auth0-react';
import {
  Add as AddIcon,
  Delete as DeleteIcon,
  Edit as EditIcon,
  Email as EmailIcon,
  Phone as PhoneIcon,
  Place as PlaceIcon,
  AccessTime as TimeIcon,
  Language as WebsiteIcon,
} from '@mui/icons-material';
import {
  Alert,
  Avatar,
  Box,
  Button,
  Card,
  CardContent,
  CircularProgress,
  Container,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Divider,
  FormControl,
  Grid,
  IconButton,
  Link,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  MenuItem,
  Paper,
  Select,
  TextField,
  Tooltip,
  Typography
} from '@mui/material';
import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import { deleteVenue, fetchVenueBusinessHours, fetchVenueById, fetchVenueSpecials } from '@/store/venueSlice';
import { deleteSpecial } from '@/store/specialSlice';
import { createSchedule, deleteSchedule, updateSchedule } from '@/store/scheduleSlice';
import VenueMap from '@components/venues/VenueMap';
import { useAppDispatch, useAppSelector } from '@hooks';
import { useApiClient } from '@services/apiClient';
import { OperatingScheduleItem } from '@models/operatingSchedule';
import { DateTime } from 'luxon';

// Define interfaces for the component props
interface DeleteSpecialDialogProps {
  open: boolean;
  specialId: string;
  specialName: string;
  onClose: () => void;
  onConfirm: (id: string) => void;
  isDeleting: boolean;
}

interface BusinessHourDialogProps {
  open: boolean;
  onClose: () => void;
  onSave: (data: any) => void;
  dayOfWeek: number;
  dayName: string;
  initialData: OperatingScheduleItem | null;
  isEditing: boolean;
  isLoading: boolean;
  venueId: string | undefined;
}

// Dialog component for confirming special deletion
const DeleteSpecialDialog = ({ 
  open, 
  specialId, 
  specialName, 
  onClose, 
  onConfirm, 
  isDeleting 
}: DeleteSpecialDialogProps) => {
  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Delete Special</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Are you sure you want to delete the special "{specialName}"? This action cannot be undone.
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} disabled={isDeleting}>Cancel</Button>
        <Button 
          onClick={() => onConfirm(specialId)} 
          color="error" 
          disabled={isDeleting}
          startIcon={isDeleting ? <CircularProgress size={20} /> : <DeleteIcon />}
        >
          Delete
        </Button>
      </DialogActions>
    </Dialog>
  );
};

// Dialog component for editing or adding business hours
const BusinessHourDialog = ({ 
  open, 
  onClose, 
  onSave, 
  dayOfWeek, 
  dayName, 
  initialData, 
  isEditing, 
  isLoading,
  venueId
}: BusinessHourDialogProps) => {
  const [timeOfOpen, setTimeOfOpen] = useState(initialData?.openTime || '09:00');
  const [timeOfClose, setTimeOfClose] = useState(initialData?.closeTime || '17:00');
  const [isClosed, setIsClosed] = useState(initialData?.isClosed || false);
  
  useEffect(() => {
    if (initialData) {
      setTimeOfOpen(initialData.openTime || '09:00');
      setTimeOfClose(initialData.closeTime || '17:00');
      setIsClosed(initialData.isClosed || false);
    } else {
      setTimeOfOpen('09:00');
      setTimeOfClose('17:00');
      setIsClosed(false);
    }
  }, [initialData, open]);

  const handleSave = () => {
    onSave({
      dayOfWeek,
      timeOfOpen,
      timeOfClose,
      isClosed,
      id: initialData?.id,
      venueId
    });
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>
        {isEditing ? `Edit Hours for ${dayName}` : `Add Hours for ${dayName}`}
      </DialogTitle>
      <DialogContent>
        <Box sx={{ pt: 2 }}>
          <FormControl fullWidth sx={{ mb: 2 }}>
            <Select
              value={isClosed ? "closed" : "open"}
              onChange={(e) => setIsClosed(e.target.value === "closed")}
            >
              <MenuItem value="open">Open</MenuItem>
              <MenuItem value="closed">Closed</MenuItem>
            </Select>
          </FormControl>
          
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={6}>
              <TextField
                label="Opening Time"
                type="time"
                fullWidth
                value={timeOfOpen}
                onChange={(e) => setTimeOfOpen(e.target.value)}
                InputLabelProps={{ shrink: true }}
                inputProps={{ step: 300 }}
                disabled={isClosed}
              />
            </Grid>
            <Grid item xs={6}>
              <TextField
                label="Closing Time"
                type="time"
                fullWidth
                value={timeOfClose}
                onChange={(e) => setTimeOfClose(e.target.value)}
                InputLabelProps={{ shrink: true }}
                inputProps={{ step: 300 }}
                disabled={isClosed}
              />
            </Grid>
          </Grid>
          
          <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mt: 2 }}>
            Note: For hours extending past midnight, use the closing time of the next day.
            For example, if you close at 2 AM, enter 02:00.
          </Typography>
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} disabled={isLoading}>
          Cancel
        </Button>
        <Button 
          onClick={handleSave} 
          variant="contained" 
          color="primary"
          disabled={isLoading}
          startIcon={isLoading ? <CircularProgress size={20} /> : null}
        >
          Save
        </Button>
      </DialogActions>
    </Dialog>
  );
};

const VenueDetailsPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const apiClient = useApiClient();
  const { isAuthenticated } = useAuth0();

  const { currentVenue, venueBusinessHours, venueSpecials, loading, error } = useAppSelector((state) => state.venues);
  const { loading: scheduleLoading } = useAppSelector((state) => state.schedules);

  // State for delete dialogs
  const [deleteVenueDialogOpen, setDeleteVenueDialogOpen] = useState(false);
  const [deleteSpecialDialogOpen, setDeleteSpecialDialogOpen] = useState(false);
  const [selectedSpecialId, setSelectedSpecialId] = useState<string | null>(null);
  const [selectedSpecialName, setSelectedSpecialName] = useState<string>('');
  const [isDeleting, setIsDeleting] = useState(false);
  
  // State for business hours management
  const [businessHourDialogOpen, setBusinessHourDialogOpen] = useState(false);
  const [selectedDayOfWeek, setSelectedDayOfWeek] = useState<number>(0);
  const [selectedDayName, setSelectedDayName] = useState<string>('');
  const [selectedSchedule, setSelectedSchedule] = useState<OperatingScheduleItem | null>(null);
  const [isEditingSchedule, setIsEditingSchedule] = useState(false);
  const [deleteScheduleDialogOpen, setDeleteScheduleDialogOpen] = useState(false);
  const [selectedScheduleId, setSelectedScheduleId] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      dispatch(fetchVenueById(id) as any);
      dispatch(fetchVenueBusinessHours(id) as any);
      dispatch(fetchVenueSpecials(id) as any);
    }
  }, [id, dispatch]);

  useEffect(() => {
    if (venueBusinessHours) {
      console.log('Business hours loaded:', venueBusinessHours);
    }
  }, [venueBusinessHours]);

  const handleEditVenue = () => {
    navigate(`/venues/${id}/edit`);
  };

  const handleDeleteVenue = async () => {
    if (!id) return;

    setIsDeleting(true);
    try {
      await dispatch(deleteVenue({ id, apiClient }) as any);
      navigate('/venues');
    } catch (error) {
      console.error('Failed to delete venue:', error);
    } finally {
      setIsDeleting(false);
      setDeleteVenueDialogOpen(false);
    }
  };

  const handleAddSpecial = () => {
    navigate(`/venues/${id}/specials/new`);
  };

  const handleEditSpecial = (specialId: string) => {
    navigate(`/venues/${id}/specials/${specialId}/edit`);
  };

  const openDeleteSpecialDialog = (specialId: string, content: string) => {
    setSelectedSpecialId(specialId);
    setSelectedSpecialName(content);
    setDeleteSpecialDialogOpen(true);
  };

  const handleDeleteSpecial = async (specialId: string) => {
    setIsDeleting(true);
    try {
      await dispatch(deleteSpecial({ id: specialId, apiClient }) as any);
      // Refresh specials list
      dispatch(fetchVenueSpecials(id as string) as any);
    } catch (error) {
      console.error('Failed to delete special:', error);
    } finally {
      setIsDeleting(false);
      setDeleteSpecialDialogOpen(false);
      setSelectedSpecialId(null);
    }
  };
  
  // Business hours management functions
  const openBusinessHourDialog = (dayOfWeek: number, dayName: string, schedule: OperatingScheduleItem | null) => {
    setSelectedDayOfWeek(dayOfWeek);
    setSelectedDayName(dayName);
    setSelectedSchedule(schedule);
    setIsEditingSchedule(schedule !== null);
    setBusinessHourDialogOpen(true);
  };
  
  const handleSaveBusinessHour = async (data: any) => {
    try {
      if (isEditingSchedule) {
        const { id: scheduleId, ...updateData } = data;
        await dispatch(updateSchedule({ 
          id: scheduleId, 
          scheduleData: updateData, 
          apiClient 
        }) as any);
      } else {
        await dispatch(createSchedule({ 
          scheduleData: data, 
          apiClient 
        }) as any);
      }
      
      // Refresh business hours
      dispatch(fetchVenueBusinessHours(id as string) as any);
      setBusinessHourDialogOpen(false);
    } catch (error) {
      console.error('Failed to save business hours:', error);
    }
  };
  
  const openDeleteScheduleDialog = (scheduleId: string) => {
    setSelectedScheduleId(scheduleId);
    setDeleteScheduleDialogOpen(true);
  };
  
  const handleDeleteSchedule = async () => {
    if (!selectedScheduleId) return;
    
    setIsDeleting(true);
    try {
      await dispatch(deleteSchedule({ 
        id: selectedScheduleId, 
        apiClient 
      }) as any);
      
      // Refresh business hours
      dispatch(fetchVenueBusinessHours(id as string) as any);
    } catch (error) {
      console.error('Failed to delete schedule:', error);
    } finally {
      setIsDeleting(false);
      setDeleteScheduleDialogOpen(false);
      setSelectedScheduleId(null);
    }
  };
  
  // Helper function to get day name from day of week
  const getDayName = (day: number): string => {
    const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    return days[day];
  };
  
  // Helper function to format time string
  const formatTimeString = (timeString: string): string => {
    try {
      const [hours, minutes] = timeString.split(':').map(Number);
      const dateTime = DateTime.fromObject({ hour: hours, minute: minutes });
      return dateTime.toLocaleString(DateTime.TIME_SIMPLE);
    } catch (error) {
      return timeString;
    }
  };
  
  // Check which days of the week are missing in the schedule
  const getMissingDays = (): number[] => {
    const existingDays = venueBusinessHours.map(schedule => schedule.dayOfWeek);
    return [0, 1, 2, 3, 4, 5, 6].filter(day => !existingDays.includes(day));
  };

  if (loading && !currentVenue) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Container>
        <Alert severity="error" sx={{ mt: 2 }}>
          {error}
        </Alert>
        <Button
          variant="outlined"
          onClick={() => navigate('/venues')}
          sx={{ mt: 2 }}
        >
          Back to Venues
        </Button>
      </Container>
    );
  }

  if (!currentVenue) {
    return (
      <Container>
        <Alert severity="warning" sx={{ mt: 2 }}>
          Venue not found
        </Alert>
        <Button
          variant="outlined"
          onClick={() => navigate('/venues')}
          sx={{ mt: 2 }}
        >
          Back to Venues
        </Button>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg">
      {/* Header section with name and action buttons */}
      <Box sx={{ 
        display: 'flex', 
        justifyContent: 'space-between',
        alignItems: 'flex-start', 
        mb: 3 
      }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
          <Avatar
            src={currentVenue.profileImage || '/img/placeholder-venue.jpg'}
            alt={currentVenue.name}
            sx={{ width: 64, height: 64 }}
            variant="rounded"
          />
          <Box>
            <Typography variant="h4" component="h1">
              {currentVenue.name}
            </Typography>
            <Typography variant="subtitle1" color="text.secondary">
              {currentVenue.locality}, {currentVenue.region}
            </Typography>
          </Box>
        </Box>

        {isAuthenticated && (
          <Box>
            <Button
              variant="outlined"
              startIcon={<EditIcon />}
              onClick={handleEditVenue}
              sx={{ mr: 1 }}
            >
              Edit Venue
            </Button>
            <Button
              variant="outlined"
              color="error"
              startIcon={<DeleteIcon />}
              onClick={() => setDeleteVenueDialogOpen(true)}
            >
              Delete Venue
            </Button>
          </Box>
        )}
      </Box>

      {/* Description */}
      {currentVenue.description && (
        <Typography variant="body1" sx={{ mb: 4 }}>
          {currentVenue.description}
        </Typography>
      )}

      {/* Main content grid */}
      <Grid container spacing={4}>
        {/* Left column - Contact info at top, then address and map */}
        <Grid item xs={12} md={6}>
          {/* Contact information block */}
          <Paper elevation={1} sx={{ p: 3, mb: 4 }}>
            <Typography variant="h6" gutterBottom>
              Contact Information
            </Typography>
            <List dense>
              {currentVenue.phoneNumber && (
                <ListItem>
                  <ListItemIcon>
                    <PhoneIcon />
                  </ListItemIcon>
                  <ListItemText>
                    <Link href={`tel:${currentVenue.phoneNumber}`} color="inherit">
                      {currentVenue.phoneNumber}
                    </Link>
                  </ListItemText>
                </ListItem>
              )}
              
              {currentVenue.email && (
                <ListItem>
                  <ListItemIcon>
                    <EmailIcon />
                  </ListItemIcon>
                  <ListItemText>
                    <Link href={`mailto:${currentVenue.email}`} color="inherit">
                      {currentVenue.email}
                    </Link>
                  </ListItemText>
                </ListItem>
              )}
              
              {currentVenue.website && (
                <ListItem>
                  <ListItemIcon>
                    <WebsiteIcon />
                  </ListItemIcon>
                  <ListItemText>
                    <Link href={currentVenue.website} target="_blank" rel="noopener noreferrer">
                      {currentVenue.website}
                    </Link>
                  </ListItemText>
                </ListItem>
              )}
            </List>
          </Paper>

          {/* Address and map block */}
          <Paper elevation={1} sx={{ p: 3, mb: 4 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
              <Typography variant="h6">
                Address
              </Typography>
              <Button
                variant="outlined"
                startIcon={<PlaceIcon />}
                component="a"
                href={`https://maps.google.com/?q=${encodeURIComponent(
                  `${currentVenue.streetAddress}, ${currentVenue.locality}, ${currentVenue.region}, ${currentVenue.postcode}`
                )}`}
                target="_blank"
                rel="noopener noreferrer"
                size="small"
              >
                Get Directions
              </Button>
            </Box>
            
            <Box sx={{ display: 'flex', alignItems: 'flex-start', mb: 2 }}>
              <PlaceIcon sx={{ mr: 1, mt: 0.5 }} color="primary" />
              <Typography variant="body1">
                {currentVenue.streetAddress}
                {currentVenue.secondaryAddress && <span>, {currentVenue.secondaryAddress}</span>}<br />
                {currentVenue.locality}, {currentVenue.region} {currentVenue.postcode}<br />
                {currentVenue.country}
              </Typography>
            </Box>
            
            {/* Map component below address */}
            {currentVenue.latitude && currentVenue.longitude && (
              <Box sx={{ mt: 2, height: 250 }}>
                <VenueMap
                  latitude={currentVenue.latitude}
                  longitude={currentVenue.longitude}
                  venueName={currentVenue.name}
                />
              </Box>
            )}
          </Paper>
        </Grid>

        {/* Right column - Business hours with edit functionality */}
        <Grid item xs={12} md={6}>
          <Paper elevation={1} sx={{ p: 3, mb: 4 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <TimeIcon sx={{ mr: 1 }} color="primary" />
                <Typography variant="h6">Business Hours</Typography>
              </Box>
            </Box>
            
            {/* Enhanced business hours display with edit/delete buttons */}
            <List>
              {loading ? (
                <Box display="flex" justifyContent="center" my={2}>
                  <CircularProgress size={24} />
                </Box>
              ) : venueBusinessHours && venueBusinessHours.length > 0 ? (
                // Display existing business hours with edit/delete buttons
                [0, 1, 2, 3, 4, 5, 6].map(dayNum => {
                  const schedule = venueBusinessHours.find(s => s.dayOfWeek === dayNum);
                  const dayName = getDayName(dayNum);
                  const currentDay = DateTime.now().weekday % 7 === dayNum;
                  
                  return (
                    <ListItem 
                      key={dayNum}
                      sx={{ 
                        borderBottom: '1px solid',
                        borderColor: 'divider',
                        backgroundColor: currentDay ? 'rgba(0, 0, 0, 0.04)' : 'transparent',
                        py: 1
                      }}
                      secondaryAction={
                        isAuthenticated && (
                          <Box>
                            {schedule ? (
                              <>
                                <IconButton 
                                  edge="end" 
                                  aria-label="edit"
                                  onClick={() => openBusinessHourDialog(dayNum, dayName, schedule)}
                                  size="small"
                                >
                                  <EditIcon fontSize="small" />
                                </IconButton>
                                <IconButton 
                                  edge="end" 
                                  aria-label="delete"
                                  onClick={() => openDeleteScheduleDialog(schedule.id)}
                                  size="small"
                                >
                                  <DeleteIcon fontSize="small" />
                                </IconButton>
                              </>
                            ) : (
                              <Button
                                size="small"
                                startIcon={<AddIcon />}
                                onClick={() => openBusinessHourDialog(dayNum, dayName, null)}
                              >
                                Add
                              </Button>
                            )}
                          </Box>
                        )
                      }
                    >
                      <ListItemText
                        primary={<Typography fontWeight={currentDay ? 'bold' : 'regular'}>{dayName}</Typography>}
                        secondary={
                          schedule ? (
                            schedule.isClosed ? (
                              <Typography variant="body2" color="error">Closed</Typography>
                            ) : (
                              <Typography variant="body2">
                                {formatTimeString(schedule.openTime)} - {formatTimeString(schedule.closeTime)}
                              </Typography>
                            )
                          ) : (
                            <Typography variant="body2" color="text.secondary">Not set</Typography>
                          )
                        }
                      />
                    </ListItem>
                  );
                })
              ) : (
                <Box sx={{ py: 2, textAlign: 'center' }}>
                  <Typography color="text.secondary" gutterBottom>
                    No business hours available
                  </Typography>
                  {isAuthenticated && (
                    <Button
                      variant="outlined"
                      startIcon={<AddIcon />}
                      size="small"
                      onClick={() => openBusinessHourDialog(1, getDayName(1), null)}
                      sx={{ mt: 1 }}
                    >
                      Add Business Hours
                    </Button>
                  )}
                </Box>
              )}
            </List>
          </Paper>
        </Grid>
      </Grid>

      {/* Specials section - Full width at bottom */}
      <Paper elevation={1} sx={{ p: 3, mt: 2 }}>
        <Box sx={{ 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center', 
          mb: 3 
        }}>
          <Typography variant="h5" component="h2">
            Specials
          </Typography>

          {isAuthenticated && (
            <Button
              variant="contained"
              color="primary"
              startIcon={<AddIcon />}
              onClick={handleAddSpecial}
            >
              Add New Special
            </Button>
          )}
        </Box>

        {venueSpecials && venueSpecials.length > 0 ? (
          <Grid container spacing={2}>
            {venueSpecials.map((special) => (
              <Grid item xs={12} md={6} key={special.id}>
                <Card variant="outlined" sx={{ height: '100%' }}>
                  <CardContent>
                    <Typography variant="h6" gutterBottom>{special.content}</Typography>
                    <Typography variant="body2" color="text.secondary">
                      Type: {special.typeName}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Time: {special.startTime} {special.endTime ? `- ${special.endTime}` : ''}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {special.isRecurring ? 'Recurring special' : 'One-time special'}
                    </Typography>
                    {isAuthenticated && (
                      <Box sx={{ mt: 2, display: 'flex', justifyContent: 'flex-end', gap: 1 }}>
                        <Button 
                          size="small" 
                          variant="outlined"
                          startIcon={<EditIcon />}
                          onClick={() => handleEditSpecial(special.id as string)}
                        >
                          Edit
                        </Button>
                        <Button 
                          size="small" 
                          variant="outlined" 
                          color="error"
                          startIcon={<DeleteIcon />}
                          onClick={() => openDeleteSpecialDialog(special.id as string, special.content)}
                        >
                          Delete
                        </Button>
                      </Box>
                    )}
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        ) : (
          <Box textAlign="center" py={4}>
            <Typography color="text.secondary" sx={{ mb: 2 }}>
              No specials available for this venue yet.
            </Typography>
            {isAuthenticated && (
              <Button
                variant="outlined"
                startIcon={<AddIcon />}
                onClick={handleAddSpecial}
              >
                Create First Special
              </Button>
            )}
          </Box>
        )}
      </Paper>

      {/* Delete venue confirmation dialog */}
      <Dialog
        open={deleteVenueDialogOpen}
        onClose={() => setDeleteVenueDialogOpen(false)}
      >
        <DialogTitle>Delete Venue</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete "{currentVenue.name}"? This action cannot be undone,
            and all associated specials and business hours will also be deleted.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() => setDeleteVenueDialogOpen(false)}
            disabled={isDeleting}
          >
            Cancel
          </Button>
          <Button
            onClick={handleDeleteVenue}
            color="error"
            disabled={isDeleting}
            startIcon={isDeleting ? <CircularProgress size={20} /> : <DeleteIcon />}
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>

      {/* Delete special confirmation dialog */}
      <DeleteSpecialDialog 
        open={deleteSpecialDialogOpen}
        specialId={selectedSpecialId || ''}
        specialName={selectedSpecialName}
        onClose={() => setDeleteSpecialDialogOpen(false)}
        onConfirm={handleDeleteSpecial}
        isDeleting={isDeleting}
      />

      {/* Business hour dialog for adding/editing */}
      <BusinessHourDialog
        open={businessHourDialogOpen}
        onClose={() => setBusinessHourDialogOpen(false)}
        onSave={handleSaveBusinessHour}
        dayOfWeek={selectedDayOfWeek}
        dayName={selectedDayName}
        initialData={selectedSchedule}
        isEditing={isEditingSchedule}
        isLoading={scheduleLoading}
        venueId={id}
      />
      
      {/* Delete schedule confirmation dialog */}
      <Dialog
        open={deleteScheduleDialogOpen}
        onClose={() => setDeleteScheduleDialogOpen(false)}
      >
        <DialogTitle>Delete Business Hours</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete these business hours? This action cannot be undone.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() => setDeleteScheduleDialogOpen(false)}
            disabled={isDeleting}
          >
            Cancel
          </Button>
          <Button
            onClick={handleDeleteSchedule}
            color="error"
            disabled={isDeleting}
            startIcon={isDeleting ? <CircularProgress size={20} /> : <DeleteIcon />}
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default VenueDetailsPage;