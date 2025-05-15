import { Container, Typography } from '@mui/material';
import { VenueListView } from '../components/venues';

export const BackofficePage = () => {
  return (
    <Container maxWidth="lg">
      <Typography variant="h4" component="h1" gutterBottom>
        Backoffice
      </Typography>
      
      <VenueListView />
    </Container>
  );
};
