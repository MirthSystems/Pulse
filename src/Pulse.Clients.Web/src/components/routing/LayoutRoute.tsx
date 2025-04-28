import React, { useEffect } from 'react';
import DefaultLayout from '../../layout/default';
import DashboardLayout from '../../layout/dashboard';
import { RouteConfiguration } from '../../types/route-configuration';

interface LayoutRouteProps {
  route: RouteConfiguration;
  children: React.ReactNode;
}

/**
 * Wraps content in the correct layout based on route configuration.
 * Also handles document title updates and other metadata.
 */
const LayoutRoute: React.FC<LayoutRouteProps> = ({ route, children }) => {
  // Set document title based on route configuration
  useEffect(() => {
    if (route.title) {
      document.title = `Pulse | ${route.title}`;
    } else {
      document.title = 'Pulse';
    }
    
    // Set meta description if available
    if (route.meta?.description) {
      const metaDescription = document.querySelector('meta[name="description"]');
      if (metaDescription) {
        metaDescription.setAttribute('content', route.meta.description);
      }
    }
  }, [route]);

  // Use the appropriate layout
  if (route.layout === 'dashboard') {
    return (
      <DashboardLayout 
        title={route.title}
        hideHeader={route.meta?.hideHeader}
        hideFooter={route.meta?.hideFooter}
        fullWidth={route.meta?.fullWidth}
      >
        {children}
      </DashboardLayout>
    );
  }
  
  return (
    <DefaultLayout
      hideHeader={route.meta?.hideHeader}
      hideFooter={route.meta?.hideFooter}
      fullWidth={route.meta?.fullWidth}
    >
      {children}
    </DefaultLayout>
  );
};

export default LayoutRoute;
