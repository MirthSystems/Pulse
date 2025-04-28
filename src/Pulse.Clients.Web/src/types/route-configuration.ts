import { LayoutType } from './layout-type';
import { RouteIconType } from './route-icon-type';
import React from 'react';

/**
 * Configuration for application routes
 */
export type RouteConfiguration = {
  /** URL path for the route */
  path: string;
  
  /** Layout to use for this route */
  layout: LayoutType;
  
  /** Page title to display */
  title?: string;
  
  /** Whether route requires authentication */
  protected?: boolean;
  
  /** Component to render for this route */
  component?: React.ComponentType<Record<string, unknown>>;
  
  /** Icon to display in navigation */
  icon?: RouteIconType;
  
  /** Whether to show in navigation menu */
  showInNav?: boolean;
  
  /** Role-based access control */
  requiredRoles?: string[];
  
  /** Route priority for ordering in menus */
  priority?: number;
  
  /** Child routes */
  children?: RouteConfiguration[];
  
  /** Meta information for the route */
  meta?: {
    description?: string;
    breadcrumbLabel?: string;
    hideHeader?: boolean;
    hideFooter?: boolean;
    fullWidth?: boolean;
    [key: string]: unknown;
  };
}