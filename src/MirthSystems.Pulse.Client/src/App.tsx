import { Routes, Route } from 'react-router-dom';
import { useAuth0 } from '@auth0/auth0-react';
import { CircularProgress, Box } from '@mui/material';

import MainLayout from '@components/layout/MainLayout';
import LandingPage from '@components/pages/LandingPage';
import SearchResultsPage from '@components/pages/SearchResultsPage';
import BackofficePage from '@components/pages/BackofficePage';
import VenueManagementPage from '@components/pages/VenueManagementPage';
import VenueDetailsPage from '@components/pages/VenueDetailsPage';
import VenueFormPage from '@components/pages/VenueFormPage';
import SpecialFormPage from '@components/pages/SpecialFormPage';
import NotFoundPage from '@components/pages/NotFoundPage';
import ProtectedRoute from '@components/auth/ProtectedRoute';

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
    <Routes>
      <Route path="/" element={<MainLayout />}>
        {/* Public routes */}
        <Route index element={<LandingPage />} />
        <Route path="results" element={<SearchResultsPage />} />
        <Route path="venue/:id" element={<VenueDetailsPage />} />
        
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
          path="venues/:id" 
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
  );
};

export default App;
