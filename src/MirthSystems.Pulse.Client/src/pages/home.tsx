import { Box, Container, Typography, Button, Paper } from '@mui/material';

export const HomePage = () => {
  return (
    <Container maxWidth="lg">
      <Box 
        sx={{ 
          my: 4,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          textAlign: 'center'
        }}
      >
        <Typography variant="h2" component="h1" gutterBottom>
          Welcome to Pulse
        </Typography>
        <Typography variant="h5" color="text.secondary" paragraph>
          Your platform for managing venues, specials, and operating schedules.
        </Typography>
        
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: {
              xs: 'repeat(1, 1fr)',
              sm: 'repeat(3, 1fr)',
            },
            gap: 4,
            mt: 4,
          }}
        >
          <Box
            sx={{
              gridColumn: 'span 12',
            }}
          >
            <Paper
              elevation={2}
              sx={{
                p: 3,
                height: '100%',
                display: 'flex',
                flexDirection: 'column'
              }}
            >
              <Typography variant="h5" gutterBottom>
                Venues
              </Typography>
              <Typography variant="body1" component="p" sx={{ flexGrow: 1 }}>
                Manage your venues, locations, and business details all in one place.
              </Typography>
              <Button variant="outlined" color="primary">
                Explore Venues
              </Button>
            </Paper>
          </Box>
          <Box
            sx={{
              gridColumn: 'span 12',
            }}
          >
            <Paper
              elevation={2}
              sx={{
                p: 3,
                height: '100%',
                display: 'flex',
                flexDirection: 'column'
              }}
            >
              <Typography variant="h5" gutterBottom>
                Specials
              </Typography>
              <Typography variant="body1" component="p" sx={{ flexGrow: 1 }}>
                Create and manage promotions, deals, and special offers for your venues.
              </Typography>
              <Button variant="outlined" color="primary">
                View Specials
              </Button>
            </Paper>
          </Box>
          <Box
            sx={{
              gridColumn: 'span 12',
            }}
          >
            <Paper
              elevation={2}
              sx={{
                p: 3,
                height: '100%',
                display: 'flex',
                flexDirection: 'column'
              }}
            >
              <Typography variant="h5" gutterBottom>
                Schedules
              </Typography>
              <Typography variant="body1" component="p" sx={{ flexGrow: 1 }}>
                Set and manage operating hours, business schedules, and availability.
              </Typography>
              <Button variant="outlined" color="primary">
                Manage Schedules
              </Button>
            </Paper>
          </Box>
        </Box>
      </Box>
    </Container>
  );
};
