import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { 
  Box, 
  Typography, 
  Paper, 
  Button, 
  Grid, 
  Card, 
  CardContent,
  CardMedia,
  Tabs,
  Tab,
  Divider,
  CircularProgress,
  Alert,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle
} from '@mui/material';
import { 
  Edit as EditIcon, 
  Delete as DeleteIcon, 
  Add as AddIcon, 
  Phone as PhoneIcon,
  Email as EmailIcon,
  Language as WebsiteIcon,
  Place as PlaceIcon
} from '@mui/icons-material';
import { useApiClient } from '@services/apiClient';
import { RootState } from '@store/index';
import { fetchVenueById, fetchVenueBusinessHours, fetchVenueSpecials, deleteVenue } from '@features/venues/venueSlice';
import BusinessHoursDisplay from '@components/venues/BusinessHoursDisplay';
import SpecialsList from '@components/specials/SpecialsList';

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

const VenueManagementPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const apiClient = useApiClient();
  
  const { currentVenue, venueBusinessHours, venueSpecials, loading, error } = useSelector((state: RootState) => state.venues);
  
  const [tabValue, setTabValue] = useState(0);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [deleteLoading, setDeleteLoading] = useState(false);

  useEffect(() => {
    if (id) {
      dispatch(fetchVenueById(id) as any);
      dispatch(fetchVenueBusinessHours(id) as any);
      dispatch(fetchVenueSpecials(id) as any);
    }
  }, [id, dispatch]);

  const handleTabChange = (_: React.SyntheticEvent, newValue: number) => {
    setTabValue(newValue);
  };

  const handleEditVenue = () => {
    navigate(`/venues/${id}/edit`);
  };

  const handleDeleteVenue = async () => {
    if (!id) return;
    
    setDeleteLoading(true);
    try {
      await dispatch(deleteVenue({ id, apiClient }) as any);
      setDeleteDialogOpen(false);
      navigate('/backoffice');
    } catch (error) {
      console.error(error);
    } finally {
      setDeleteLoading(false);
    }
  };

  const handleAddSpecial = () => {
    navigate(`/venues/${id}/specials/new`);
  };

  if (loading && !currentVenue) {
    return (
      <Box display="flex" justifyContent="center" my={4}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Alert severity="error" sx={{ mt: 2 }}>
        {error}
      </Alert>
    );
  }

  if (!currentVenue) {
    return (
      <Alert severity="warning" sx={{ mt: 2 }}>
        Venue not found
      </Alert>
    );
  }

  return (
    <Box>
      <Box sx={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'flex-start',
        mb: 4 
      }}>
        <Box>
          <Typography variant="h4" component="h1">
            {currentVenue.name}
          </Typography>
          <Typography variant="subtitle1" color="text.secondary" sx={{ mt: 1 }}>
            {currentVenue.locality}, {currentVenue.region}
          </Typography>
        </Box>
        
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
            onClick={() => setDeleteDialogOpen(true)}
          >
            Delete Venue
          </Button>
        </Box>
      </Box>

      <Grid container spacing={3}>
        <Grid item xs={12} md={4}>
          {currentVenue.profileImage ? (
            <CardMedia
              component="img"
              height="200"
              image={currentVenue.profileImage}
              alt={currentVenue.name}
              sx={{ objectFit: 'cover', borderRadius: 1 }}
            />
          ) : (
            <Paper 
              sx={{ 
                height: 200, 
                bgcolor: 'grey.200', 
                display: 'flex', 
                alignItems: 'center', 
                justifyContent: 'center',
                borderRadius: 1
              }}
            >
              <Typography variant="body1" color="text.secondary">
                No image available
              </Typography>
            </Paper>
          )}

          <Card sx={{ mt: 3 }}>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Contact Information
              </Typography>
              
              {currentVenue.phoneNumber && (
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <PhoneIcon fontSize="small" sx={{ mr: 1 }} />
                  <Typography variant="body2">
                    {currentVenue.phoneNumber}
                  </Typography>
                </Box>
              )}
              
              {currentVenue.email && (
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <EmailIcon fontSize="small" sx={{ mr: 1 }} />
                  <Typography variant="body2">
                    {currentVenue.email}
                  </Typography>
                </Box>
              )}
              
              {currentVenue.website && (
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <WebsiteIcon fontSize="small" sx={{ mr: 1 }} />
                  <Typography variant="body2">
                    {currentVenue.website}
                  </Typography>
                </Box>
              )}
              
              <Box sx={{ display: 'flex', alignItems: 'flex-start', mb: 1 }}>
                <PlaceIcon fontSize="small" sx={{ mr: 1, mt: 0.3 }} />
                <Typography variant="body2">
                  {currentVenue.streetAddress}
                  {currentVenue.secondaryAddress && <>, {currentVenue.secondaryAddress}</>}<br />
                  {currentVenue.locality}, {currentVenue.region} {currentVenue.postcode}<br />
                  {currentVenue.country}
                </Typography>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        
        <Grid item xs={12} md={8}>
          <Paper sx={{ width: '100%' }}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
              <Tabs value={tabValue} onChange={handleTabChange} aria-label="venue tabs">
                <Tab label="Details" />
                <Tab label="Business Hours" />
                <Tab label="Specials" />
              </Tabs>
            </Box>
            
            <TabPanel value={tabValue} index={0}>
              <Typography variant="h6" gutterBottom>
                Description
              </Typography>
              <Typography paragraph>
                {currentVenue.description || "No description provided."}
              </Typography>
            </TabPanel>
            
            <TabPanel value={tabValue} index={1}>
              <Typography variant="h6" gutterBottom>
                Business Hours
              </Typography>
              <BusinessHoursDisplay schedules={venueBusinessHours} />
            </TabPanel>
            
            <TabPanel value={tabValue} index={2}>
              <Box sx={{ 
                display: 'flex', 
                justifyContent: 'space-between', 
                alignItems: 'center',
                mb: 2 
              }}>
                <Typography variant="h6">
                  Specials
                </Typography>
                <Button 
                  variant="contained" 
                  startIcon={<AddIcon />}
                  onClick={handleAddSpecial}
                >
                  Add Special
                </Button>
              </Box>
              
              {venueSpecials.length > 0 ? (
                <SpecialsList specials={venueSpecials} showVenueName={false} />
              ) : (
                <Typography variant="body1" textAlign="center" py={3}>
                  No specials found for this venue. Create your first special to get started.
                </Typography>
              )}
            </TabPanel>
          </Paper>
        </Grid>
      </Grid>

      <Dialog
        open={deleteDialogOpen}
        onClose={() => setDeleteDialogOpen(false)}
      >
        <DialogTitle>Confirm Delete</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete "{currentVenue.name}"? This action cannot be undone,
            and all associated specials and business hours will also be deleted.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button 
            onClick={() => setDeleteDialogOpen(false)} 
            disabled={deleteLoading}
          >
            Cancel
          </Button>
          <Button 
            onClick={handleDeleteVenue} 
            color="error" 
            disabled={deleteLoading}
          >
            {deleteLoading ? <CircularProgress size={24} /> : "Delete"}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default VenueManagementPage;
