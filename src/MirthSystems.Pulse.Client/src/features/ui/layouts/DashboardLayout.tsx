import { useState } from 'react';
import { Outlet, Link as RouterLink, useLocation } from 'react-router-dom';
import { 
  Box, 
  Drawer, 
  AppBar, 
  Toolbar, 
  List, 
  Typography, 
  Divider, 
  IconButton, 
  ListItem, 
  ListItemButton, 
  ListItemIcon, 
  ListItemText,
  Container 
} from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import AdminPanelSettingsIcon from '@mui/icons-material/AdminPanelSettings';
import HomeIcon from '@mui/icons-material/Home';
import { ThemeToggle } from '../components';
import { ProfileMenu } from '../../user/components';
import { useAuth } from '../../user/hooks';

const drawerWidth = 240;

/**
 * Dashboard layout with sidebar navigation for authenticated users
 */
export const DashboardLayout = () => {
  const [open, setOpen] = useState(true);
  const { isAdmin, isVenueManager } = useAuth();
  const location = useLocation();
  
  const toggleDrawer = () => {
    setOpen(!open);
  };

  const isSelected = (path: string) => {
    return location.pathname === path || 
           (path !== '/dashboard' && location.pathname.startsWith(path));
  };

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            edge="start"
            onClick={toggleDrawer}
            sx={{ mr: 2 }}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1 }}>
            Pulse Dashboard
          </Typography>
          <ThemeToggle />
          <ProfileMenu />
        </Toolbar>
      </AppBar>
      
      <Drawer
        variant="persistent"
        anchor="left"
        open={open}
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
          },
        }}
      >
        <Toolbar />
        <Box sx={{ overflow: 'auto', mt: 2 }}>
          <List>
            <ListItem disablePadding>
              <ListItemButton
                component={RouterLink}
                to="/"
                sx={{ 
                  backgroundColor: isSelected('/') ? 'action.selected' : 'inherit' 
                }}
              >
                <ListItemIcon>
                  <HomeIcon />
                </ListItemIcon>
                <ListItemText primary="Home" />
              </ListItemButton>
            </ListItem>
            
            <ListItem disablePadding>
              <ListItemButton
                component={RouterLink}
                to="/dashboard/account"
                sx={{ 
                  backgroundColor: isSelected('/dashboard/account') ? 'action.selected' : 'inherit' 
                }}
              >
                <ListItemIcon>
                  <AccountCircleIcon />
                </ListItemIcon>
                <ListItemText primary="Account" />
              </ListItemButton>
            </ListItem>
            
            {(isAdmin() || isVenueManager()) && (
              <ListItem disablePadding>
                <ListItemButton
                  component={RouterLink}
                  to="/dashboard/administration"
                  sx={{ 
                    backgroundColor: isSelected('/dashboard/administration') ? 'action.selected' : 'inherit' 
                  }}
                >
                  <ListItemIcon>
                    <AdminPanelSettingsIcon />
                  </ListItemIcon>
                  <ListItemText primary="Administration" />
                </ListItemButton>
              </ListItem>
            )}
          </List>
        </Box>
      </Drawer>
      
      <Box component="main" sx={{ 
        flexGrow: 1, 
        p: 3,
        marginLeft: open ? `${drawerWidth}px` : 0,
        transition: theme => theme.transitions.create(['margin', 'width'], {
          easing: theme.transitions.easing.sharp,
          duration: theme.transitions.duration.leavingScreen,
        }),
      }}>
        <Toolbar />
        <Container maxWidth="lg">
          <Outlet />
        </Container>
      </Box>
    </Box>
  );
};