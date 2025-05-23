import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Dropdown, DropdownTrigger, DropdownMenu, DropdownItem, User } from "@heroui/react";
import { useNavigate, useLocation } from "react-router";
import UserAvatar from "./user-avatar";

export const UserMenu: React.FC = () => {
  const { user, logout } = useAuth0();
  const navigate = useNavigate();
  const location = useLocation();

  if (!user) return null;

  const isInBackoffice = location.pathname.startsWith('/backoffice');

  const handleLogout = () => {
    logout({ 
      logoutParams: { 
        returnTo: window.location.origin 
      }
    });
  };

  const handleNavigation = () => {
    if (isInBackoffice) {
      navigate("/");
    } else {
      navigate("/backoffice");
    }
  };

  return (
    <Dropdown placement="bottom-end">
      <DropdownTrigger>
        <div className="cursor-pointer">
          <UserAvatar user={user} />
        </div>
      </DropdownTrigger>
      <DropdownMenu aria-label="User menu actions" variant="flat">
        <DropdownItem key="profile" className="h-14 gap-2" textValue="Profile">
          <User
            name={user.name}
            description={user.email}
            avatarProps={{ src: user.picture }}
          />
        </DropdownItem>        
        <DropdownItem key="navigation" onPress={handleNavigation} textValue={isInBackoffice ? "Main Site" : "Backoffice"}>
          {isInBackoffice ? "Main Site" : "Backoffice"}
        </DropdownItem>
        <DropdownItem key="logout" color="danger" onPress={handleLogout}>
          Log Out
        </DropdownItem>
      </DropdownMenu>
    </Dropdown>
  );
};

export default UserMenu;
