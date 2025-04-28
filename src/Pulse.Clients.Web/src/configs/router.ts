import { RouteConfiguration } from '../types/route-configuration';
import { RouteIconType } from '../types/route-icon-type';
import React from 'react';

// Lazy load pages for code-splitting
const Home = React.lazy(() => import('../pages/Home'));
const SpecialsList = React.lazy(() => import('../pages/Specials'));
const Main = React.lazy(() => import('../pages/dashboard/Main'));
const Profile = React.lazy(() => import('../pages/dashboard/Profile'));

// Note: getIconType is currently unused but preserved for future dynamic icon generation
// Prefix with underscore to comply with the unused variable naming convention
const _getIconType = (name: string): RouteIconType => {
  const iconMap: Record<string, RouteIconType> = {
    'home': 'home',
    'dashboard': 'dashboard',
    'profile': 'profile',
    'specials': 'specials',
    'settings': 'settings',
    'help': 'help'
  };
  return iconMap[name] || 'none';
};

/**
 * Application route configuration
 * Defines all routes and their metadata
 */
export const routerConfiguration: RouteConfiguration[] = [
  {
    path: '/',
    layout: 'default',
    component: Home,
    title: 'Home',
    icon: 'home',
    showInNav: true,
    priority: 10,
    meta: {
      description: 'Welcome to Pulse',
      breadcrumbLabel: 'Home',
    }
  },
  {
    path: '/specials',
    layout: 'default',
    title: 'Specials',
    icon: 'specials',
    showInNav: true,
    priority: 20,
    children: [
      {
        path: '/specials/list',
        layout: 'default',
        component: SpecialsList,
        title: 'Browse Specials',
        showInNav: true,
        meta: {
          description: 'Browse available specials',
          breadcrumbLabel: 'Browse',
        }
      }
    ]
  },
  {
    path: '/dashboard',
    layout: 'dashboard',
    title: 'Dashboard',
    icon: 'dashboard',
    protected: true,
    component: Main,
    showInNav: true,
    priority: 30,
    meta: {
      description: 'User dashboard',
      breadcrumbLabel: 'Dashboard',
    }
  },
  {
    path: '/profile',
    layout: 'dashboard',
    title: 'Profile',
    icon: 'profile',
    protected: true,
    component: Profile,
    showInNav: true,
    priority: 40,
    meta: {
      description: 'User profile information',
      breadcrumbLabel: 'Profile',
    }
  }
];

/**
 * Gets a flattened list of all routes, including children
 */
export const getAllRoutes = (): RouteConfiguration[] => {
  const flatten = (routes: RouteConfiguration[]): RouteConfiguration[] => {
    return routes.reduce<RouteConfiguration[]>((acc, route) => {
      if (route.children) {
        return [...acc, route, ...flatten(route.children)];
      }
      return [...acc, route];
    }, []);
  };
  
  return flatten(routerConfiguration);
};

/**
 * Finds a route by path
 */
export const findRouteByPath = (path: string): RouteConfiguration | undefined => {
  return getAllRoutes().find(route => route.path === path);
};

/**
 * Gets all routes that should be shown in navigation
 * Sorted by priority
 */
export const getNavigationRoutes = (): RouteConfiguration[] => {
  return getAllRoutes()
    .filter(route => route.showInNav)
    .sort((a, b) => (a.priority || 0) - (b.priority || 0));
};
