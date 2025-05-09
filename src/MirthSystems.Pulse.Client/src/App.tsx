import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import { useAuth } from './features/user/hooks';
import { createRoutes } from './app/routes';
import { DateTimeProvider } from './app/providers';

function App() {
  const { isAuthenticated, isAdmin, isVenueManager } = useAuth();
  
  // Create routes with authentication helpers
  const routes = createRoutes({
    isAuthenticated,
    isAdmin,
    isVenueManager
  });
  
  // Create router with our routes
  const router = createBrowserRouter(routes);

  return (
    <DateTimeProvider>
      <RouterProvider router={router} />
    </DateTimeProvider>
  );
}

export default App;
