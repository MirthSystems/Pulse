import React, { useEffect, useMemo, useCallback, ReactNode } from 'react';
import { 
  initPerformanceMonitor, 
  markStart, 
  markEnd, 
  recordMetric, 
  getMetrics, 
  clearMetrics 
} from '../../utils/performanceMonitor';
import { PerformanceConfiguration } from '../../types/performance-configuration';
import PerformanceContext, { PerformanceContextType, PerformanceMetadata } from './index';

interface PerformanceProviderProps {
  children: ReactNode;
  configuration?: PerformanceConfiguration;
}

/**
 * Provider component for performance monitoring tools
 */
export const PerformanceContextProvider: React.FC<PerformanceProviderProps> = ({ 
  children,
  configuration 
}) => {
  // Initialize the performance monitor on mount
  useEffect(() => {
    initPerformanceMonitor(configuration);
  }, [configuration]);

  // Helper functions for common performance metrics
  const recordApiCall = useCallback((apiName: string, duration: number, metadata?: PerformanceMetadata) => {
    recordMetric('api-call', apiName, duration, metadata);
  }, []);

  const recordInteraction = useCallback((interactionName: string, duration: number, metadata?: PerformanceMetadata) => {
    recordMetric('interaction', interactionName, duration, metadata);
  }, []);

  const recordNavigation = useCallback((routeName: string, duration: number, metadata?: PerformanceMetadata) => {
    recordMetric('navigation', routeName, duration, metadata);
  }, []);

  // Helper for measuring async operations (like API calls)
  const measureAsyncOperation = useCallback(async <T,>(
    operationName: string, 
    operation: () => Promise<T>,
    eventType: 'api-call' | 'navigation' | 'resource-load' = 'api-call'
  ): Promise<T> => {
    markStart(operationName);
    try {
      const result = await operation();
      markEnd(operationName, eventType);
      return result;
    } catch (error) {
      markEnd(operationName, 'error', { error: String(error) });
      throw error;
    }
  }, []);

  // Memoize context value to prevent unnecessary renders
  const contextValue = useMemo<PerformanceContextType>(() => ({
    markStart,
    markEnd,
    recordApiCall,
    recordInteraction,
    recordNavigation,
    getMetrics,
    clearMetrics,
    measureAsyncOperation,
  }), [recordApiCall, recordInteraction, recordNavigation, measureAsyncOperation]);

  return (
    <PerformanceContext.Provider value={contextValue}>
      {children}
    </PerformanceContext.Provider>
  );
};