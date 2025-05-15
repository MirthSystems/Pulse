import { Container, Typography } from '@mui/material';
import { VenueList } from '../components/venues';

export const BackofficePage = () => {
  return (
    <Container maxWidth="lg">
      <Typography variant="h4" component="h1" gutterBottom>
        Backoffice
      </Typography>
      
      <VenueList />
    </Container>
  );
};
