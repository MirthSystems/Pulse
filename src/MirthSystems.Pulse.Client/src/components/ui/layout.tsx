import { Box } from '@mui/material';
import { Header } from './header';
import { Footer } from './footer';
import { Outlet } from 'react-router-dom';

export const Layout = () => {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        minHeight: '100vh',
      }}
    >
      <Header />
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          py: 4,
          px: { xs: 2, md: 4 },
          width: '100%',
        }}
      >
        <Outlet />
      </Box>
      <Footer />
    </Box>
  );
};
