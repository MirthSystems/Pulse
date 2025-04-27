import React from 'react';
import { Routes, Route, useNavigate, Navigate } from 'react-router-dom';
import { MsalProvider, useIsAuthenticated } from '@azure/msal-react';
import { pca } from './configs/auth';

// Services
import { NavigationService } from './services/navigationService';

// Layouts
import DefaultLayout from './layouts/DefaultLayout';
import DashboardLayout from './layouts/DashboardLayout';

// Pages
import Home from './pages/Home';
import Profile from './pages/Profile';
import Dashboard from './pages/Dashboard';
import SpecialsList from './pages/SpecialsList';
import ErrorComponent from './components/ErrorComponent';

// Protected route component
interface ProtectedRouteProps {
  element: React.ReactNode;
  title?: string;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ element, title }) => {
  const isAuthenticated = useIsAuthenticated();
  
  if (!isAuthenticated) {
    return <Navigate to="/" replace />;
  }
  
  return (
    <DashboardLayout title={title}>
      {element}
    </DashboardLayout>
  );
};

const App: React.FC = () => {
  const navigate = useNavigate();
  const navigationClient = new NavigationService(navigate);
  pca.setNavigationClient(navigationClient);

  return (
    <MsalProvider instance={pca}>
      <Routes>
        {/* Public Routes with DefaultLayout */}
        <Route 
          path="/" 
          element={
            <DefaultLayout>
              <Home />
            </DefaultLayout>
          } 
        />
        
        <Route 
          path="/specials/list" 
          element={
            <DefaultLayout>
              <SpecialsList />
            </DefaultLayout>
          } 
        />
        
        {/* Protected Routes with Dashboard Layout */}
        <Route 
          path="/dashboard" 
          element={<ProtectedRoute element={<Dashboard />} title="Dashboard" />} 
        />
        
        <Route 
          path="/profile" 
          element={<ProtectedRoute element={<Profile />} title="Profile" />} 
        />
        
        <Route 
          path="/specials" 
          element={<ProtectedRoute element={<div>Specials Management Page</div>} title="Manage Specials" />} 
        />
        
        <Route 
          path="/venues" 
          element={<ProtectedRoute element={<div>Venues Management Page</div>} title="Manage Venues" />} 
        />
        
        <Route 
          path="/settings" 
          element={<ProtectedRoute element={<div>User Settings Page</div>} title="Settings" />} 
        />
        
        {/* 404 Error Route */}
        <Route 
          path="*" 
          element={
            <DefaultLayout>
              <ErrorComponent />
            </DefaultLayout>
          } 
        />
      </Routes>
    </MsalProvider>
  );
};

export default App;