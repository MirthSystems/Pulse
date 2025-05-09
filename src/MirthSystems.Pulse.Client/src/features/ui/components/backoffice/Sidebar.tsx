import { FC, ReactNode } from 'react';
import { useLocation, Link as RouterLink } from 'react-router-dom';
import {
  Box,
  Button,
  Divider,
  Drawer,
  List,
  Typography,
  useTheme,
  ListSubheader
} from '@mui/material';
import {
  Dashboard as DashboardIcon,
  Business as BusinessIcon,
  Settings as SettingsIcon
} from '@mui/icons-material';
import { useAuth } from '../../../user/hooks';
import { SidebarNavItem } from './SidebarNavItem';

export interface SidebarProps {
  open: boolean;
  onClose: () => void;
  width?: number;
  variant?: 'permanent' | 'persistent' | 'temporary';
}

interface NavItem {
  title: string;
  path: string;
  icon: ReactNode;
  requiresAdmin?: boolean;
}

export const Sidebar: FC<SidebarProps> = ({
  open,
  onClose,
  width = 280,
  variant = 'permanent'
}) => {
  const location = useLocation();
  const theme = useTheme();
  const { isAdmin } = useAuth();

  // Define navigation items
  const navItems: NavItem[] = [
    {
      title: 'Dashboard',
      path: '/backoffice',
      icon: <DashboardIcon />
    },
    {
      title: 'Administration',
      path: '/backoffice/venues',
      icon: <BusinessIcon />
    },
    {
      title: 'Settings',
      path: '/backoffice/settings',
      icon: <SettingsIcon />,
      requiresAdmin: true
    }
  ];

  const content = (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        height: '100%'
      }}
    >
      {/* Brand/Logo */}
      <Box
        sx={{
          p: 3,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center'
        }}
      >
        <Box
          component={RouterLink}
          to="/backoffice"
          sx={{
            display: 'inline-flex',
            textDecoration: 'none',
            color: 'primary.main'
          }}
        >
          <Typography
            variant="h5"
            sx={{
              fontWeight: 700,
              letterSpacing: 0.5
            }}
          >
            Pulse Admin
          </Typography>
        </Box>
      </Box>

      <Divider />

      {/* Navigation Items */}
      <Box sx={{ flexGrow: 1, p: 2 }}>
        <List>
          {navItems.map((item) => {
            // Skip admin-only items if user is not admin
            if (item.requiresAdmin && !isAdmin()) {
              return null;
            }

            return (
              <SidebarNavItem
                key={item.title}
                icon={item.icon}
                path={item.path}
                title={item.title}
                active={location.pathname === item.path || location.pathname.startsWith(`${item.path}/`)}
              />
            );
          })}
        </List>

        {isAdmin() && (
          <>
            <Divider sx={{ my: 2 }} />
            <List
              subheader={
                <ListSubheader
                  sx={{
                    backgroundColor: 'transparent',
                    color: 'text.secondary',
                    lineHeight: '30px'
                  }}
                >
                  SYSTEM ADMIN
                </ListSubheader>
              }
            >
              <SidebarNavItem
                icon={<SettingsIcon />}
                path="/backoffice/settings"
                title="Settings"
                active={location.pathname === '/backoffice/settings'}
              />
            </List>
          </>
        )}
      </Box>

      <Divider />

      {/* Footer */}
      <Box sx={{ p: 2 }}>
        <Button
          component={RouterLink}
          to="/"
          variant="outlined"
          fullWidth
          sx={{ mt: 2 }}
        >
          Return to Main Site
        </Button>
      </Box>
    </Box>
  );

  return (
    <Drawer
      anchor="left"
      onClose={onClose}
      open={open}
      variant={variant}
      PaperProps={{
        sx: {
          backgroundColor: theme.palette.background.paper,
          width,
          boxSizing: 'border-box'
        }
      }}
    >
      {content}
    </Drawer>
  );
};