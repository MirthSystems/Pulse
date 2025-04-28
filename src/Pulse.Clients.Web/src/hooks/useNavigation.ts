import { useContext } from 'react';
import NavigationContext, { NavigationContextType } from '@/contexts/NavigationContext';

/**
 * Custom hook to access the navigation context.
 * Throws if used outside a NavigationProvider.
 * @returns NavigationContextType
 */
export const useNavigation = (): NavigationContextType => {
  const context = useContext(NavigationContext);
  if (context === undefined) {
    throw new Error('useNavigation must be used within a NavigationProvider');
  }
  return context;
};