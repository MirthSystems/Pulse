import { 
  AppBar,
  Toolbar, 
  Typography
} from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { ThemeSwitch } from './theme-switch';
import { Auth } from '../identity/auth';

export const Header = () => {
  return (
    <AppBar position="static" color="primary" elevation={1}>
      <Toolbar>
        <Typography 
          variant="h6" 
          component={RouterLink} 
          to="/" 
          sx={{ 
            textDecoration: 'none', 
            color: 'inherit', 
            flexGrow: 1,
            fontWeight: 'bold'
          }}
        >
          Pulse
        </Typography>
        
        <ThemeSwitch />
        <Auth />
      </Toolbar>
    </AppBar>
  );
};
