import React from 'react';
import { 
  Grid, 
  Paper, 
  Typography, 
  Box, 
  List, 
  ListItem, 
  ListItemText, 
  Divider,
  Card,
  CardContent,
  CardHeader,
  Button,
  Stack,
  Chip,
  LinearProgress
} from '@mui/material';
import { MsalAuthenticationTemplate } from "@azure/msal-react";
import { InteractionType } from "@azure/msal-browser";
import { loginRequest } from "../../configs/auth";
import ErrorComponent from "@/components/ErrorComponent";
import Loading from "@/components/Loading";

// Sample data
const recentSpecials = [
  { id: 1, title: 'Happy Hour - 2 for 1 Drinks', status: 'active', views: 245 },
  { id: 2, title: 'Live Band Night', status: 'scheduled', views: 0 },
  { id: 3, title: 'Taco Tuesday', status: 'active', views: 352 },
  { id: 4, title: 'Wine Wednesday', status: 'scheduled', views: 0 },
];

const venueMetrics = {
  totalViews: 1243,
  activeSpecials: 2,
  scheduledSpecials: 2,
  postEngagement: 76
};

const DashboardContent: React.FC = () => {
  return (
    <Box sx={{ flexGrow: 1 }}>
      <Grid container spacing={3}>
        {/* Summary Cards */}
        <Grid item xs={12} md={6} lg={3}>
          <Paper
            sx={{
              p: 2,
              display: 'flex',
              flexDirection: 'column',
              height: 140,
            }}
          >
            <Typography variant="h6" color="text.secondary" gutterBottom>
              Total Venue Views
            </Typography>
            <Typography component="p" variant="h4">
              {venueMetrics.totalViews}
            </Typography>
            <Typography variant="body2" sx={{ flex: 1 }}>
              in the last 7 days
            </Typography>
          </Paper>
        </Grid>
        <Grid item xs={12} md={6} lg={3}>
          <Paper
            sx={{
              p: 2,
              display: 'flex',
              flexDirection: 'column',
              height: 140,
            }}
          >
            <Typography variant="h6" color="text.secondary" gutterBottom>
              Active Specials
            </Typography>
            <Typography component="p" variant="h4">
              {venueMetrics.activeSpecials}
            </Typography>
            <Box sx={{ display: 'flex', alignItems: 'center' }}>
              <Typography variant="body2" color="success.main">
                +28% from last month
              </Typography>
            </Box>
          </Paper>
        </Grid>
        <Grid item xs={12} md={6} lg={3}>
          <Paper
            sx={{
              p: 2,
              display: 'flex',
              flexDirection: 'column',
              height: 140,
            }}
          >
            <Typography variant="h6" color="text.secondary" gutterBottom>
              Scheduled Specials
            </Typography>
            <Typography component="p" variant="h4">
              {venueMetrics.scheduledSpecials}
            </Typography>
            <Typography variant="body2">
              ready to go live
            </Typography>
          </Paper>
        </Grid>
        <Grid item xs={12} md={6} lg={3}>
          <Paper
            sx={{
              p: 2,
              display: 'flex',
              flexDirection: 'column',
              height: 140,
            }}
          >
            <Typography variant="h6" color="text.secondary" gutterBottom>
              Post Engagement
            </Typography>
            <Typography component="p" variant="h4">
              {venueMetrics.postEngagement}
            </Typography>
            <Typography variant="body2">
              user interactions this week
            </Typography>
          </Paper>
        </Grid>

        {/* Recent Specials */}
        <Grid item xs={12}>
          <Paper sx={{ p: 2 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
              <Typography variant="h6" component="h2">
                Recent Specials
              </Typography>
              <Button variant="outlined" size="small">
                View All
              </Button>
            </Box>
            <Divider sx={{ mb: 2 }} />
            <List>
              {recentSpecials.map((special, index) => (
                <React.Fragment key={special.id}>
                  <ListItem 
                    button
                    sx={{ 
                      py: 1.5, 
                      px: 2,
                      '&:hover': { bgcolor: 'rgba(0, 0, 0, 0.04)' }
                    }}
                  >
                    <Grid container alignItems="center">
                      <Grid item xs={6} md={5}>
                        <ListItemText 
                          primary={special.title}
                          primaryTypographyProps={{ fontWeight: 500 }} 
                        />
                      </Grid>
                      <Grid item xs={3} md={2} sx={{ textAlign: 'center' }}>
                        <Chip 
                          label={special.status} 
                          color={special.status === 'active' ? 'success' : 'primary'} 
                          size="small"
                          variant="outlined"
                        />
                      </Grid>
                      <Grid item xs={3} md={3} sx={{ textAlign: 'center' }}>
                        <Stack direction="column" spacing={1}>
                          <Typography variant="body2" color="text.secondary">
                            {special.status === 'active' ? 'Views' : 'Scheduled'}
                          </Typography>
                          {special.status === 'active' ? (
                            <Typography variant="body1" fontWeight={500}>
                              {special.views}
                            </Typography>
                          ) : (
                            <Typography variant="body2" color="text.secondary">
                              Apr 30, 2025
                            </Typography>
                          )}
                        </Stack>
                      </Grid>
                      <Grid item xs={12} md={2} sx={{ textAlign: 'right' }}>
                        <Button size="small">Edit</Button>
                      </Grid>
                    </Grid>
                  </ListItem>
                  {index < recentSpecials.length - 1 && <Divider />}
                </React.Fragment>
              ))}
            </List>
          </Paper>
        </Grid>

        {/* Quick Actions and Activity */}
        <Grid item xs={12} md={4}>
          <Card>
            <CardHeader title="Quick Actions" />
            <CardContent>
              <Stack spacing={2}>
                <Button variant="contained" fullWidth>
                  Create New Special
                </Button>
                <Button variant="outlined" fullWidth>
                  Add Venue
                </Button>
                <Button variant="outlined" fullWidth>
                  View Analytics
                </Button>
              </Stack>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={8}>
          <Card>
            <CardHeader 
              title="Engagement Overview" 
              subheader="Last 7 days activity"
            />
            <CardContent>
              <Box sx={{ mb: 4 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                  <Typography variant="body2">Happy Hour Views</Typography>
                  <Typography variant="body2" fontWeight={500}>245/500 target</Typography>
                </Box>
                <LinearProgress 
                  variant="determinate" 
                  value={49} 
                  sx={{ height: 8, borderRadius: 4 }} 
                />
              </Box>
              
              <Box sx={{ mb: 4 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                  <Typography variant="body2">Taco Tuesday Views</Typography>
                  <Typography variant="body2" fontWeight={500}>352/400 target</Typography>
                </Box>
                <LinearProgress 
                  variant="determinate" 
                  value={88} 
                  sx={{ height: 8, borderRadius: 4 }} 
                />
              </Box>
              
              <Box>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                  <Typography variant="body2">User Engagement</Typography>
                  <Typography variant="body2" fontWeight={500}>76/100 target</Typography>
                </Box>
                <LinearProgress 
                  variant="determinate" 
                  value={76} 
                  sx={{ height: 8, borderRadius: 4 }} 
                />
              </Box>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Box>
  );
};

// Wrapper components for MSAL authentication to fix type issues
const MsalErrorComponent: React.FC<unknown> = () => (
  <ErrorComponent 
    title="Authentication Error" 
    message="There was a problem authenticating you. Please try again." 
  />
);
const MsalLoadingComponent = () => <Loading />;

const Dashboard: React.FC = () => {
  return (
    <MsalAuthenticationTemplate
      interactionType={InteractionType.Redirect}
      authenticationRequest={loginRequest}
      errorComponent={MsalErrorComponent}
      loadingComponent={MsalLoadingComponent}
    >
      <DashboardContent />
    </MsalAuthenticationTemplate>
  );
};

export default Dashboard;