import React from 'react';
import { Avatar, useTheme } from '@mui/material';

interface UserAvatarProps {
  displayName?: string;
  size?: number;
  color?: string;
}

/**
 * A reusable user avatar component that displays the first letter of the user's name
 */
const UserAvatar: React.FC<UserAvatarProps> = React.memo(({ 
  displayName = '', 
  size = 32,
  color
}) => {
  const theme = useTheme();
  // Get first letter of name for avatar, or use default
  const avatarLetter = displayName && displayName.length > 0 
    ? displayName.charAt(0).toUpperCase() 
    : 'U';

  return (
    <Avatar 
      sx={{ 
        width: size, 
        height: size, 
        bgcolor: color || theme.palette.secondary.main,
        fontSize: size * 0.5
      }}
    >
      {avatarLetter}
    </Avatar>
  );
});

// Add display name to fix the ESLint react/display-name warning
UserAvatar.displayName = 'UserAvatar';

export default UserAvatar;