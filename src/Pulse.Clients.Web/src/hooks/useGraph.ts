import { useContext } from 'react';
import { GraphContextType } from '../types/graph-context-type';
import { GraphContext } from '../contexts/graph-context';

export const useGraph = (): GraphContextType => {
  const context = useContext(GraphContext);
  if (context === undefined) {
    throw new Error('useGraph must be used within a GraphProvider');
  }
  return context;
};