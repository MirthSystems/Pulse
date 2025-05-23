import React from "react";
import { User } from "@auth0/auth0-react";
import { Avatar } from "@heroui/react";

interface UserAvatarProps {
  user: User;
  size?: "sm" | "md" | "lg";
  className?: string;
}

export const UserAvatar: React.FC<UserAvatarProps> = ({ 
  user, 
  size = "md",
  className = "",
}) => {
  const getInitials = () => {
    if (user.name) {
      return user.name
        .split(' ')
        .map(name => name[0])
        .join('')
        .toUpperCase()
        .substring(0, 2);
    }
    return user.email ? user.email[0].toUpperCase() : "U";
  };

  return (
    <Avatar
      name={user.name || user.email || "User"}
      src={user.picture}
      fallback={getInitials()}
      size={size}
      className={className}
    />
  );
};

export default UserAvatar;
