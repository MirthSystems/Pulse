import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Button } from "@heroui/react";

interface LoginButtonProps {
  className?: string;
}

export const LoginButton: React.FC<LoginButtonProps> = ({ className = "" }) => {
  const { loginWithRedirect, isLoading } = useAuth0();

  const handleLogin = () => {
    loginWithRedirect();
  };

  return (
    <Button
      color="primary"
      isLoading={isLoading}
      className={className}
      onPress={handleLogin}
    >
      Log In
    </Button>
  );
};

export default LoginButton;
