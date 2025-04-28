import React, { useState, useEffect, ReactNode } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { NavigationHistoryEntry } from '../../types/navigation-history-entry';
import { BreadcrumbItem } from '../../types/breadcrumb-item';
import NavigationContext, { NavigationContextType } from './index';

// Maximum history entries to maintain
const MAX_HISTORY_LENGTH = 20;

// Storage key for persisting navigation history
const HISTORY_STORAGE_KEY = 'pulse-navigation-history';

// Map of route paths to breadcrumb labels
const routeLabels: Record<string, string> = {
  '/': 'Home',
  '/profile': 'Profile',
  '/dashboard': 'Dashboard',
  '/specials': 'Specials',
  // Add more route mappings as needed
};

// Helper to get page title for a given path
const getPageTitle = (path: string): string => {
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

interface NavigationProviderProps {
  children: ReactNode;
}

/**
 * Provider for navigation state and functionality.
 * Manages navigation history, breadcrumbs, and back navigation.
 */
export const NavigationContextProvider: React.FC<NavigationProviderProps> = ({ children }) => {
  const navigate = useNavigate();
  const location = useLocation();
  
  // Initialize history state from storage or empty array
  const [history, setHistory] = useState<NavigationHistoryEntry[]>(() => {
    try {
      const savedHistory = localStorage.getItem(HISTORY_STORAGE_KEY);
      return savedHistory ? JSON.parse(savedHistory) : [];
    } catch (error) {
      console.error('Error loading navigation history:', error);
      return [];
    }
  });

  const [previousPath, setPreviousPath] = useState<string | null>(null);

  // Update history when location changes
  useEffect(() => {
    setHistory(prevHistory => {
      // Don't record duplicate consecutive entries
      if (prevHistory.length > 0 && prevHistory[prevHistory.length - 1].path === location.pathname) {
        return prevHistory;
      }

      // Set the previous path
      if (prevHistory.length > 0) {
        setPreviousPath(prevHistory[prevHistory.length - 1].path);
      }

      // Create new history array with latest entry
      const newEntry: NavigationHistoryEntry = {
        path: location.pathname,
        title: getPageTitle(location.pathname),
        timestamp: Date.now(),
        state: location.state as Record<string, unknown> || {}
      };

      const updatedHistory = [...prevHistory, newEntry].slice(-MAX_HISTORY_LENGTH);
      
      // Save to localStorage
      try {
        localStorage.setItem(HISTORY_STORAGE_KEY, JSON.stringify(updatedHistory));
      } catch (error) {
        console.error('Error saving navigation history:', error);
      }
      
      return updatedHistory;
    });
  }, [location.pathname, location.state]);

  // Navigate back to previous page
  const goBack = () => {
    if (history.length > 1) {
      const prevEntry = history[history.length - 2];
      navigate(prevEntry.path, { 
        state: prevEntry.state,
        replace: true 
      });
      
      // Update history manually to remove current page
      const updatedHistory = history.slice(0, -1);
      setHistory(updatedHistory);
      
      // Update localStorage
      try {
        localStorage.setItem(HISTORY_STORAGE_KEY, JSON.stringify(updatedHistory));
      } catch (error) {
        console.error('Error saving navigation history:', error);
      }
    } else {
      // If no history, go to home
      navigate('/', { replace: true });
    }
  };

  // Enhanced navigation function
  const navigateTo = (path: string, options = {}) => {
    navigate(path, options);
  };

  // Generate breadcrumbs based on current path
  const getBreadcrumbs = (): BreadcrumbItem[] => {
    const pathSegments = location.pathname.split('/').filter(Boolean);
    const breadcrumbs: BreadcrumbItem[] = [
      { path: '/', label: 'Home', isActive: location.pathname === '/' }
    ];

    // Build up breadcrumb items based on path segments
    let currentPath = '';
    pathSegments.forEach((segment, index) => {
      currentPath += `/${segment}`;
      const isLast = index === pathSegments.length - 1;
      
      breadcrumbs.push({
        path: currentPath,
        label: getPageTitle(currentPath),
        isActive: isLast
      });
    });

    return breadcrumbs;
  };

  // Clear navigation history
  const clearHistory = () => {
    setHistory([{
      path: location.pathname,
      title: getPageTitle(location.pathname),
      timestamp: Date.now(),
      state: location.state as Record<string, unknown> || {}
    }]);
    localStorage.removeItem(HISTORY_STORAGE_KEY);
  };

  const value: NavigationContextType = {
    history,
    currentPath: location.pathname,
    previousPath,
    canGoBack: history.length > 1,
    goBack,
    navigateTo,
    getBreadcrumbs,
    clearHistory
  };

  return (
    <NavigationContext.Provider value={value}>
      {children}
    </NavigationContext.Provider>
  );
};