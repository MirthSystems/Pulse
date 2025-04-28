import { useContext } from 'react';
import QueryContext, { QueryContextType } from '../contexts/QueryContext';

/**
 * Custom hook to access the query context.
 * Throws if used outside a QueryProvider.
 * @returns QueryContextType
 */
export const useQueryContext = (): QueryContextType => {
  const context = useContext(QueryContext);
  if (context === undefined) {
    throw new Error('useQueryContext must be used within a QueryProvider');
  }
  return context;
};