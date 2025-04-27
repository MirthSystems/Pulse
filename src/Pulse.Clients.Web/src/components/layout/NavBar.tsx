import React from 'react';
import { Link } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Button from '@mui/material/Button';
import { useIsAuthenticated } from '@azure/msal-react';

const NavBar: React.FC = () => {
  const isAuthenticated = useIsAuthenticated();

  return (
    <AppBar position="static">
      <Toolbar>
        <div style={{ display: 'flex', gap: '10px' }}>
          <Button color="inherit" component={Link} to="/">
            Home
          </Button>
          {isAuthenticated && (
            <Button color="inherit" component={Link} to="/profile">
              Profile
            </Button>
          )}
        </div>
      </Toolbar>
    </AppBar>
  );
};

export default NavBar;