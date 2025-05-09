import { FC } from 'react';
import {
  AppBar,
  Box,
  IconButton,
  Toolbar,
  Typography,
  useTheme,
} from '@mui/material';
import { Menu as MenuIcon } from '@mui/icons-material';
import { ThemeToggle } from '../ThemeToggle';
import { Profile } from '../../../user/components/Profile';

interface HeaderProps {
  onSidebarOpen: () => void;
  sidebarWidth: number;
  isSidebarOpen: boolean;
}

export const Header: FC<HeaderProps> = ({
  onSidebarOpen,
  sidebarWidth,
  isSidebarOpen
}) => {
  const theme = useTheme();

  return (
    <AppBar
      elevation={0}
      sx={{
        backgroundColor: theme.palette.background.paper,
        color: theme.palette.text.primary,
        boxShadow: theme.shadows[3],
        width: {
          lg: isSidebarOpen ? `calc(100% - ${sidebarWidth}px)` : '100%'
        },
        marginLeft: {
          lg: isSidebarOpen ? `${sidebarWidth}px` : 0
        },
        zIndex: (theme) => theme.zIndex.drawer + 1,
        transition: theme.transitions.create(['width', 'margin'], {
          easing: theme.transitions.easing.sharp,
          duration: theme.transitions.duration.leavingScreen
        })
      }}
    >
      <Toolbar
        disableGutters
        sx={{
          minHeight: 64,
          left: 0,
          px: 2
        }}
      >
        {/* Menu Icon - visible only on mobile */}
        <IconButton
          onClick={onSidebarOpen}
          sx={{
            display: { xs: 'inline-flex', lg: 'none' }
          }}
        >
          <MenuIcon />
        </IconButton>

        {/* Title */}
        <Typography
          variant="h6"
          noWrap
          component="div"
          sx={{
            flexGrow: 1,
            display: 'flex',
            alignItems: 'center',
            ml: { xs: 2, md: 3 },
          }}
        >
          Pulse Administration
        </Typography>

        {/* Right side elements */}
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          {/* Theme toggle */}
          <ThemeToggle />
          
          {/* User Profile Menu */}
          <Profile />
        </Box>
      </Toolbar>
    </AppBar>
  );
};