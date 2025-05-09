import { Outlet } from 'react-router-dom';
import { Container, Box, AppBar, Toolbar, Typography } from '@mui/material';
import { ProfileMenu } from '../../user/index';
import { ThemeToggle } from '../components';

/**
 * Default layout for public pages
 * Simple header with just profile menu in top right
 */
export const DefaultLayout = () => {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Pulse
          </Typography>
          <ThemeToggle />
          <Profile />
        </Toolbar>
      </AppBar>
      
      <Container component="main" maxWidth="lg" sx={{ mt: 4, mb: 4, flexGrow: 1 }}>
        <Outlet />
      </Container>
      
      <Box component="footer" sx={{ py: 3, px: 2, mt: 'auto', backgroundColor: (theme) => theme.palette.grey[200] }}>
        <Container maxWidth="lg">
          <Typography variant="body2" color="text.secondary" align="center">
            © {new Date().getFullYear()} Mirth Systems
          </Typography>
        </Container>
      </Box>
    </Box>
  );
};