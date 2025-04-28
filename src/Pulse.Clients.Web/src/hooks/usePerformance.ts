import { useContext } from 'react';
import PerformanceContext, { PerformanceContextType } from '../contexts/PerformanceContext';

/**
 * Custom hook to access the performance context.
 * Throws if used outside a PerformanceProvider.
 * @returns PerformanceContextType
 */
export const usePerformance = (): PerformanceContextType => {
  const context = useContext(PerformanceContext);
  if (context === undefined) {
    throw new Error('usePerformance must be used within a PerformanceProvider');
  }
  return context;
};