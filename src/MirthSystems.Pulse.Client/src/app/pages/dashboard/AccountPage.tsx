import { useEffect, useState } from 'react';
import { 
  Box, 
  Typography, 
  Paper,
  Avatar,
  Grid,
  Divider,
  List,
  ListItem,
  ListItemText,
  CircularProgress
} from '@mui/material';
import { useAuth } from '../../../features/user/hooks';

const AccountPage = () => {
  const { user } = useAuth();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Simulate loading user data
    const timer = setTimeout(() => {
      setLoading(false);
    }, 500);
    
    return () => clearTimeout(timer);
  }, []);

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '50vh' }}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box>
      <Typography variant="h4" component="h1" gutterBottom>
        Account Profile
      </Typography>
      
      <Paper elevation={3} sx={{ p: 4, mb: 4 }}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={4} sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
            <Avatar 
              src={user?.picture}
              alt={user?.name}
              sx={{ width: 128, height: 128, mb: 2 }}
            />
            <Typography variant="h6">
              {user?.name}
            </Typography>
            <Typography color="textSecondary">
              {user?.email}
            </Typography>
          </Grid>
          
          <Grid item xs={12} md={8}>
            <Typography variant="h6" gutterBottom>
              Profile Information
            </Typography>
            <Divider sx={{ mb: 2 }} />
            
            <List disablePadding>
              <ListItem>
                <ListItemText 
                  primary="Email" 
                  secondary={user?.email || 'Not available'}
                  primaryTypographyProps={{ fontWeight: 'bold' }}
                />
              </ListItem>
              <Divider component="li" />
              
              <ListItem>
                <ListItemText 
                  primary="Nickname" 
                  secondary={user?.nickname || 'Not available'}
                  primaryTypographyProps={{ fontWeight: 'bold' }}
                />
              </ListItem>
              <Divider component="li" />
              
              <ListItem>
                <ListItemText 
                  primary="Email Verified" 
                  secondary={user?.email_verified ? 'Yes' : 'No'}
                  primaryTypographyProps={{ fontWeight: 'bold' }}
                />
              </ListItem>
              <Divider component="li" />
              
              <ListItem>
                <ListItemText 
                  primary="Account Updated" 
                  secondary={user?.updated_at ? new Date(user.updated_at).toLocaleString() : 'Not available'}
                  primaryTypographyProps={{ fontWeight: 'bold' }}
                />
              </ListItem>
            </List>
          </Grid>
        </Grid>
      </Paper>
      
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h6" gutterBottom>
          User Claims
        </Typography>
        <Divider sx={{ mb: 2 }} />
        
        <Box sx={{ bgcolor: 'background.default', p: 2, borderRadius: 1 }}>
          <pre style={{ overflow: 'auto', margin: 0 }}>
            {JSON.stringify(user, null, 2)}
          </pre>
        </Box>
      </Paper>
    </Box>
  );
};

export default AccountPage;