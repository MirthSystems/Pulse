/**
 * Performance monitoring utility for tracking and optimizing application performance
 * Provides tools for measuring render times, tracking user interactions, and collecting metrics
 */
import React from 'react';
import { PerformanceMetric } from '../types/performance-metric';
import { PerformanceEventType } from '../types/performance-event-type';
import { PerformanceMonitorConfiguration } from '../types/performance-monitor-configuration';

// Default configuration
const defaultConfig: PerformanceMonitorConfiguration = {
  enabled: process.env.NODE_ENV === 'production',
  sampleRate: 0.1, // 10% sampling in production
  maxStoredEvents: 100,
  includeResourceTiming: false, 
  logToConsole: process.env.NODE_ENV !== 'production'
};

// State for performance monitor
let metrics: PerformanceMetric[] = [];
const marks: Record<string, number> = {};
let config: PerformanceMonitorConfiguration = defaultConfig;
let initialized = false;

/**
 * Initialize the performance monitor with configuration options
 */
export const initPerformanceMonitor = (userConfig?: Partial<PerformanceMonitorConfiguration>) => {
  if (initialized) return;
  
  // Apply user config over default config
  config = { ...defaultConfig, ...userConfig };
  
  // Reset metrics array
  metrics = [];
  
  // Set up performance observer for resource timing if configured
  if (config.includeResourceTiming && typeof PerformanceObserver !== 'undefined') {
    const observer = new PerformanceObserver((list) => {
      for (const entry of list.getEntries()) {
        if (Math.random() < config.sampleRate) {
          recordMetric('resource-load', entry.name, entry.duration, {
            startTime: entry.startTime,
            entryType: entry.entryType,
            initiatorType: (entry as PerformanceResourceTiming).initiatorType || 'unknown',
          });
        }
      }
    });
    
    observer.observe({ entryTypes: ['resource', 'navigation'] });
  }
  
  // Capture initial page load time
  if (typeof window !== 'undefined' && window.performance) {
    const navigationTiming = window.performance.timing;
    if (navigationTiming) {
      const loadTime = navigationTiming.loadEventEnd - navigationTiming.navigationStart;
      const dnsTime = navigationTiming.domainLookupEnd - navigationTiming.domainLookupStart;
      const connectTime = navigationTiming.connectEnd - navigationTiming.connectStart;
      const renderTime = navigationTiming.domComplete - navigationTiming.domLoading;
      
      recordMetric('navigation', 'page-load', loadTime);
      recordMetric('navigation', 'dns-lookup', dnsTime);
      recordMetric('navigation', 'connection', connectTime);
      recordMetric('navigation', 'dom-rendering', renderTime);
    }
  }
  
  initialized = true;
};

/**
 * Start timing an operation
 */
export const markStart = (markName: string) => {
  if (!config.enabled) return;
  marks[markName] = performance.now();
};

/**
 * End timing an operation and record the duration
 */
export const markEnd = (
  markName: string, 
  eventType: PerformanceEventType = 'render',
  metadata?: Record<string, unknown>
) => {
  if (!config.enabled || !marks[markName]) return;
  
  const startTime = marks[markName];
  const endTime = performance.now();
  const duration = endTime - startTime;
  
  delete marks[markName];
  
  recordMetric(eventType, markName, duration, metadata);
  
  return duration;
};

/**
 * Record a performance metric
 */
export const recordMetric = (
  eventType: PerformanceEventType,
  name: string,
  value: number,
  metadata?: Record<string, unknown>
) => {
  if (!config.enabled || Math.random() > config.sampleRate) return;
  
  const metric: PerformanceMetric = {
    name: `${eventType}:${name}`,
    value,
    timestamp: Date.now(),
    metadata
  };
  
  // Add to metrics array, keeping under max size
  metrics.push(metric);
  if (metrics.length > config.maxStoredEvents) {
    metrics.shift();
  }
  
  // Log to console if enabled
  if (config.logToConsole && value > (config.reportingThreshold || 0)) {
    console.info(`Performance: ${metric.name} - ${value.toFixed(2)}ms`, metadata);
  }
  
  // Send to reporting endpoint if configured and over threshold
  if (config.reportingEndpoint && value > (config.reportingThreshold || 0)) {
    reportMetric(metric);
  }
};

/**
 * Send performance data to a reporting endpoint
 */
const reportMetric = (metric: PerformanceMetric) => {
  if (!config.reportingEndpoint) return;
  
  // Use sendBeacon if available for better reliability during page unload
  if (navigator.sendBeacon) {
    navigator.sendBeacon(
      config.reportingEndpoint,
      JSON.stringify(metric)
    );
  } else {
    // Fallback to fetch with keepalive
    fetch(config.reportingEndpoint, {
      method: 'POST',
      body: JSON.stringify(metric),
      keepalive: true,
      headers: {
        'Content-Type': 'application/json'
      }
    }).catch(() => {
      // Silently fail - this is just telemetry
    });
  }
};

/**
 * Get all collected metrics
 */
export const getMetrics = () => {
  return [...metrics];
};

/**
 * Clear all collected metrics
 */
export const clearMetrics = () => {
  metrics = [];
};

/**
 * Create a HOC to measure component render time
 */
export const withPerformanceTracking = <P extends object>(
  Component: React.ComponentType<P>,
  componentName?: string
): React.FC<P> => {
  const displayName = componentName || Component.displayName || Component.name || 'Component';
  
  const WrappedComponent: React.FC<P> = (props) => {
    const renderRef = React.useRef<number>();
    
    React.useEffect(() => {
      if (renderRef.current) {
        const renderTime = performance.now() - renderRef.current;
        recordMetric('render', `${displayName}`, renderTime);
      }
      
      return () => {
        markStart(`unmount:${displayName}`);
      };
    });
    
    React.useEffect(() => {
      return () => {
        markEnd(`unmount:${displayName}`);
      };
    }, []);
    
    renderRef.current = performance.now();
    
    return <Component {...props} />;
  };
  
  WrappedComponent.displayName = `WithPerformance(${displayName})`;
  
  return WrappedComponent;
};