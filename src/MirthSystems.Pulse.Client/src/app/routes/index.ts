import { RouteObject, Navigate } from 'react-router-dom';
import { lazy, Suspense } from 'react';
import { DefaultLayout } from '../../features/ui/layouts/DefaultLayout';
import { DashboardLayout } from '../../features/ui/layouts/DashboardLayout';
import NotFoundView from '../views/NotFoundView';
import { LoadingPage } from '../../features/ui/components';

// Lazy loaded route components
const HomePage = lazy(() => import('../pages/home/HomePage')); // Landing page with specials search form
const SearchResultsPage = lazy(() => import('../pages/search/SearchResultsPage')); // Search results page

// Dashboard pages
const AccountPage = lazy(() => import('../pages/dashboard/AccountPage')); // Profile details from auth0 claims
const AdministrationPage = lazy(() => import('../pages/dashboard/AdministrationPage')); // Admin only page

/**
 * Creates all application routes with authentication checks where needed
 */
export const createRoutes = (authHelpers: {
  isAuthenticated: boolean;
  isAdmin: () => boolean;
  isVenueManager: () => boolean;
}): RouteObject[] => {
  const { isAuthenticated, isAdmin, isVenueManager } = authHelpers;
  
  return [
    // Public routes with DefaultLayout
    {
      element: <DefaultLayout />,
      children: [
        { 
          path: '/', 
          element: <Suspense fallback={<LoadingPage />}><HomePage /></Suspense>
        },
        { 
          path: '/search', 
          element: <Suspense fallback={<LoadingPage />}><SearchResultsPage /></Suspense> 
        }
      ]
    },
    
    // Protected dashboard routes
    {
      path: '/dashboard',
      element: isAuthenticated ? <DashboardLayout /> : <Navigate to="/" />,
      children: [
        {
          index: true,
          element: <Suspense fallback={<LoadingPage />}><AccountPage /></Suspense>
        },
        {
          path: 'account',
          element: <Suspense fallback={<LoadingPage />}><AccountPage /></Suspense>
        },
        // Admin/venue manager only route
        {
          path: 'administration',
          element: (isAdmin() || isVenueManager()) 
            ? <Suspense fallback={<LoadingPage />}><AdministrationPage /></Suspense>
            : <Navigate to="/dashboard" />
        }
      ]
    },
    
    // Not Found Route (catch-all)
    {
      path: '*',
      element: <NotFoundView />
    }
  ];
};