import React from 'react';
import { Routes, Route, useNavigate, Navigate } from 'react-router-dom';
import { MsalProvider, useIsAuthenticated } from '@azure/msal-react';
import { pca } from './configs/auth';

// Services
import { NavigationService } from './services/navigationService';

// Contexts providers
import { GraphProvider } from './contexts/providers/GraphProvider';
import { AuthProvider } from './contexts/providers/AuthProvider';
import { ThemeProvider } from './contexts/providers/ThemeProvider';

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

// Route configuration for better maintainability
interface RouteConfig {
  path: string;
  element: React.ReactNode;
  layout: React.ComponentType<any>;
  title?: string;
  protected?: boolean;
}

const routes: RouteConfig[] = [
  // Public routes
  {
    path: "/",
    element: <Home />,
    layout: DefaultLayout
  },
  {
    path: "/specials/list",
    element: <SpecialsList />,
    layout: DefaultLayout
  },
  
  // Protected routes
  {
    path: "/dashboard",
    element: <Dashboard />,
    layout: ProtectedRoute,
    title: "Dashboard",
    protected: true
  },
  {
    path: "/profile",
    element: <Profile />,
    layout: ProtectedRoute,
    title: "Profile",
    protected: true
  },
  {
    path: "/specials",
    element: <div>Specials Management Page</div>,
    layout: ProtectedRoute,
    title: "Manage Specials",
    protected: true
  },
  {
    path: "/venues",
    element: <div>Venues Management Page</div>,
    layout: ProtectedRoute,
    title: "Manage Venues",
    protected: true
  },
  {
    path: "/settings",
    element: <div>User Settings Page</div>,
    layout: ProtectedRoute,
    title: "Settings",
    protected: true
  },
  
  // 404 route
  {
    path: "*",
    element: <ErrorComponent title="Page Not Found" message="The page you're looking for doesn't exist." />,
    layout: DefaultLayout
  }
];

const App: React.FC = () => {
  const navigate = useNavigate();
  const navigationClient = new NavigationService(navigate);
  pca.setNavigationClient(navigationClient);

  return (
    <MsalProvider instance={pca}>
      <ThemeProvider>
        <AuthProvider>
          <GraphProvider>
            <Routes>
              {routes.map((route, index) => {
                const RouteLayout = route.layout;
                
                // For protected routes
                if (route.protected) {
                  return (
                    <Route 
                      key={index}
                      path={route.path} 
                      element={
                        <RouteLayout 
                          element={route.element}
                          title={route.title}
                        />
                      } 
                    />
                  );
                }
                
                // For public routes
                return (
                  <Route 
                    key={index}
                    path={route.path} 
                    element={
                      <RouteLayout>
                        {route.element}
                      </RouteLayout>
                    } 
                  />
                );
              })}
            </Routes>
          </GraphProvider>
        </AuthProvider>
      </ThemeProvider>
    </MsalProvider>
  );
};

export default App;