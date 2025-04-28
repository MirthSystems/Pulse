import { createContext } from 'react';
import { BreadcrumbItem } from '../../types/breadcrumb-item';
import { NavigationHistoryEntry } from '../../types/navigation-history-entry';

/**
 * Interface for navigation context state.
 * Provides navigation history tracking, back navigation, and breadcrumb generation.
 */
export interface NavigationContextType {
  /** Array of navigation history entries */
  history: NavigationHistoryEntry[];
  
  /** The current path of the application */
  currentPath: string;
  
  /** The previously visited path, or null if there is none */
  previousPath: string | null;
  
  /** Whether there is history to navigate back to */
  canGoBack: boolean;
  
  /** Navigate back to previous page */
  goBack: () => void;
  
  /** Navigate to a specific path with options */
  navigateTo: (path: string, options?: { state?: Record<string, unknown>, replace?: boolean }) => void;
  
  /** Get breadcrumb items for current location */
  getBreadcrumbs: () => BreadcrumbItem[];
  
  /** Clear navigation history */
  clearHistory: () => void;
}

/**
 * React context for navigation state and actions.
 * Provides navigation history tracking, breadcrumb generation, and navigation utilities.
 */
const NavigationContext = createContext<NavigationContextType | undefined>(undefined);

export default NavigationContext;