import React from 'react';
import { Navigate } from 'react-router-dom';
import { useIsAuthenticated } from '@azure/msal-react';
import { useAuth } from '../../hooks/useAuth';
import { RouteConfiguration } from '../../types/route-configuration';
import Loading from '../Loading';
import ErrorComponent from '../ErrorComponent';

interface ProtectedRouteProps {
  children: React.ReactNode;
  redirectTo?: string;
  requiredRoles?: string[];
  route?: RouteConfiguration;
}

/**
 * Protects a route by requiring authentication and optional role-based permissions.
 * Redirects to `redirectTo` if not authenticated or not authorized.
 */
const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ 
  children, 
  redirectTo = '/', 
  requiredRoles,
  route
}) => {
  const isAuthenticated = useIsAuthenticated();
  const { userRoles, isLoadingRoles, error } = useAuth();
  
  // Get roles from route if provided
  const effectiveRequiredRoles = requiredRoles || route?.requiredRoles;
  
  // If not authenticated, redirect to login
  if (!isAuthenticated) {
    return <Navigate to={redirectTo} replace />;
  }
  
  // Show loading while checking roles
  if (effectiveRequiredRoles && isLoadingRoles) {
    return <Loading message="Checking permissions..." />;
  }
  
  // Show error if roles couldn't be loaded
  if (effectiveRequiredRoles && error) {
    return (
      <ErrorComponent 
        title="Permission Error"
        message="There was a problem checking your permissions. Please try again later."
        showRetry={true}
      />
    );
  }
  
  // Check if user has required roles
  if (effectiveRequiredRoles && effectiveRequiredRoles.length > 0) {
    const hasRequiredRole = userRoles.some((role: string) => 
      effectiveRequiredRoles.includes(role)
    );
    
    if (!hasRequiredRole) {
      return (
        <ErrorComponent 
          title="Access Denied"
          message="You don't have permission to access this page."
          showHome={true}
          showRetry={false}
        />
      );
    }
  }
  
  // User is authenticated and authorized
  return <>{children}</>;
};

export default ProtectedRoute;
