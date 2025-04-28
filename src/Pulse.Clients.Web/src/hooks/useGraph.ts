import { useContext } from 'react';
import GraphContext, { GraphContextType } from '@/contexts/GraphContext';

/**
 * Custom hook to access the Microsoft Graph context.
 * Throws if used outside a GraphProvider.
 * @returns GraphContextType
 */
export const useGraph = (): GraphContextType => {
  const context = useContext(GraphContext);
  if (context === undefined) {
    throw new Error('useGraph must be used within a GraphProvider');
  }
  return context;
};