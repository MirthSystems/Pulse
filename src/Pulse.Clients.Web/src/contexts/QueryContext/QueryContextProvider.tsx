import React, { useCallback, useState, useEffect, ReactNode } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import QueryContext, { QueryContextType } from './index';

interface QueryProviderProps {
  children: ReactNode;
}

/**
 * Provider component for query parameter state and actions.
 * Handles synchronization between URL query parameters and React state.
 */
export const QueryContextProvider: React.FC<QueryProviderProps> = ({ children }) => {
  const location = useLocation();
  const navigate = useNavigate();
  const [queryParams, setQueryParams] = useState<URLSearchParams>(
    new URLSearchParams(location.search)
  );

  // Sync with URL when location changes
  useEffect(() => {
    setQueryParams(new URLSearchParams(location.search));
  }, [location.search]);

  // Get a specific query parameter
  const getParam = useCallback((key: string): string | null => {
    return queryParams.get(key);
  }, [queryParams]);

  // Set a query parameter and update URL
  const setParam = useCallback((key: string, value: string | null): void => {
    const newParams = new URLSearchParams(queryParams.toString());
    
    if (value === null || value === '') {
      newParams.delete(key);
    } else {
      newParams.set(key, value);
    }
    
    // Update the URL but preserve the current path
    navigate({
      pathname: location.pathname,
      search: newParams.toString()
    }, { replace: true });
  }, [queryParams, navigate, location.pathname]);

  // Set multiple parameters at once
  const setParams = useCallback((params: Record<string, string | null>): void => {
    const newParams = new URLSearchParams(queryParams.toString());
    
    Object.entries(params).forEach(([key, value]) => {
      if (value === null || value === '') {
        newParams.delete(key);
      } else {
        newParams.set(key, value);
      }
    });
    
    navigate({
      pathname: location.pathname,
      search: newParams.toString()
    }, { replace: true });
  }, [queryParams, navigate, location.pathname]);

  // Get all query parameters as an object
  const getAllParams = useCallback((): Record<string, string> => {
    const params: Record<string, string> = {};
    queryParams.forEach((value, key) => {
      params[key] = value;
    });
    return params;
  }, [queryParams]);

  // Clear all query parameters
  const clearParams = useCallback((): void => {
    navigate({
      pathname: location.pathname,
      search: ''
    }, { replace: true });
  }, [navigate, location.pathname]);

  // Check if a parameter exists
  const hasParam = useCallback((key: string): boolean => {
    return queryParams.has(key);
  }, [queryParams]);

  // Get the query string representation
  const getQueryString = useCallback((): string => {
    return queryParams.toString();
  }, [queryParams]);

  const contextValue: QueryContextType = {
    getParam,
    setParam,
    setParams,
    getAllParams,
    clearParams,
    getQueryString,
    hasParam,
  };

  return (
    <QueryContext.Provider value={contextValue}>
      {children}
    </QueryContext.Provider>
  );
};