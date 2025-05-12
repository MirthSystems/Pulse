import { AppBar, Toolbar, Typography, Box, Button } from '@mui/material';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import { useAuth0 } from '@auth0/auth0-react';
import UserMenu from './UserMenu';

const Header = () => {
  const { isAuthenticated } = useAuth0();
  const location = useLocation();
  const isBackoffice = location.pathname.includes('/backoffice') || 
                      location.pathname.includes('/venues/');

  return (
    <AppBar position="static" color="primary">
      <Toolbar>
        <Typography 
          variant="h6" 
          component={RouterLink} 
          to="/"
          sx={{ 
            flexGrow: 1,
            textDecoration: 'none',
            color: 'inherit',
            fontWeight: 700,
            letterSpacing: '0.05em'
          }}
        >
          PULSE
        </Typography>

        {isAuthenticated && isBackoffice && (
          <Button 
            color="inherit" 
            component={RouterLink} 
            to="/"
            sx={{ mr: 2 }}
          >
            Main Site
          </Button>
        )}

        <Box sx={{ ml: 2 }}>
          <UserMenu isBackoffice={isBackoffice} />
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;
