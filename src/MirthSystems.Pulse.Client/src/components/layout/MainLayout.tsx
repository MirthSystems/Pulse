import { Outlet } from 'react-router-dom';
import { Box, Container, useTheme } from '@mui/material';
import Header from './Header';

const MainLayout = () => {
  const theme = useTheme();

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <Header />
      
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          p: 3,
          minHeight: 'calc(100vh - 64px)',
          backgroundColor: theme.palette.background.default
        }}
      >
        <Container maxWidth="xl" sx={{ mt: 2 }}>
          <Outlet />
        </Container>
      </Box>
    </Box>
  );
};

export default MainLayout;
