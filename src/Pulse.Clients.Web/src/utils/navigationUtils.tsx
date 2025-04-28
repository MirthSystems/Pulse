/**
 * Navigation history management utility for enhanced user experience
 * This utility provides improved navigation history tracking, breadcrumb generation,
 * and navigation state persistence
 */

import { useNavigation } from '../hooks/useNavigation';

// Maximum history entries to maintain
export const MAX_HISTORY_LENGTH = 20;

// Storage key for persisting navigation history
export const HISTORY_STORAGE_KEY = 'pulse-navigation-history';

// Map of route paths to breadcrumb labels
export const routeLabels: Record<string, string> = {
  '/': 'Home',
  '/profile': 'Profile',
  '/dashboard': 'Dashboard',
  '/specials': 'Specials',
  // Add more route mappings as needed
};

// Helper to get page title for a given path
export const getPageTitle = (path: string): string => {
  const exactMatch = routeLabels[path];
  if (exactMatch) return exactMatch;

  // Handle dynamic routes
  const pathSegments = path.split('/').filter(Boolean);
  if (pathSegments.length > 0) {
    // Try to match parent routes
    const parentPath = '/' + pathSegments[0];
    if (routeLabels[parentPath]) {
      return routeLabels[parentPath];
    }
  }

  // Default to capitalized last path segment
  const lastSegment = pathSegments[pathSegments.length - 1];
  return lastSegment 
    ? lastSegment.charAt(0).toUpperCase() + lastSegment.slice(1)
    : 'Unknown Page';
};

// Export the useNavigation hook for easy access
export { useNavigation };