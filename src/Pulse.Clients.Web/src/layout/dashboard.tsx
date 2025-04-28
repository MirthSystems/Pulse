import React, { useState, ReactNode, useEffect } from 'react';
import { styled } from '@mui/material/styles';
import {
  Box,
  Drawer,
  AppBar,
  Toolbar,
  List,
  Typography,
  Divider,
  IconButton,
  Container,
  Badge,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  useTheme,
  useMediaQuery
} from '@mui/material';
import { Link, useLocation } from 'react-router-dom';

// Components
import UserMenu from '../components/user/UserMenu';
import ThemeToggle from '../components/ThemeToggle';
import Breadcrumbs from '../components/routing/Breadcrumbs';
import BackButton from '../components/routing/BackButton';

// Icons
import MenuIcon from '@mui/icons-material/Menu';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import DashboardIcon from '@mui/icons-material/Dashboard';
import PersonIcon from '@mui/icons-material/Person';
import LocalBarIcon from '@mui/icons-material/LocalBar';
import SettingsIcon from '@mui/icons-material/Settings';
import NotificationsIcon from '@mui/icons-material/Notifications';
import LocalFireDepartmentIcon from '@mui/icons-material/LocalFireDepartment';

// Route configuration
import { getNavigationRoutes } from '../configs/router';

const drawerWidth = 240;

