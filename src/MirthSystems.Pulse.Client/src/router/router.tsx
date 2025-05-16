import { createBrowserRouter, Navigate, RouterProvider } from 'react-router-dom';
import { ProtectedRoute as AuthGuard } from '../components/identity';
import { Layout } from '../components/ui/layout'; // Corrected Layout import path again
import { BackofficePage, CallbackPage, HomePage, NotFoundPage, SearchResultsPage, VenuePage } from '../pages';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Layout />,
    errorElement: <NotFoundPage />,
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: 'search-results',
        element: <SearchResultsPage />,
      },
      {
        path: 'backoffice',
        element: (
          <AuthGuard>
            <BackofficePage />
          </AuthGuard>
        ),
      },
      {
        path: 'backoffice/venues/:id',
        element: (
          <AuthGuard>
            <VenuePage />
          </AuthGuard>
        ),
      },
    ],
  },
  {
    path: '/callback',
    element: <CallbackPage />,
  },
  {
    path: '*',
    element: <Navigate to="/" replace />
  }
]);

export const AppRouter = () => {
  return <RouterProvider router={router} />;
};
