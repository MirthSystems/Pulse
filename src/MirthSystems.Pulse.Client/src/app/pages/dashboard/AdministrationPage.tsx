import { 
  Box, 
  Typography, 
  Paper,
  Card,
  CardContent,
  CardActions,
  Button,
  Grid,
  Alert
} from '@mui/material';
import AdminPanelSettingsIcon from '@mui/icons-material/AdminPanelSettings';
import { useAuth } from '../../../features/user/hooks';

const AdministrationPage = () => {
  const { isAdmin, isVenueManager } = useAuth();
  
  return (
    <Box>
      <Typography variant="h4" component="h1" gutterBottom sx={{ display: 'flex', alignItems: 'center' }}>
        <AdminPanelSettingsIcon sx={{ mr: 1 }} /> Administration
      </Typography>
      
      <Alert severity="info" sx={{ mb: 4 }}>
        This page is only accessible to administrators and venue managers.
      </Alert>
      
      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Venue Management
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Create, edit, and manage venues in the system.
              </Typography>
            </CardContent>
            <CardActions>
              <Button size="small">Manage Venues</Button>
            </CardActions>
          </Card>
        </Grid>
        
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Specials Management
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Create, edit, and manage specials for venues.
              </Typography>
            </CardContent>
            <CardActions>
              <Button size="small">Manage Specials</Button>
            </CardActions>
          </Card>
        </Grid>
        
        {isAdmin() && (
          <Grid item xs={12} md={6}>
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  User Management
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Manage user accounts and permissions (admin only).
                </Typography>
              </CardContent>
              <CardActions>
                <Button size="small">Manage Users</Button>
              </CardActions>
            </Card>
          </Grid>
        )}
      </Grid>
    </Box>
  );
};

export default AdministrationPage;