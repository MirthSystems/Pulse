import { Container, Typography, Box } from '@mui/material';
import { VenueListView } from '../components/venues';

export const BackofficePage = () => {
  return (
    <Container maxWidth="xl">
      <Box sx={{ mb: 3 }}>
        <Typography variant="h4" component="h1">
          Backoffice
        </Typography>
      </Box>
      <VenueListView />
    </Container>
  );
};
