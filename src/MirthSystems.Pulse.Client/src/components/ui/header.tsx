import NightlifeIcon from '@mui/icons-material/Nightlife'; // Changed to nightlife icon
import {
  AppBar,
  Box,
  Container,
  Toolbar,
  Typography,
  useTheme
} from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { Auth } from '../identity/auth';
import { ThemeSwitch } from './theme-switch';

export const Header = () => {
  const theme = useTheme();

  return (
    <AppBar
      position="sticky"
      color="transparent"
      elevation={0}
      sx={{
        backdropFilter: 'blur(10px)',
        backgroundColor: theme.palette.mode === 'light'
          ? 'rgba(255, 255, 255, 0.8)'
          : 'rgba(13, 18, 23, 0.85)',
        borderBottom: '1px solid',
        borderColor: theme.palette.mode === 'light'
          ? 'rgba(0, 194, 203, 0.1)'
          : 'rgba(0, 231, 242, 0.1)'
      }}
    >
      <Container maxWidth="lg">
        <Toolbar sx={{ px: { xs: 0 } }}>
          <Box
            component={RouterLink}
            to="/"
            sx={{
              display: 'flex',
              alignItems: 'center',
              textDecoration: 'none',
              color: 'inherit',
              flexGrow: 1
            }}
          >
            <Box
              sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                width: 40,
                height: 40,
                borderRadius: '50%',
                mr: 1,
                background: theme.palette.mode === 'light'
                  ? 'linear-gradient(135deg, #00C2CB 30%, #FF7E45 70%)'
                  : 'linear-gradient(135deg, #00E7F2 30%, #FF9255 70%)',
                boxShadow: theme.palette.mode === 'light'
                  ? '0 3px 10px rgba(0, 194, 203, 0.2)'
                  : '0 3px 10px rgba(0, 231, 242, 0.3)'
              }}
            >
              <NightlifeIcon sx={{ color: '#fff', fontSize: '1.5rem' }} />
            </Box>

            <Typography
              variant="h5"
              sx={{
                fontWeight: 800,
                letterSpacing: '-0.01em',
                background: theme.palette.mode === 'light'
                  ? 'linear-gradient(90deg, #00C2CB 30%, #FF7E45 70%)'
                  : 'linear-gradient(90deg, #00E7F2 30%, #FF9255 70%)',
                WebkitBackgroundClip: 'text',
                WebkitTextFillColor: 'transparent',
                textShadow: theme.palette.mode === 'light'
                  ? '0 0 30px rgba(0, 194, 203, 0.2)'
                  : '0 0 30px rgba(0, 231, 242, 0.3)',
              }}
            >
              Pulse
            </Typography>
          </Box>

          <Box sx={{ display: 'flex', gap: 1 }}>
            <ThemeSwitch />
            <Auth />
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
};
