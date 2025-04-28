import { createContext } from 'react';
import { GraphContextType } from '../types/graph-context-type';

/**
 * React context for Microsoft Graph API state and actions.
 * Provides Graph client, user profile, error/loading state, and Graph methods.
 * @see GraphContextType for context value shape
 */
export const GraphContext = createContext<GraphContextType | undefined>(undefined);
