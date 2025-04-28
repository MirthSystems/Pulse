/**
 * Basic metric shape returned from the performance monitor
 */
export type PerformanceMetric = {
  name: string;
  value: number;
  timestamp: number;
  metadata?: Record<string, unknown>;
};