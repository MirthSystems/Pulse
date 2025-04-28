import { createContext } from 'react';
import { PerformanceEventType } from '../../types/performance-event-type';
import { PerformanceMetric } from '../../types/performance-metric';

/**
 * Type for metadata used in performance records
 */
export type PerformanceMetadata = Record<string, string | number | boolean | null>;

/**
 * Interface for performance context value.
 * Provides measurement methods and utilities for tracking performance.
 */
export type PerformanceContextType = {
  /** Start a performance measurement with the given name */
  markStart: (markName: string) => void;
  
  /** End a performance measurement with the given name and record the duration */
  markEnd: (markName: string, eventType?: PerformanceEventType, metadata?: PerformanceMetadata) => number | undefined;
  
  /** Record an API call with duration and optional metadata */
  recordApiCall: (apiName: string, duration: number, metadata?: PerformanceMetadata) => void;
  
  /** Record a user interaction with duration and optional metadata */
  recordInteraction: (interactionName: string, duration: number, metadata?: PerformanceMetadata) => void;
  
  /** Record a navigation event with duration and optional metadata */
  recordNavigation: (routeName: string, duration: number, metadata?: PerformanceMetadata) => void;
  
  /** Get all collected metrics */
  getMetrics: () => PerformanceMetric[];
  
  /** Clear all collected metrics */
  clearMetrics: () => void;
  
  /** Measure an async operation and automatically record its duration */
  measureAsyncOperation: <T>(
    operationName: string, 
    operation: () => Promise<T>,
    eventType?: 'api-call' | 'navigation' | 'resource-load'
  ) => Promise<T>;
};

/**
 * React context for performance measurement and monitoring.
 * Provides utilities for measuring, recording, and retrieving performance metrics.
 */
const PerformanceContext = createContext<PerformanceContextType | undefined>(undefined);

export default PerformanceContext;