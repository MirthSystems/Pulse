import { Routes, Route } from 'react-router-dom';
import { useAuth0 } from '@auth0/auth0-react';
import { CircularProgress, Box } from '@mui/material';

import MainLayout from '@components/layout/MainLayout';
import LandingPage from '@/pages/LandingPage';
import SearchResultsPage from '@/pages/SearchResultsPage';
import BackofficePage from '@/pages/BackofficePage';
import VenueManagementPage from '@/pages/VenueManagementPage';
import VenueDetailsPage from '@/pages/VenueDetailsPage';
import VenueFormPage from '@/pages/VenueFormPage';
import SpecialFormPage from '@/pages/SpecialFormPage';
import NotFoundPage from '@/pages/NotFoundPage';
import ProtectedRoute from '@components/auth/ProtectedRoute';
import AuthStateManager from '@components/auth/AuthStateManager';

const App = () => {
  const { isLoading } = useAuth0();

  if (isLoading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <AuthStateManager>
      <Routes>
        <Route path="/" element={<MainLayout />}>
          {/* Public routes */}
          <Route index element={<LandingPage />} />
          <Route path="results" element={<SearchResultsPage />} />
          <Route path="venues/:id" element={<VenueDetailsPage />} />
          
          {/* Protected routes - Backoffice */}
          <Route 
            path="backoffice" 
            element={
              <ProtectedRoute>
                <BackofficePage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path="venues/new" 
            element={
              <ProtectedRoute>
                <VenueFormPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path="venues/:id/manage" 
            element={
              <ProtectedRoute>
                <VenueManagementPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path="venues/:id/edit" 
            element={
              <ProtectedRoute>
                <VenueFormPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path="venues/:id/specials/new" 
            element={
              <ProtectedRoute>
                <SpecialFormPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path="venues/:venueId/specials/:id/edit" 
            element={
              <ProtectedRoute>
                <SpecialFormPage />
              </ProtectedRoute>
            } 
          />
          
          {/* Catch all for 404 */}
          <Route path="*" element={<NotFoundPage />} />
        </Route>
      </Routes>
    </AuthStateManager>
  );
};

export default App;
