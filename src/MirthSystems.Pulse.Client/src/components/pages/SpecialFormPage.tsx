import { useState, useEffect } from 'react';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
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
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  FormHelperText,
  FormControlLabel,
  Switch,
  FormGroup,
  Autocomplete
} from '@mui/material';
import { 
  Save as SaveIcon, 
  ArrowBack as ArrowBackIcon
} from '@mui/icons-material';
import { DateTime } from 'luxon';

import { RootState } from '@store/index';
import { VenueService } from '@services/venueService';
import { getSpecialById, createSpecial, updateSpecial, clearCurrentSpecial, clearSpecialsError } from '@features/specials/specialSlice';
import { VenueItem } from '@models/venue';
import { CreateSpecialRequest, UpdateSpecialRequest, SpecialTypes } from '@models/special';
import { useApiClient } from '@services/apiClient';

const SpecialFormPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const apiClient = useApiClient();
  const { isAuthenticated, isLoading: authLoading } = useAuth0();
  const [searchParams] = useSearchParams();
  
  const { currentSpecial, loading, error } = useSelector((state: RootState) => state.specials);

  // Form state
  const [content, setContent] = useState<string>('');
  const [type, setType] = useState<SpecialTypes>(SpecialTypes.Drink);
  const [startDate, setStartDate] = useState<string>(DateTime.now().toFormat('yyyy-MM-dd'));
  const [startTime, setStartTime] = useState<string>('17:00');
  const [endTime, setEndTime] = useState<string>('19:00');
  const [expirationDate, setExpirationDate] = useState<string>('');
  const [isRecurring, setIsRecurring] = useState<boolean>(false);
  const [cronSchedule, setCronSchedule] = useState<string>('0 17 * * 1-5'); // Weekdays at 5pm
  
  // Venue selection
  const [venueId, setVenueId] = useState<string>('');
  const [venueName, setVenueName] = useState<string>('');
  const [venueOptions, setVenueOptions] = useState<VenueItem[]>([]);
  const [loadingVenues, setLoadingVenues] = useState<boolean>(false);
  const [venueSearchText, setVenueSearchText] = useState<string>('');
  
  // Form submission and error states
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [formErrors, setFormErrors] = useState<{ [key: string]: string }>({});
  
  // Load venues
  useEffect(() => {
    const loadVenues = async () => {
      if (!isAuthenticated) return;
      
      try {
        setLoadingVenues(true);
        const result = await VenueService.getVenues({
          page: 1,
          pageSize: 100,
          searchText: venueSearchText,
          includeAddress: true,
        });
        setVenueOptions(result.items);
      } catch (error) {
        console.error('Failed to load venues:', error);
      } finally {
        setLoadingVenues(false);
      }
    };
    
    loadVenues();
  }, [isAuthenticated, venueSearchText]);
  
  // Fetch special data if in edit mode
  useEffect(() => {
    if (id) {
      dispatch(getSpecialById(id) as any);
    } else {
      // Clear current special if we're in create mode
      dispatch(clearCurrentSpecial());
      dispatch(clearSpecialsError());
      
      // Check for venueId in query parameters
      const queryVenueId = searchParams.get('venueId');
      if (queryVenueId) {
        setVenueId(queryVenueId);
        
        // Fetch venue details to get the name
        const loadVenueDetails = async () => {
          try {
            const venue = await VenueService.getVenueById(queryVenueId);
            if (venue) {
              setVenueName(venue.name);
            }
          } catch (error) {
            console.error('Failed to load venue details:', error);
          }
        };
        
        loadVenueDetails();
      }
    }
    
    return () => {
      // Cleanup
      dispatch(clearCurrentSpecial());
      dispatch(clearSpecialsError());
    };
  }, [id, dispatch, searchParams]);
  
  // Populate form with special data in edit mode
  useEffect(() => {
    if (currentSpecial && id) {
      setContent(currentSpecial.content);
      setType(currentSpecial.type);
      setStartDate(currentSpecial.startDate);
      setStartTime(currentSpecial.startTime);
      setEndTime(currentSpecial.endTime || '');
      setExpirationDate(currentSpecial.expirationDate || '');
      setIsRecurring(currentSpecial.isRecurring);
      setCronSchedule(currentSpecial.cronSchedule || '');
      setVenueId(currentSpecial.venueId);
      
      if (currentSpecial.venue) {
        setVenueName(currentSpecial.venue.name);
      }
    }
  }, [currentSpecial, id]);
  
  // Validate form data
  const validateForm = (): boolean => {
    const errors: { [key: string]: string } = {};
    
    if (!content.trim()) errors.content = "Special description is required";
    else if (content.trim().length < 5) errors.content = "Description must be at least 5 characters";
    else if (content.trim().length > 200) errors.content = "Description cannot exceed 200 characters";
    
    if (!startDate) errors.startDate = "Start date is required";
    
    if (!startTime) errors.startTime = "Start time is required";
    else if (!/^([01]?[0-9]|2[0-3]):[0-5][0-9]$/.test(startTime)) 
      errors.startTime = "Start time must be in format HH:MM (24-hour)";
    
    if (endTime && !/^([01]?[0-9]|2[0-3]):[0-5][0-9]$/.test(endTime)) 
      errors.endTime = "End time must be in format HH:MM (24-hour)";
    
    if (isRecurring && !cronSchedule.trim()) 
      errors.cronSchedule = "CRON schedule is required for recurring specials";
    
    if (!venueId) errors.venueId = "Please select a venue";
    
    setFormErrors(errors);
    return Object.keys(errors).length === 0;
  };
  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) return;
    
    setIsSubmitting(true);
    
    try {
      if (id) {
        // Edit mode
        const updateData: UpdateSpecialRequest = {
          content,
          type,
          startDate,
          startTime,
          endTime: endTime || undefined,
          expirationDate: expirationDate || undefined,
          isRecurring,
          cronSchedule: isRecurring ? cronSchedule : undefined
        };
        
        await dispatch(updateSpecial({ id, specialData: updateData, apiClient }) as any);
        navigate(`/venues/${venueId}?tab=specials`);
      } else {
        // Create mode
        const createData: CreateSpecialRequest = {
          venueId,
          content,
          type,
          startDate,
          startTime,
          endTime: endTime || undefined,
          expirationDate: expirationDate || undefined,
          isRecurring,
          cronSchedule: isRecurring ? cronSchedule : undefined
        };
        
        await dispatch(createSpecial({ specialData: createData, apiClient }) as any);
        navigate(`/venues/${venueId}?tab=specials`);
      }
    } catch (error) {
      console.error("Error saving special:", error);
    } finally {
      setIsSubmitting(false);
    }
  };
  
  // Check if user is authorized
  if (!authLoading && !isAuthenticated) {
    return (
      <Container>
        <Alert severity="error" sx={{ mt: 2 }}>
          You must be logged in to manage specials.
        </Alert>
        <Button 
          variant="contained" 
          onClick={() => navigate('/specials')} 
          sx={{ mt: 2 }}
        >
          Back to Specials
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
    <Container maxWidth="md">
      <Paper elevation={3} sx={{ p: 3, mb: 4 }}>
        <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Typography variant="h5" component="h1">
            {id ? 'Edit Special' : 'Add New Special'}
          </Typography>
          <Button
            variant="outlined"
            startIcon={<ArrowBackIcon />}
            onClick={() => navigate(venueId ? `/venues/${venueId}` : '/specials')}
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
            {!id && (
              <Grid item xs={12}>
                <Autocomplete
                  id="venue-select"
                  options={venueOptions}
                  getOptionLabel={(option) => `${option.name} (${option.locality}, ${option.region})`}
                  loading={loadingVenues}
                  value={venueOptions.find(v => v.id === venueId) || null}
                  onChange={(_, newValue) => {
                    if (newValue) {
                      setVenueId(newValue.id || '');
                      setVenueName(newValue.name);
                    } else {
                      setVenueId('');
                      setVenueName('');
                    }
                  }}
                  onInputChange={(_, newInputValue) => {
                    setVenueSearchText(newInputValue);
                  }}
                  renderInput={(params) => (
                    <TextField
                      {...params}
                      label="Select Venue"
                      required
                      error={!!formErrors.venueId}
                      helperText={formErrors.venueId}
                      InputProps={{
                        ...params.InputProps,
                        endAdornment: (
                          <>
                            {loadingVenues ? <CircularProgress color="inherit" size={20} /> : null}
                            {params.InputProps.endAdornment}
                          </>
                        ),
                      }}
                    />
                  )}
                  disabled={!!id || isSubmitting}
                />
              </Grid>
            )}
            
            {id && (
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Venue"
                  value={venueName}
                  disabled
                  InputProps={{ readOnly: true }}
                />
              </Grid>
            )}

            <Grid item xs={12}>
              <TextField
                fullWidth
                required
                multiline
                rows={2}
                label="Special Description"
                value={content}
                onChange={(e) => setContent(e.target.value)}
                error={!!formErrors.content}
                helperText={formErrors.content}
                disabled={isSubmitting}
                placeholder="Example: 'Half-Price Wings Happy Hour' or 'Live Jazz Night'"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <FormControl fullWidth error={!!formErrors.type}>
                <InputLabel id="special-type-label">Type</InputLabel>
                <Select
                  labelId="special-type-label"
                  value={type}
                  label="Type"
                  onChange={(e) => setType(e.target.value as SpecialTypes)}
                  disabled={isSubmitting}
                >
                  <MenuItem value={SpecialTypes.Food}>Food</MenuItem>
                  <MenuItem value={SpecialTypes.Drink}>Drink</MenuItem>
                  <MenuItem value={SpecialTypes.Entertainment}>Entertainment</MenuItem>
                </Select>
                {formErrors.type && <FormHelperText>{formErrors.type}</FormHelperText>}
              </FormControl>
            </Grid>
            
            <Grid item xs={12} sm={6}>
              <FormGroup>
                <FormControlLabel
                  control={
                    <Switch
                      checked={isRecurring}
                      onChange={(e) => setIsRecurring(e.target.checked)}
                      disabled={isSubmitting}
                    />
                  }
                  label="Recurring Special"
                />
              </FormGroup>
              <Typography variant="caption" color="text.secondary">
                {isRecurring 
                  ? "This special repeats on a regular schedule"
                  : "This is a one-time special"
                }
              </Typography>
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                required
                type="date"
                label="Start Date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                error={!!formErrors.startDate}
                helperText={formErrors.startDate}
                disabled={isSubmitting}
                InputLabelProps={{
                  shrink: true,
                }}
              />
            </Grid>

            {isRecurring && (
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  type="date"
                  label="Expiration Date (optional)"
                  value={expirationDate}
                  onChange={(e) => setExpirationDate(e.target.value)}
                  error={!!formErrors.expirationDate}
                  helperText={formErrors.expirationDate || "Leave blank for no expiration"}
                  disabled={isSubmitting}
                  InputLabelProps={{
                    shrink: true,
                  }}
                />
              </Grid>
            )}

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                required
                type="time"
                label="Start Time"
                value={startTime}
                onChange={(e) => setStartTime(e.target.value)}
                error={!!formErrors.startTime}
                helperText={formErrors.startTime}
                disabled={isSubmitting}
                InputLabelProps={{
                  shrink: true,
                }}
                inputProps={{
                  step: 300, // 5 min
                }}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="time"
                label="End Time (optional)"
                value={endTime}
                onChange={(e) => setEndTime(e.target.value)}
                error={!!formErrors.endTime}
                helperText={formErrors.endTime || "Leave blank for all-day specials"}
                disabled={isSubmitting}
                InputLabelProps={{
                  shrink: true,
                }}
                inputProps={{
                  step: 300, // 5 min
                }}
              />
            </Grid>
            
            {isRecurring && (
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  required
                  label="CRON Schedule"
                  value={cronSchedule}
                  onChange={(e) => setCronSchedule(e.target.value)}
                  error={!!formErrors.cronSchedule}
                  helperText={formErrors.cronSchedule || "CRON expression for when this special recurs (e.g., '0 17 * * 1-5' for weekdays at 5 PM)"}
                  disabled={isSubmitting}
                />
                <Box sx={{ mt: 1, mb: 0, fontSize: '0.875rem', color: 'text.secondary' }}>
                  Common patterns:
                  <ul style={{ margin: '0.5rem 0', paddingLeft: '1.5rem' }}>
                    <li>Daily at 5 PM: 0 17 * * *</li>
                    <li>Weekdays at 5 PM: 0 17 * * 1-5</li>
                    <li>Weekends at 4 PM: 0 16 * * 0,6</li>
                    <li>Every Monday at 8 PM: 0 20 * * 1</li>
                  </ul>
                </Box>
              </Grid>
            )}

            <Grid item xs={12} sx={{ mt: 2 }}>
              <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 2 }}>
                <Button
                  variant="outlined"
                  onClick={() => navigate(venueId ? `/venues/${venueId}` : '/specials')}
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
                  {isSubmitting ? 'Saving...' : 'Save Special'}
                </Button>
              </Box>
            </Grid>
          </Grid>
        </form>
      </Paper>
    </Container>
  );
};

export default SpecialFormPage;
