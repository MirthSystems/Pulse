import React from 'react';
import { Box, Typography, Paper, Drawer, List, ListItem, ListItemButton, ListItemText, Toolbar, AppBar, CssBaseline } from '@mui/material';
import { useNavigate, Routes, Route } from 'react-router-dom';

const drawerWidth = 220;

function AdminVenues() {
  return <Typography variant="h6">Venues management coming soon...</Typography>;
}
function AdminSpecials() {
  return <Typography variant="h6">Specials management coming soon...</Typography>;
}
function AdminVenueTypes() {
  return <Typography variant="h6">Venue Types management coming soon...</Typography>;
}
function AdminTags() {
  return <Typography variant="h6">Tags management coming soon...</Typography>;
}
function AdminSettings() {
  return <Typography variant="h6">Settings coming soon...</Typography>;
}

const navItems = [
  { label: 'Venues', path: '/admin/venues' },
  { label: 'Specials', path: '/admin/specials' },
  { label: 'Venue Types', path: '/admin/venue-types' },
  { label: 'Tags', path: '/admin/tags' },
  { label: 'Settings', path: '/admin/settings' },
];

export default function Admin() {
  const navigate = useNavigate();
  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
        <Toolbar>
          <Typography variant="h6" noWrap component="div">
            Admin Portal
          </Typography>
        </Toolbar>
      </AppBar>
      <Drawer
        variant="permanent"
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          [`& .MuiDrawer-paper`]: { width: drawerWidth, boxSizing: 'border-box' },
        }}
      >
        <Toolbar />
        <List>
          {navItems.map((item) => (
            <ListItem key={item.path} disablePadding>
              <ListItemButton onClick={() => navigate(item.path)}>
                <ListItemText primary={item.label} />
              </ListItemButton>
            </ListItem>
          ))}
        </List>
      </Drawer>
      <Box component="main" sx={{ flexGrow: 1, bgcolor: '#f5f5f5', p: 3, minHeight: '100vh' }}>
        <Toolbar />
        <Routes>
          <Route path="venues" element={<AdminVenues />} />
          <Route path="specials" element={<AdminSpecials />} />
          <Route path="venue-types" element={<AdminVenueTypes />} />
          <Route path="tags" element={<AdminTags />} />
          <Route path="settings" element={<AdminSettings />} />
          <Route path="*" element={<Typography variant="h5">Select an admin section.</Typography>} />
        </Routes>
      </Box>
    </Box>
  );
}