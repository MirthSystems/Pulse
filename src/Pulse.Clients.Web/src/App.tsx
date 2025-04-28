import React, { Suspense, lazy } from 'react';
import { Routes, Route, useNavigate } from 'react-router-dom';
import { MsalProvider } from '@azure/msal-react';
import { pca } from './configs/auth';

// Services
import { NavigationService } from './services/navigationService';

// Import router configuration from configs
import { routerConfiguration } from './configs/router';

// Import context providers
import { AuthContextProvider } from './contexts/AuthContext/AuthContextProvider';
import { NavigationContextProvider } from './contexts/NavigationContext/NavigationContextProvider';
import { ThemeContextProvider } from './contexts/ThemeContext/ThemeContextProvider';
import { GraphContextProvider } from './contexts/GraphContext/GraphContextProvider';
import { PerformanceContextProvider } from './contexts/PerformanceContext/PerformanceContextProvider';
import { QueryContextProvider } from './contexts/QueryContext/QueryContextProvider';

// Lazy load components for better code splitting
const ProtectedRoute = lazy(() => import('./components/routing/ProtectedRoute'));
const LayoutRoute = lazy(() => import('./components/routing/LayoutRoute'));
const ErrorBoundary = lazy(() => import('./components/ErrorBoundary'));

// Fallback loading component (needs to be eagerly loaded)
const AppLoading = () => (
  <div style={{ 
    display: 'flex', 
    justifyContent: 'center', 
    alignItems: 'center', 
    height: '100vh',
    flexDirection: 'column',
    gap: '1rem'
  }}>
    <div>Loading application...</div>
  </div>
);

/**
 * Main application component that sets up providers and routing
 */
const App: React.FC = () => {
  const navigate = useNavigate();
  const navigationClient = new NavigationService(navigate);
  pca.setNavigationClient(navigationClient);

  return (
    <MsalProvider instance={pca}>
      <ThemeContextProvider>
        <AuthContextProvider>
          <GraphContextProvider>
            <PerformanceContextProvider>
              <QueryContextProvider>
                <NavigationContextProvider>
                  <ErrorBoundary>
                    <Suspense fallback={<AppLoading />}>
                      <Routes>
                        {routerConfiguration.map((route, index) => {
                          const Content = route.component ? <route.component /> : <div>Page Not Found</div>;
                          let element = (
                            <LayoutRoute route={route}>{Content}</LayoutRoute>
                          );
                          if (route.protected) {
                            element = <ProtectedRoute route={route}>{element}</ProtectedRoute>;
                          }
                          return (
                            <Route key={index} path={route.path} element={element} />
                          );
                        })}
                        {/* 404 fallback */}
                        <Route path="*" element={<LayoutRoute route={{ layout: 'default', path: '*' }}>
                          <div>Page Not Found</div>
                        </LayoutRoute>} />
                      </Routes>
                    </Suspense>
                  </ErrorBoundary>
                </NavigationContextProvider>
              </QueryContextProvider>
            </PerformanceContextProvider>
          </GraphContextProvider>
        </AuthContextProvider>
      </ThemeContextProvider>
    </MsalProvider>
  );
};

export default App;