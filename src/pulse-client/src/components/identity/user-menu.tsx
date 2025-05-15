import { Menu, MenuItem, ListItemIcon, ListItemText, Divider } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import DashboardIcon from '@mui/icons-material/Dashboard';
import LogoutIcon from '@mui/icons-material/Logout';

interface UserMenuProps {
  anchorEl: null | HTMLElement;
  open: boolean;
  onClose: () => void;
  onLogout: () => void;
}

export const UserMenu = ({ anchorEl, open, onClose, onLogout }: UserMenuProps) => (
  <Menu
    id="account-menu"
    anchorEl={anchorEl}
    open={open}
    onClose={onClose}
    onClick={onClose}
    PaperProps={{
      elevation: 0,
      sx: {
        overflow: 'visible',
        filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
        mt: 1.5,
        '& .MuiAvatar-root': {
          width: 32,
          height: 32,
          ml: -0.5,
          mr: 1,
        },
        '&::before': {
          content: '""',
          display: 'block',
          position: 'absolute',
          top: 0,
          right: 14,
          width: 10,
          height: 10,
          bgcolor: 'background.paper',
          transform: 'translateY(-50%) rotate(45deg)',
          zIndex: 0,
        },
      },
    }}
    transformOrigin={{ horizontal: 'right', vertical: 'top' }}
    anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
  >
    <MenuItem component={RouterLink} to="/backoffice">
      <ListItemIcon>
        <DashboardIcon fontSize="small" />
      </ListItemIcon>
      <ListItemText>Backoffice</ListItemText>
    </MenuItem>
    <Divider />
    <MenuItem onClick={onLogout}>
      <ListItemIcon>
        <LogoutIcon fontSize="small" />
      </ListItemIcon>
      <ListItemText>Logout</ListItemText>
    </MenuItem>
  </Menu>
);
