import React from "react";
import { Button } from "@heroui/button";
import { useAuth } from "@/hooks/useAuth";

const LoginButton: React.FC = () => {
  const { isAuthenticated, login, logout } = useAuth();

  return (
    <div>
      {isAuthenticated ? (
        <Button onClick={logout} variant="flat">
          Logout
        </Button>
      ) : (
        <Button onClick={login} variant="flat">
          Login / Sign Up
        </Button>
      )}
    </div>
  );
};

export default LoginButton;
