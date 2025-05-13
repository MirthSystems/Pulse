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
  Box,
  Button,
  CardMedia,
  Chip,
  CircularProgress,
  Container,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Grid,
  Link,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Paper,
  Tab,
  Tabs,
  Typography
} from '@mui/material';
import { DateTime } from 'luxon';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom';

import { deleteVenue, fetchVenueBusinessHours, fetchVenueById, fetchVenueSpecials } from '@/store/venueSlice';
import SpecialsList from '@components/specials/SpecialsList';
import BusinessHoursDisplay from '@components/venues/BusinessHoursDisplay';
import VenueMap from '@components/venues/VenueMap';
import { clearSpecialsError } from '@store/specialSlice';
import { useApiClient } from '@services/apiClient';
import { RootState } from '@store/index';

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`venue-tabpanel-${index}`}
      aria-labelledby={`venue-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ p: 3 }}>
          {children}
        </Box>
      )}
    </div>
  );
}

const VenueDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const apiClient = useApiClient();
  const { isAuthenticated } = useAuth0();

  const { currentVenue, venueBusinessHours, venueSpecials, loading, error } = useSelector((state: RootState) => state.venues);

  const [tabValue, setTabValue] = useState(0);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [deleteInProgress, setDeleteInProgress] = useState(false);

  useEffect(() => {
    if (id) {
      dispatch(fetchVenueById(id) as any);
      dispatch(fetchVenueBusinessHours(id) as any);
      dispatch(fetchVenueSpecials(id) as any);
    }

    return () => {
      // Cleanup
      dispatch(clearSpecialsError());
    };
  }, [id, dispatch]);

  const handleTabChange = (_: React.SyntheticEvent, newValue: number) => {
    setTabValue(newValue);
  };

  const handleEditVenue = () => {
    navigate(`/venues/${id}/edit`);
  };

  const handleDeleteVenue = async () => {
    if (!id) return;

    setDeleteInProgress(true);
    try {
      await dispatch(deleteVenue({ id, apiClient }) as any);
      setDeleteDialogOpen(false);
      navigate('/venues');
    } catch (error) {
      console.error('Failed to delete venue:', error);
    } finally {
      setDeleteInProgress(false);
    }
  };

  const handleAddSpecial = () => {
    navigate(`/specials/new?venueId=${id}`);
  };

  const handleBack = () => {
    navigate('/backoffice');
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
          onClick={handleBack}
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
          onClick={handleBack}
          sx={{ mt: 2 }}
        >
          Back to Venues
        </Button>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg">
      <Box sx={{ position: 'relative', mb: 4 }}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <Typography variant="h4" component="h1" gutterBottom>
              {currentVenue.name}
            </Typography>

            <Box sx={{ mb: 2 }}>
              <Typography variant="body1" color="text.secondary" sx={{ display: 'flex', alignItems: 'center' }}>
                <PlaceIcon fontSize="small" sx={{ mr: 0.5 }} />
                {currentVenue.locality}, {currentVenue.region}
              </Typography>
            </Box>

            {currentVenue.description && (
              <Typography variant="body1" paragraph>
                {currentVenue.description}
              </Typography>
            )}

            <List dense sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper', mb: 2 }}>
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

              {currentVenue.website && (
                <ListItem>
                  <ListItemIcon>
                    <WebsiteIcon />
                  </ListItemIcon>
                  <ListItemText>
                    <Link href={currentVenue.website} target="_blank" rel="noopener noreferrer">
                      {new URL(currentVenue.website).hostname}
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

              <ListItem>
                <ListItemIcon>
                  <TimeIcon />
                </ListItemIcon>
                <ListItemText>
                  {isVenueOpenNow(venueBusinessHours) ? (
                    <Chip label="Open Now" color="success" size="small" />
                  ) : (
                    <Chip label="Closed Now" color="error" size="small" />
                  )}
                </ListItemText>
              </ListItem>
            </List>

            {isAuthenticated && (
              <Box sx={{ mt: 2 }}>
                <Button
                  variant="outlined"
                  startIcon={<EditIcon />}
                  onClick={handleEditVenue}
                  sx={{ mr: 2 }}
                >
                  Edit Venue
                </Button>
                <Button
                  variant="outlined"
                  color="error"
                  startIcon={<DeleteIcon />}
                  onClick={() => setDeleteDialogOpen(true)}
                >
                  Delete Venue
                </Button>
              </Box>
            )}
          </Grid>

          <Grid item xs={12} md={6}>
            {currentVenue.profileImage ? (
              <CardMedia
                component="img"
                height="300"
                image={currentVenue.profileImage}
                alt={currentVenue.name}
                sx={{ objectFit: 'cover', borderRadius: 2 }}
              />
            ) : (
              <Paper
                sx={{
                  height: 300,
                  bgcolor: 'grey.200',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  borderRadius: 2
                }}
              >
                <Typography variant="body1" color="text.secondary">
                  No image available
                </Typography>
              </Paper>
            )}

            {currentVenue.latitude && currentVenue.longitude && (
              <Box sx={{ mt: 2, height: 200 }}>
                <VenueMap
                  latitude={currentVenue.latitude}
                  longitude={currentVenue.longitude}
                  venueName={currentVenue.name}
                />
              </Box>
            )}
          </Grid>
        </Grid>
      </Box>

      <Box sx={{ width: '100%', mb: 4 }}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
          <Tabs
            value={tabValue}
            onChange={handleTabChange}
            aria-label="venue information tabs"
            sx={{ mb: 1 }}
          >
            <Tab label="Address & Hours" />
            <Tab label="Specials" />
          </Tabs>
        </Box>

        <TabPanel value={tabValue} index={0}>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <Typography variant="h6" gutterBottom>Full Address</Typography>
              <Typography>
                {currentVenue.streetAddress}
                {currentVenue.secondaryAddress && <span>, {currentVenue.secondaryAddress}</span>}
                <br />
                {currentVenue.locality}, {currentVenue.region} {currentVenue.postcode}
                <br />
                {currentVenue.country}
              </Typography>

              <Box sx={{ mt: 2 }}>
                <Button
                  variant="outlined"
                  startIcon={<PlaceIcon />}
                  component="a"
                  href={`https://maps.google.com/?q=${encodeURIComponent(
                    `${currentVenue.streetAddress}, ${currentVenue.locality}, ${currentVenue.region}, ${currentVenue.postcode}`
                  )}`}
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  Get Directions
                </Button>
              </Box>
            </Grid>

            <Grid item xs={12} md={6}>
              <Typography variant="h6" gutterBottom>Business Hours</Typography>
              <BusinessHoursDisplay schedules={venueBusinessHours} />
            </Grid>
          </Grid>
        </TabPanel>

        <TabPanel value={tabValue} index={1}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">
              {venueSpecials.length > 0 ? 'Current Specials' : 'No Current Specials'}
            </Typography>

            {isAuthenticated && (
              <Button
                variant="contained"
                color="primary"
                startIcon={<AddIcon />}
                onClick={handleAddSpecial}
              >
                Add Special
              </Button>
            )}
          </Box>

          {venueSpecials.length > 0 ? (
            <SpecialsList specials={venueSpecials} />
          ) : (
            <Box sx={{ textAlign: 'center', py: 4 }}>
              <Typography color="text.secondary">
                This venue doesn't have any active specials right now.
              </Typography>

              {isAuthenticated && (
                <Button
                  variant="outlined"
                  startIcon={<AddIcon />}
                  onClick={handleAddSpecial}
                  sx={{ mt: 2 }}
                >
                  Create First Special
                </Button>
              )}
            </Box>
          )}
        </TabPanel>
      </Box>

      <Dialog
        open={deleteDialogOpen}
        onClose={() => setDeleteDialogOpen(false)}
      >
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete the venue "{currentVenue.name}"?
            This action cannot be undone, and all associated specials will also be deleted.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)} disabled={deleteInProgress}>
            Cancel
          </Button>
          <Button
            onClick={handleDeleteVenue}
            color="error"
            disabled={deleteInProgress}
            startIcon={deleteInProgress ? <CircularProgress size={20} /> : null}
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

// Helper function to check if venue is open now
function isVenueOpenNow(businessHours: any[]): boolean {
  if (!businessHours || businessHours.length === 0) return false;

  const now = DateTime.local();
  const dayOfWeek = now.weekday % 7; // Convert to 0-based (0=Sunday)
  const currentTime = now.toFormat("HH:mm");

  const todayHours = businessHours.find(schedule =>
    schedule.dayOfWeek === dayOfWeek
  );

  if (!todayHours || todayHours.isClosed) return false;

  // Handle case where venue is open past midnight
  if (todayHours.openTime > todayHours.closeTime) {
    return currentTime >= todayHours.openTime || currentTime <= todayHours.closeTime;
  }

  return currentTime >= todayHours.openTime && currentTime <= todayHours.closeTime;
}

export default VenueDetailPage;
