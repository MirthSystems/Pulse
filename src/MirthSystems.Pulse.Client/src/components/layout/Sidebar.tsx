import { useLocation, Link as RouterLink } from 'react-router-dom';
import { useAuth0 } from '@auth0/auth0-react';
import {
  Box,
  Divider,
  Drawer,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar,
  Typography,
} from '@mui/material';
import {
  Search as SearchIcon,
  Nightlife as NightlifeIcon,
  EventNote as EventNoteIcon,
  Add as AddIcon,
  AdminPanelSettings as AdminPanelSettingsIcon,
} from '@mui/icons-material';

interface SidebarProps {
  drawerWidth: number;
  mobileOpen: boolean;
  handleDrawerToggle: () => void;
  isMobile: boolean;
}

const Sidebar = ({ drawerWidth, mobileOpen, handleDrawerToggle, isMobile }: SidebarProps) => {
  const location = useLocation();
  const { isAuthenticated } = useAuth0();

  const navigationItems = [
    { text: 'Discover Venues', icon: <SearchIcon />, path: '/' },
    { text: 'All Venues', icon: <NightlifeIcon />, path: '/venues' },
    { text: 'Specials', icon: <EventNoteIcon />, path: '/specials' },
  ];

  const authenticatedItems = [
    { text: 'Add New Venue', icon: <AddIcon />, path: '/venues/new' },
    { text: 'Add New Special', icon: <AddIcon />, path: '/specials/new' },
    { text: 'Admin Dashboard', icon: <AdminPanelSettingsIcon />, path: '/admin' },
  ];

  const drawer = (
    <div>
      <Toolbar>
        <Typography variant="h6" noWrap component="div">
          Navigation
        </Typography>
      </Toolbar>
      <Divider />
      <List>
        {navigationItems.map((item) => (
          <ListItem key={item.text} disablePadding>
            <ListItemButton
              component={RouterLink}
              to={item.path}
              selected={location.pathname === item.path}
            >
              <ListItemIcon>{item.icon}</ListItemIcon>
              <ListItemText primary={item.text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
      
      {isAuthenticated && (
        <>
          <Divider />
          <List>
            {authenticatedItems.map((item) => (
              <ListItem key={item.text} disablePadding>
                <ListItemButton
                  component={RouterLink}
                  to={item.path}
                  selected={location.pathname === item.path}
                >
                  <ListItemIcon>{item.icon}</ListItemIcon>
                  <ListItemText primary={item.text} />
                </ListItemButton>
              </ListItem>
            ))}
          </List>
        </>
      )}
    </div>
  );

  return (
    <Box
      component="nav"
      sx={{ width: { md: drawerWidth }, flexShrink: { md: 0 } }}
    >
      {isMobile ? (
        <Drawer
          variant="temporary"
          open={mobileOpen}
          onClose={handleDrawerToggle}
          ModalProps={{
            keepMounted: true, // Better open performance on mobile.
          }}
          sx={{
            display: { xs: 'block', md: 'none' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
          }}
        >
          {drawer}
        </Drawer>
      ) : (
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: 'none', md: 'block' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
          }}
          open
        >
          {drawer}
        </Drawer>
      )}
    </Box>
  );
};

export default Sidebar;
