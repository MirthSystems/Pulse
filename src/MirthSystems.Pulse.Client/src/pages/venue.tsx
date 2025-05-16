import { Container, Box } from '@mui/material';
import { VenueView } from '../components/venues/venue-view';

export const VenuePage = () => {
  return (
    <Container maxWidth="xl">
      <Box sx={{ mb: 3 }}>
        <VenueView />
      </Box>
    </Container>
  );
};
