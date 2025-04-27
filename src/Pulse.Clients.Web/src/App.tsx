import React from 'react';
import { Routes, Route, useNavigate } from 'react-router-dom';
import { MsalProvider } from '@azure/msal-react';
import { pca } from './configs/auth';
import DefaultLayout from './components/layout/Default';
import Home from './pages/Home';
import Profile from './pages/Profile';
import { CustomNavigationClient } from './utils/NavigationClient';

const App: React.FC = () => {
  const navigate = useNavigate();
  const navigationClient = new CustomNavigationClient(navigate);
  pca.setNavigationClient(navigationClient);

  return (
    <MsalProvider instance={pca}>
      <DefaultLayout>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/profile" element={<Profile />} />
        </Routes>
      </DefaultLayout>
    </MsalProvider>
  );
};

export default App;