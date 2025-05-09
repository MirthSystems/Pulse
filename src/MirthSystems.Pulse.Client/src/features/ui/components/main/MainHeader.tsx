import { FC } from 'react';
import { NavLink as RouterLink } from 'react-router-dom';
import {
  AppBar,
  Toolbar,
  Button,
  Stack,
  useTheme,
  useMediaQuery,
  Typography,
} from '@mui/material';
import { ThemeToggle } from '../ThemeToggle';
import { Profile } from '../../../user/components/Profile';

export const MainHeader: FC = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  const navItems = [
    { name: 'Home', path: '/' },
    { name: 'Search', path: '/search' },
    { name: 'Specials', path: '/specials' },
    { name: 'Venues', path: '/venues' }
  ];

  return (
    <AppBar 
      position="static" 
      color="default"
      elevation={0}
      sx={{
        backgroundColor: 'transparent',
        borderBottom: `1px solid ${theme.palette.divider}`,
      }}
    >
      <Toolbar sx={{ justifyContent: 'space-between' }}>
        {/* Logo */}
        <Typography 
          variant="h6" 
          component={RouterLink} 
          to="/" 
          sx={{ 
            flexGrow: { xs: 1, sm: 0 },
            textDecoration: 'none',
            fontWeight: 700,
            letterSpacing: '0.05rem',
            color: theme.palette.primary.main,
            display: 'flex',
            alignItems: 'center'
          }}
        >
          Pulse
        </Typography>

        {/* Navigation Links */}
        {!isMobile && (
          <Stack 
            direction="row" 
            spacing={1} 
            sx={{ mx: 'auto' }}
          >
            {navItems.map((item) => (
              <Button
                key={item.name}
                component={RouterLink}
                to={item.path}
                color="inherit"
                sx={{ 
                  textTransform: 'none',
                  fontWeight: 500,
                  '&.active': {
                    color: theme.palette.primary.main,
                  }
                }}
              >
                {item.name}
              </Button>
            ))}
          </Stack>
        )}

        {/* User Profile and Theme Toggle */}
        <Stack direction="row" spacing={1} alignItems="center">
          <ThemeToggle />
          <Profile />
        </Stack>
      </Toolbar>
    </AppBar>
  );
};