// Styled components
const StyledAppBar = styled(AppBar, {
  shouldForwardProp: (prop) => prop !== 'open',
})<{ open?: boolean }>(({ theme, open }) => ({
  zIndex: theme.zIndex.drawer + 1,
  transition: theme.transitions.create(['width', 'margin'], {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  ...(open && {
    marginLeft: drawerWidth,
    width: `calc(100% - ${drawerWidth}px)`,
    transition: theme.transitions.create(['width', 'margin'], {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.enteringScreen,
    }),
  }),
}));

const StyledDrawer = styled(Drawer, { shouldForwardProp: (prop) => prop !== 'open' })(
  ({ theme, open }) => ({
    '& .MuiDrawer-paper': {
      position: 'relative',
      whiteSpace: 'nowrap',
      width: drawerWidth,
      transition: theme.transitions.create('width', {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.enteringScreen,
      }),
      boxSizing: 'border-box',
      ...(!open && {
        overflowX: 'hidden',
        transition: theme.transitions.create('width', {
          easing: theme.transitions.easing.sharp,
          duration: theme.transitions.duration.leavingScreen,
        }),
        width: theme.spacing(7),
        [theme.breakpoints.up('sm')]: {
          width: theme.spacing(9),
        },
      }),
    },
  }),
);

// Map route icons to components
const getIconComponent = (iconType: string) => {
  switch (iconType) {
    case 'dashboard':
      return <DashboardIcon />;
    case 'profile':
      return <PersonIcon />;
    case 'specials':
      return <LocalBarIcon />;
    case 'settings':
      return <SettingsIcon />;
    default:
      return <DashboardIcon />;
  }
};

interface DashboardLayoutProps {
  children: ReactNode;
  title?: string;
  hideHeader?: boolean;
  hideFooter?: boolean;
  fullWidth?: boolean;
  showBreadcrumbs?: boolean;
}

const DashboardLayout: React.FC<DashboardLayoutProps> = ({ 
  children, 
  title = 'Dashboard',
  hideHeader = false,
  fullWidth = false,
  showBreadcrumbs = true
}) => {
  const [open, setOpen] = useState(false);
  const location = useLocation();
  const theme = useTheme();
  const isLargeScreen = useMediaQuery(theme.breakpoints.up('md'));
  
  // Auto-open drawer on large screens, closed on small screens
  useEffect(() => {
    setOpen(isLargeScreen);
  }, [isLargeScreen]);

  const toggleDrawer = () => {
    setOpen(!open);
  };

  // Get navigation items from route config
  const navItems = getNavigationRoutes();
  
  // Separate main and secondary items
  const mainNavItems = navItems.filter(
    item => item.layout === 'dashboard' && item.priority && item.priority < 100
  );
  
  const secondaryNavItems = navItems.filter(
    item => item.layout === 'dashboard' && item.priority && item.priority >= 100
  );

  if (hideHeader) {
    return (
      <Box 
        component="main"
        sx={{
          backgroundColor: theme.palette.background.default,
          flexGrow: 1,
          height: '100vh',
          overflow: 'auto',
          p: 3
        }}
      >
        <Container maxWidth={fullWidth ? false : "lg"}>
          {children}
        </Container>
      </Box>
    );
  }

  return (
    <Box sx={{ display: 'flex', height: '100vh' }}>
      <StyledAppBar position="absolute" open={open}>
        <Toolbar
          sx={{
            pr: '24px',
          }}
        >
          <IconButton
            edge="start"
            color="inherit"
            aria-label={open ? "close drawer" : "open drawer"}
            onClick={toggleDrawer}
            sx={{
              marginRight: '36px',
              ...(open && isLargeScreen && { display: 'none' }),
            }}
          >
            <MenuIcon />
          </IconButton>
          
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <BackButton variant="icon" sx={{ color: 'inherit' }} />
            
            <LocalFireDepartmentIcon sx={{ display: { xs: 'none', sm: 'flex' }, mr: 1 }} />
            <Typography
              component="h1"
              variant="h6"
              color="inherit"
              noWrap
              sx={{ flexGrow: 1 }}
            >
              {title}
            </Typography>
          </Box>
          
          <Box sx={{ display: 'flex', alignItems: 'center', ml: 'auto', gap: 1 }}>
            <ThemeToggle size="small" />
            
            <IconButton color="inherit" aria-label="notifications">
              <Badge badgeContent={4} color="secondary">
                <NotificationsIcon />
              </Badge>
            </IconButton>
            
            <Box sx={{ ml: 1 }}>
              <UserMenu />
            </Box>
          </Box>
        </Toolbar>
      </StyledAppBar>
      
      <StyledDrawer variant="permanent" open={open}>
        <Toolbar
          sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'flex-end',
            px: [1],
          }}
        >
          <Box sx={{ 
            display: 'flex', 
            alignItems: 'center', 
            justifyContent: open ? 'space-between' : 'center',
            width: '100%' 
          }}>
            {open && (
              <Typography
                component={Link}
                to="/"
                variant="h6"
                sx={{ 
                  display: 'flex', 
                  alignItems: 'center', 
                  color: 'inherit',
                  textDecoration: 'none',
                  fontWeight: 600 
                }}
              >
                <LocalFireDepartmentIcon sx={{ mr: 1 }} />
                PULSE
              </Typography>
            )}
            <IconButton onClick={toggleDrawer}>
              <ChevronLeftIcon />
            </IconButton>
          </Box>
        </Toolbar>
        
        <Divider />
        
        <List component="nav">
          {mainNavItems.map((item) => (
            <ListItemButton 
              key={item.path}
              component={Link} 
              to={item.path}
              selected={location.pathname === item.path}
              sx={{
                minHeight: 48,
                px: 2.5,
                '&.Mui-selected': {
                  backgroundColor: 'rgba(0, 0, 0, 0.08)',
                  '&:hover': {
                    backgroundColor: 'rgba(0, 0, 0, 0.12)',
                  },
                },
              }}
            >
              <ListItemIcon sx={{ minWidth: 0, mr: open ? 3 : 'auto', justifyContent: 'center' }}>
                {item.icon ? getIconComponent(item.icon) : <DashboardIcon />}
              </ListItemIcon>
              <ListItemText 
                primary={item.title} 
                sx={{ opacity: open ? 1 : 0 }} 
                primaryTypographyProps={{ noWrap: true }}
              />
            </ListItemButton>
          ))}
          
          {secondaryNavItems.length > 0 && (
            <>
              <Divider sx={{ my: 1 }} />
              
              {secondaryNavItems.map((item) => (
                <ListItemButton 
                  key={item.path} 
                  component={Link} 
                  to={item.path}
                  selected={location.pathname === item.path}
                  sx={{
                    minHeight: 48,
                    px: 2.5,
                    '&.Mui-selected': {
                      backgroundColor: 'rgba(0, 0, 0, 0.08)',
                      '&:hover': {
                        backgroundColor: 'rgba(0, 0, 0, 0.12)',
                      },
                    },
                  }}
                >
                  <ListItemIcon sx={{ minWidth: 0, mr: open ? 3 : 'auto', justifyContent: 'center' }}>
                    {item.icon ? getIconComponent(item.icon) : <SettingsIcon />}
                  </ListItemIcon>
                  <ListItemText 
                    primary={item.title} 
                    sx={{ opacity: open ? 1 : 0 }}
                    primaryTypographyProps={{ noWrap: true }}
                  />
                </ListItemButton>
              ))}
            </>
          )}
        </List>
      </StyledDrawer>
      
      <Box
        component="main"
        sx={{
          backgroundColor: theme.palette.background.default,
          flexGrow: 1,
          height: '100vh',
          overflow: 'auto',
          pt: 8,
        }}
      >
        <Toolbar />
        <Container maxWidth={fullWidth ? false : "lg"} sx={{ mt: 2, mb: 4, px: { xs: 2, sm: 3 } }}>
          {/* Breadcrumbs */}
          {showBreadcrumbs && <Breadcrumbs />}
          
          {children}
        </Container>
      </Box>
    </Box>
  );
};

export default DashboardLayout;