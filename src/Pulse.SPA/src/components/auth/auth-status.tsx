import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "./login-button";
import UserMenu from "./user-menu";

export const AuthStatus: React.FC = () => {
  const { isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <div className="w-8 h-8 rounded-full bg-default-200 animate-pulse" />;
  }

  return isAuthenticated ? <UserMenu /> : <LoginButton />;
};

export default AuthStatus;
