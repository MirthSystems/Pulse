import { createContext } from 'react';
import { GraphContextType } from '../types/graph-context-type';

// Create the context with a default value
export const GraphContext = createContext<GraphContextType | undefined>(undefined);
