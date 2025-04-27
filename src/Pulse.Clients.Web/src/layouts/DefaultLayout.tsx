import React, { ReactNode } from 'react';
import {
  Box,
  Container,
  Typography,
  Link,
  Grid,
  Stack,
  Divider,
  IconButton
} from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import SpecialsSearch from '../components/search/SpecialsSearch';
import LoginButton from '../components/auth/LoginButton';

// Icons
import FacebookIcon from '@mui/icons-material/Facebook';
import TwitterIcon from '@mui/icons-material/Twitter';
import InstagramIcon from '@mui/icons-material/Instagram';
import LocalFireDepartmentIcon from '@mui/icons-material/LocalFireDepartment';

interface DefaultLayoutProps {
  children: ReactNode;
}

// Footer links configuration
const footerLinks = [
  {
    title: 'Company',
    items: [
      { name: 'About', href: '/about' },
      { name: 'Blog', href: '/blog' },
      { name: 'Careers', href: '/careers' },
      { name: 'Partners', href: '/partners' },
    ],
  },
  {
    title: 'Resources',
    items: [
      { name: 'For Venues', href: '/resources/venues' },
      { name: 'For Foodies', href: '/resources/foodies' },
      { name: 'For Night Owls', href: '/resources/night-owls' },
      { name: 'Press Kit', href: '/press-kit' },
    ],
  },
  {
    title: 'Legal',
    items: [
      { name: 'Privacy', href: '/privacy' },
      { name: 'Terms', href: '/terms' },
      { name: 'Cookie Policy', href: '/cookies' },
      { name: 'Licenses', href: '/licenses' },
    ],
  },
];

const DefaultLayout: React.FC<DefaultLayoutProps> = ({ children }) => {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      {/* Auth button (top-right corner) */}
      <LoginButton />
      
      {/* Hero section */}
      <Box
        sx={{
          position: 'relative',
          backgroundColor: 'primary.dark',
          color: 'white',
          pt: { xs: 12, sm: 16 },
          pb: { xs: 10, sm: 20 },
          overflow: 'hidden',
        }}
      >
        {/* Background pattern */}
        <Box 
          sx={{ 
            position: 'absolute', 
            top: 0, 
            left: 0, 
            right: 0, 
            bottom: 0, 
            opacity: 0.1,
            backgroundImage: 'radial-gradient(circle, rgba(255,255,255,0.3) 2px, transparent 0)',
            backgroundSize: '30px 30px',
          }} 
        />

        <Container maxWidth="lg">
          <Stack direction="row" alignItems="center" spacing={1} sx={{ mb: 2 }}>
            <LocalFireDepartmentIcon sx={{ fontSize: 32 }} />
            <Typography
              variant="h5"
              component={RouterLink}
              to="/"
              sx={{
                fontWeight: 700,
                color: 'white',
                textDecoration: 'none',
              }}
            >
              PULSE
            </Typography>
          </Stack>

          <Grid container spacing={4} alignItems="center" sx={{ mt: 4 }}>
            <Grid item xs={12} md={7}>
              <Typography
                component="h1"
                variant="h2"
                color="inherit"
                sx={{
                  fontWeight: 700,
                  letterSpacing: '-0.025em',
                }}
              >
                Real-time Nightlife Discovery
              </Typography>
              <Typography
                variant="h5"
                color="inherit"
                sx={{ 
                  mt: 2, 
                  mb: 4, 
                  opacity: 0.9, 
                  maxWidth: '600px' 
                }}
              >
                Discover the perfect vibes happening right now with real-time updates from venues and fellow nightlife explorers.
              </Typography>
              
              {/* Search component */}
              <Box sx={{ mt: 6, mb: { xs: 6, md: 0 } }}>
                <SpecialsSearch />
              </Box>
            </Grid>
            
            <Grid item xs={12} md={5} sx={{ display: { xs: 'none', md: 'block' } }}>
              {/* Placeholder for hero image - would be replaced with real image */}
              <Box
                sx={{
                  height: 400,
                  width: '100%',
                  backgroundColor: 'rgba(255,255,255,0.1)',
                  borderRadius: 4,
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  boxShadow: '0 10px 40px rgba(0,0,0,0.2)',
                }}
              >
                <Typography variant="body1" sx={{ color: 'white', opacity: 0.7 }}>
                  Hero Image
                </Typography>
              </Box>
            </Grid>
          </Grid>
        </Container>
      </Box>

      {/* Main content */}
      <Box sx={{ flexGrow: 1, py: 8 }}>
        <Container maxWidth="lg">
          {children}
        </Container>
      </Box>

      {/* Footer */}
      <Box
        component="footer"
        sx={{
          py: 8,
          px: 2,
          mt: 'auto',
          backgroundColor: (theme) => theme.palette.grey[50],
        }}
      >
        <Container maxWidth="lg">
          <Grid container spacing={4} justifyContent="space-between">
            {footerLinks.map((section) => (
              <Grid item xs={12} sm={4} key={section.title}>
                <Typography variant="h6" color="text.primary" gutterBottom>
                  {section.title}
                </Typography>
                <ul style={{ margin: 0, padding: 0, listStyle: 'none' }}>
                  {section.items.map((item) => (
                    <li key={item.name}>
                      <Link
                        component={RouterLink}
                        to={item.href}
                        variant="subtitle1"
                        color="text.secondary"
                        sx={{
                          textDecoration: 'none',
                          '&:hover': {
                            textDecoration: 'underline',
                          },
                          display: 'inline-block',
                          py: 0.5,
                        }}
                      >
                        {item.name}
                      </Link>
                    </li>
                  ))}
                </ul>
              </Grid>
            ))}

            <Grid item xs={12} sm={12}>
              <Divider sx={{ my: 3 }} />
              <Box
                sx={{
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  flexDirection: { xs: 'column', sm: 'row' },
                  gap: 2,
                }}
              >
                <Typography variant="body2" color="text.secondary">
                  {'© '}
                  {new Date().getFullYear()}{' '}
                  Pulse. All rights reserved.
                </Typography>
                <Box sx={{ display: 'flex', gap: 1 }}>
                  <IconButton color="inherit" aria-label="Facebook" size="small">
                    <FacebookIcon fontSize="small" />
                  </IconButton>
                  <IconButton color="inherit" aria-label="Twitter" size="small">
                    <TwitterIcon fontSize="small" />
                  </IconButton>
                  <IconButton color="inherit" aria-label="Instagram" size="small">
                    <InstagramIcon fontSize="small" />
                  </IconButton>
                </Box>
              </Box>
            </Grid>
          </Grid>
        </Container>
      </Box>
    </Box>
  );
};

export default DefaultLayout;