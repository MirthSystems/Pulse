import { useState, useEffect, useCallback } from "react";
import { PublicClientApplication, AccountInfo } from "@azure/msal-browser";
import { msalConfig, loginRequest } from "@/config/auth";

const msalInstance = new PublicClientApplication(msalConfig);

export const useAuth = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState<AccountInfo | null>(null);

  const login = useCallback(async () => {
    try {
      const loginResponse = await msalInstance.loginPopup(loginRequest);
      setUser(loginResponse.account);
      setIsAuthenticated(true);
    } catch (error) {
      console.error("Login failed", error);
    }
  }, []);

  const logout = useCallback(() => {
    msalInstance.logoutPopup();
    setUser(null);
    setIsAuthenticated(false);
  }, []);

  useEffect(() => {
    const checkAccount = () => {
      const currentAccounts = msalInstance.getAllAccounts();
      if (currentAccounts && currentAccounts.length > 0) {
        setUser(currentAccounts[0]);
        setIsAuthenticated(true);
      }
    };

    checkAccount();
  }, []);

  return {
    isAuthenticated,
    user,
    login,
    logout,
  };
};
