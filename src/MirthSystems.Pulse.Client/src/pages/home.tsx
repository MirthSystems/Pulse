import { Box, Container } from '@mui/material';
import { SearchSpecials } from '../components/specials/search-specials';
import { WelcomeContent } from '../components/welcome';

export const HomePage = () => {
  return (
    <Container maxWidth="lg">
      <Box sx={{ my: { xs: 2, md: 4 } }}>
        {/* Main search section */}
        <SearchSpecials />

        {/* Welcome content with features */}
        <WelcomeContent />
      </Box>
    </Container>
  );
};
