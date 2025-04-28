/**
 * Configuration for performance monitoring utility
 */
export type PerformanceMonitorConfiguration = {
  enabled: boolean;
  sampleRate: number; // 0 to 1, percentage of events to sample
  maxStoredEvents: number;
  reportingEndpoint?: string;
  reportingThreshold?: number; // ms threshold for slow events
  includeResourceTiming: boolean;
  logToConsole: boolean;
};