import { createContext } from 'react';

/**
 * Type for query context values.
 * Provides state and methods for managing query parameters.
 */
export type QueryContextType = {
  /** Get the current value of a query parameter */
  getParam: (key: string) => string | null;
  
  /** Set a query parameter value */
  setParam: (key: string, value: string | null) => void;
  
  /** Set multiple query parameters at once */
  setParams: (params: Record<string, string | null>) => void;
  
  /** Get all query parameters as an object */
  getAllParams: () => Record<string, string>;
  
  /** Clear all query parameters */
  clearParams: () => void;
  
  /** Get query string for current parameters */
  getQueryString: () => string;
  
  /** Check if a specific parameter exists */
  hasParam: (key: string) => boolean;
};

/**
 * React context for query parameter state and actions.
 * Provides utilities for working with URL query parameters.
 */
const QueryContext = createContext<QueryContextType | undefined>(undefined);

export default QueryContext;