import { Avatar } from '@mui/material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useAuthStore } from '../../store';

export const UserAvatar = () => {
  const { user } = useAuthStore();
  
  if (user?.picture) {
    return (
      <Avatar 
        src={user.picture} 
        alt={user.name || 'User'} 
        sx={{ width: 32, height: 32 }}
      />
    );
  }
  
  return <AccountCircleIcon />;
};
