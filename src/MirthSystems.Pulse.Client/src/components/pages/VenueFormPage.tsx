import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { useAuth0 } from '@auth0/auth0-react';
import { 
  Box, 
  Typography, 
  Paper, 
  TextField, 
  Button, 
  Grid, 
  Container,
  Alert,
  CircularProgress,
  Divider,
  FormControlLabel,
  Switch,
  Stack
} from '@mui/material';
import { 
  Save as SaveIcon, 
  ArrowBack as ArrowBackIcon
} from '@mui/icons-material';

import { RootState } from '@store/index';
import { fetchVenueById, createVenue, updateVenue, clearVenueError } from '@features/venues/venueSlice';
import { AddressRequest, CreateVenueRequest, UpdateVenueRequest, OperatingHours } from '@models/venue';
import { useApiClient } from '@services/apiClient';
import BusinessHoursEditor from '@components/venues/BusinessHoursEditor';

const VenueFormPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const apiClient = useApiClient();
  const { isAuthenticated, isLoading: authLoading } = useAuth0();
  
  const { currentVenue, loading, error } = useSelector((state: RootState) => state.venues);

  // Form state
  const [name, setName] = useState<string>('');
  const [description, setDescription] = useState<string>('');
  const [phoneNumber, setPhoneNumber] = useState<string>('');
  const [website, setWebsite] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [profileImage, setProfileImage] = useState<string>('');
  
  // Address fields
  const [streetAddress, setStreetAddress] = useState<string>('');
  const [secondaryAddress, setSecondaryAddress] = useState<string>('');
  const [locality, setLocality] = useState<string>('');
  const [region, setRegion] = useState<string>('');
  const [postcode, setPostcode] = useState<string>('');
  const [country, setCountry] = useState<string>('United States');
  
  // Business hours
  const [businessHours, setBusinessHours] = useState<OperatingHours[]>([
    { dayOfWeek: 0, timeOfOpen: "11:00", timeOfClose: "22:00", isClosed: false }, // Sunday
    { dayOfWeek: 1, timeOfOpen: "11:00", timeOfClose: "22:00", isClosed: false }, // Monday
    { dayOfWeek: 2, timeOfOpen: "11:00", timeOfClose: "22:00", isClosed: false }, // Tuesday
    { dayOfWeek: 3, timeOfOpen: "11:00", timeOfClose: "22:00", isClosed: false }, // Wednesday
    { dayOfWeek: 4, timeOfOpen: "11:00", timeOfClose: "23:00", isClosed: false }, // Thursday
    { dayOfWeek: 5, timeOfOpen: "11:00", timeOfClose: "00:00", isClosed: false }, // Friday
    { dayOfWeek: 6, timeOfOpen: "11:00", timeOfClose: "00:00", isClosed: false }, // Saturday
  ]);
  
  // Form submission and error states
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [formErrors, setFormErrors] = useState<{ [key: string]: string }>({});
  
  // Fetch venue data if in edit mode
  useEffect(() => {
    if (id) {
      dispatch(fetchVenueById(id) as any);
    } else {
      // Clear current venue if we're in create mode
      dispatch(clearVenueError());
    }
  }, [id, dispatch]);
  
  // Populate form with venue data in edit mode
  useEffect(() => {
    if (currentVenue && id) {
      setName(currentVenue.name);
      setDescription(currentVenue.description || '');
      setPhoneNumber(currentVenue.phoneNumber || '');
      setWebsite(currentVenue.website || '');
      setEmail(currentVenue.email || '');
      setProfileImage(currentVenue.profileImage || '');
      
      setStreetAddress(currentVenue.streetAddress);
      setSecondaryAddress(currentVenue.secondaryAddress || '');
      setLocality(currentVenue.locality);
      setRegion(currentVenue.region);
      setPostcode(currentVenue.postcode);
      setCountry(currentVenue.country);
      
      if (currentVenue.businessHours?.length) {
        // Convert business hours from API to form model
        const hours: OperatingHours[] = currentVenue.businessHours.map(bh => ({
          dayOfWeek: bh.dayOfWeek,
          timeOfOpen: bh.openTime,
          timeOfClose: bh.closeTime,
          isClosed: bh.isClosed
        })).sort((a, b) => a.dayOfWeek - b.dayOfWeek);
        
        setBusinessHours(hours);
      }
    }
  }, [currentVenue, id]);
  
  // Validate form data
  const validateForm = (): boolean => {
    const errors: { [key: string]: string } = {};
    
    // Validate venue fields
    if (!name.trim()) errors.name = "Venue name is required";
    else if (name.trim().length < 2) errors.name = "Venue name must be at least 2 characters";
    else if (name.trim().length > 100) errors.name = "Venue name cannot exceed 100 characters";
    
    if (description && description.length > 500) 
      errors.description = "Description cannot exceed 500 characters";
    
    if (website && !/^https?:\/\/.+\..+$/.test(website)) 
      errors.website = "Website must be a valid URL (include http:// or https://)";
    
    if (email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) 
      errors.email = "Email must be a valid email address";
    
    if (profileImage && !/^https?:\/\/.+/.test(profileImage)) 
      errors.profileImage = "Profile image must be a valid URL (include http:// or https://)";
    
    // Validate address fields
    if (!streetAddress.trim()) errors.streetAddress = "Street address is required";
    if (!locality.trim()) errors.locality = "City is required";
    if (!region.trim()) errors.region = "State/province is required";
    if (!postcode.trim()) errors.postcode = "Postal code is required";
    if (!country.trim()) errors.country = "Country is required";
    
    // Check business hours
    const missingHours = businessHours.some(hour => {
      return !hour.isClosed && (!hour.timeOfOpen || !hour.timeOfClose);
    });
    
    if (missingHours) {
      errors.businessHours = "Please provide both opening and closing times for all open days";
    }
    
    setFormErrors(errors);
    return Object.keys(errors).length === 0;
  };
  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) return;
    
    setIsSubmitting(true);
    
    try {
      const addressData: AddressRequest = {
        streetAddress,
        secondaryAddress: secondaryAddress || undefined,
        locality,
        region,
        postcode,
        country
      };
      
      if (id) {
        // Edit mode
        const updateData: UpdateVenueRequest = {
          name,
          description: description || undefined,
          phoneNumber: phoneNumber || undefined,
          website: website || undefined,
          email: email || undefined,
          profileImage: profileImage || undefined,
          address: addressData
        };
        
        await dispatch(updateVenue({ id, venueData: updateData, apiClient }) as any);
      } else {
        // Create mode
        const createData: CreateVenueRequest = {
          name,
          description: description || undefined,
          phoneNumber: phoneNumber || undefined,
          website: website || undefined,
          email: email || undefined,
          profileImage: profileImage || undefined,
          address: addressData,
          hoursOfOperation: businessHours
        };
        
        const result = await dispatch(createVenue({ venueData: createData, apiClient }) as any);
        if (result.payload?.id) {
          navigate(`/venues/${result.payload.id}`);
          return;
        }
      }
      
      if (id) {
        navigate(`/venues/${id}`);
      }
    } catch (error) {
      console.error("Error saving venue:", error);
    } finally {
      setIsSubmitting(false);
    }
  };
  
  const handleUpdateBusinessHours = (dayIndex: number, field: keyof OperatingHours, value: any) => {
    const updatedHours = [...businessHours];
    updatedHours[dayIndex] = { ...updatedHours[dayIndex], [field]: value };
    setBusinessHours(updatedHours);
  };
  
  // Check if user is authorized
  if (!authLoading && !isAuthenticated) {
    return (
      <Container>
        <Alert severity="error" sx={{ mt: 2 }}>
          You must be logged in to manage venues.
        </Alert>
        <Button 
          variant="contained" 
          onClick={() => navigate('/venues')} 
          sx={{ mt: 2 }}
        >
          Back to Venues
        </Button>
      </Container>
    );
  }
  
  if (loading && id) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Container maxWidth="lg">
      <Paper elevation={3} sx={{ p: 3, mb: 4 }}>
        <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Typography variant="h5" component="h1">
            {id ? 'Edit Venue' : 'Add New Venue'}
          </Typography>
          <Button
            variant="outlined"
            startIcon={<ArrowBackIcon />}
            onClick={() => navigate(id ? `/venues/${id}` : '/venues')}
          >
            Back
          </Button>
        </Box>
        
        {error && (
          <Alert severity="error" sx={{ mb: 3 }}>
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                required
                label="Venue Name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                error={!!formErrors.name}
                helperText={formErrors.name}
                disabled={isSubmitting}
                margin="normal"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Phone Number"
                value={phoneNumber}
                onChange={(e) => setPhoneNumber(e.target.value)}
                error={!!formErrors.phoneNumber}
                helperText={formErrors.phoneNumber}
                disabled={isSubmitting}
                margin="normal"
                placeholder="+1 (555) 123-4567"
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                multiline
                rows={3}
                label="Description"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                error={!!formErrors.description}
                helperText={formErrors.description}
                disabled={isSubmitting}
                margin="normal"
                placeholder="Describe your venue in a few sentences..."
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Website"
                value={website}
                onChange={(e) => setWebsite(e.target.value)}
                error={!!formErrors.website}
                helperText={formErrors.website}
                disabled={isSubmitting}
                margin="normal"
                placeholder="https://www.yourvenue.com"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                error={!!formErrors.email}
                helperText={formErrors.email}
                disabled={isSubmitting}
                margin="normal"
                placeholder="contact@yourvenue.com"
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Profile Image URL"
                value={profileImage}
                onChange={(e) => setProfileImage(e.target.value)}
                error={!!formErrors.profileImage}
                helperText={formErrors.profileImage}
                disabled={isSubmitting}
                margin="normal"
                placeholder="https://example.com/your-image.jpg"
              />
            </Grid>
            
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="h6" gutterBottom>Address Information</Typography>
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                required
                label="Street Address"
                value={streetAddress}
                onChange={(e) => setStreetAddress(e.target.value)}
                error={!!formErrors.streetAddress}
                helperText={formErrors.streetAddress}
                disabled={isSubmitting}
                margin="normal"
                placeholder="123 Main St"
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Secondary Address"
                value={secondaryAddress}
                onChange={(e) => setSecondaryAddress(e.target.value)}
                error={!!formErrors.secondaryAddress}
                helperText={formErrors.secondaryAddress}
                disabled={isSubmitting}
                margin="normal"
                placeholder="Suite 100 (Optional)"
              />
            </Grid>

            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                required
                label="City"
                value={locality}
                onChange={(e) => setLocality(e.target.value)}
                error={!!formErrors.locality}
                helperText={formErrors.locality}
                disabled={isSubmitting}
                margin="normal"
              />
            </Grid>

            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                required
                label="State/Province"
                value={region}
                onChange={(e) => setRegion(e.target.value)}
                error={!!formErrors.region}
                helperText={formErrors.region}
                disabled={isSubmitting}
                margin="normal"
              />
            </Grid>

            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                required
                label="Postal Code"
                value={postcode}
                onChange={(e) => setPostcode(e.target.value)}
                error={!!formErrors.postcode}
                helperText={formErrors.postcode}
                disabled={isSubmitting}
                margin="normal"
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                required
                label="Country"
                value={country}
                onChange={(e) => setCountry(e.target.value)}
                error={!!formErrors.country}
                helperText={formErrors.country}
                disabled={isSubmitting}
                margin="normal"
              />
            </Grid>
            
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="h6" gutterBottom>Business Hours</Typography>
              {formErrors.businessHours && (
                <Alert severity="error" sx={{ mb: 2 }}>
                  {formErrors.businessHours}
                </Alert>
              )}
            </Grid>
            
            <Grid item xs={12}>
              <BusinessHoursEditor 
                businessHours={businessHours}
                onChange={handleUpdateBusinessHours}
                disabled={isSubmitting}
              />
            </Grid>

            <Grid item xs={12} sx={{ mt: 2 }}>
              <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 2 }}>
                <Button
                  variant="outlined"
                  onClick={() => navigate(id ? `/venues/${id}` : '/venues')}
                  disabled={isSubmitting}
                >
                  Cancel
                </Button>
                <Button
                  type="submit"
                  variant="contained"
                  color="primary"
                  startIcon={isSubmitting ? <CircularProgress size={20} color="inherit" /> : <SaveIcon />}
                  disabled={isSubmitting}
                >
                  {isSubmitting ? 'Saving...' : 'Save Venue'}
                </Button>
              </Box>
            </Grid>
          </Grid>
        </form>
      </Paper>
    </Container>
  );
};

export default VenueFormPage;
