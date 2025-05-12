import { Link as RouterLink } from 'react-router-dom';
import { Box, Button, Container, Typography } from '@mui/material';
import { SentimentDissatisfied as SadIcon, Home as HomeIcon } from '@mui/icons-material';

const NotFoundPage = () => {
  return (
    <Container maxWidth="md">
      <Box 
        sx={{ 
          display: 'flex', 
          flexDirection: 'column', 
          alignItems: 'center', 
          justifyContent: 'center',
          textAlign: 'center',
          minHeight: '70vh',
          py: 4
        }}
      >
        <SadIcon sx={{ fontSize: 100, color: 'text.secondary', mb: 4 }} />
        
        <Typography variant="h3" component="h1" gutterBottom>
          404: Page Not Found
        </Typography>
        
        <Typography variant="h5" color="text.secondary" paragraph>
          We can't seem to find the page you're looking for.
        </Typography>
        
        <Typography variant="body1" color="text.secondary" sx={{ mb: 4 }}>
          The page might have been removed, had its name changed, or is temporarily unavailable.
        </Typography>
        
        <Button 
          variant="contained" 
          component={RouterLink} 
          to="/"
          startIcon={<HomeIcon />}
          size="large"
        >
          Back to Home
        </Button>
      </Box>
    </Container>
  );
};

export default NotFoundPage;
