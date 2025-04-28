/**
 * Configuration options for the Performance Provider
 */
export type PerformanceConfiguration = {
  enabled?: boolean;
  sampleRate?: number;
  maxStoredEvents?: number;
  reportingEndpoint?: string;
  reportingThreshold?: number;
  includeResourceTiming?: boolean;
  logToConsole?: boolean;
};