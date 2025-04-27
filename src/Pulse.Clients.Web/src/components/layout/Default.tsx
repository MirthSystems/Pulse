import React, { ReactNode } from 'react';
import Typography from "@mui/material/Typography";
import { useIsAuthenticated } from '@azure/msal-react';
import NavBar from './NavBar';
import SignInSignOutButton from '../auth/SignInSignOutButton';
import WelcomeName from '../auth/WelcomeName';
import { useMsal } from '@azure/msal-react';

interface DefaultLayoutProps {
  children: ReactNode;
}

const DefaultLayout: React.FC<DefaultLayoutProps> = ({ children }) => {
  const isAuthenticated = useIsAuthenticated();
  const { instance } = useMsal();
  
  const activeAccount = instance.getActiveAccount();
  const name = activeAccount ? activeAccount.name : '';

  return (
    <>
      <header>
        <NavBar />
        <div style={{ display: 'flex', justifyContent: 'flex-end', padding: '8px 16px' }}>
          {isAuthenticated && name && <WelcomeName name={name} />}
          <SignInSignOutButton />
        </div>
      </header>
      <main style={{ padding: '20px' }}>
        <Typography variant="h5" align="center">
          Welcome to the Microsoft Authentication Library For React Quickstart
        </Typography>
        <br />
        <br />
        {children}
      </main>
    </>
  );
};

export default DefaultLayout;