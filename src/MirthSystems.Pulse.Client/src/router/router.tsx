import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { Layout } from '../components';
import { HomePage, BackofficePage, VenuePage, NotFoundPage, CallbackPage } from '../pages';
import { ProtectedRoute } from '../components/identity/protected-route';

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
        path: 'backoffice',
        element: (
          <ProtectedRoute>
            <BackofficePage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'backoffice/venues/:id',
        element: (
          <ProtectedRoute>
            <VenuePage />
          </ProtectedRoute>
        ),
      },
    ],
  },
  {
    path: '/callback',
    element: <CallbackPage />,
  },
]);

export const AppRouter = () => {
  return <RouterProvider router={router} />;
};
