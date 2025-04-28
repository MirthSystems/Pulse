/**
 * Types for error handling system
 */

/**
 * Error severity levels
 */
export enum ErrorSeverity {
  FATAL = 'fatal',     // Application-breaking errors that prevent core functionality
  HIGH = 'high',       // Serious errors that significantly impact user experience
  MEDIUM = 'medium',   // Moderate errors that cause some functionality to fail
  LOW = 'low'          // Minor errors that don't significantly impact user experience
}

/**
 * Error sources
 */
export enum ErrorSource {
  APP_CODE = 'app-code',       // Application code errors
  APP_API = 'app-api',         // Application API errors
  THIRD_PARTY = 'third-party', // Third-party service or API errors
  USER = 'user',               // User-generated errors (e.g., invalid input)
  NETWORK = 'network',         // Network-related errors
  UI = 'ui'                    // UI-related errors
}

/**
 * Standardized application error type
 */
export type AppError = {
  /** Human-readable error message */
  message: string;
  /** Severity level of the error */
  severity: ErrorSeverity;
  /** Source/origin of the error */
  source: ErrorSource;
  /** Original error object if available */
  originalError?: Error | unknown;
  /** Timestamp when the error occurred */
  timestamp: number;
  /** Optional error code for specific error types */
  code?: string;
  /** Additional contextual information about the error */
  context?: Record<string, unknown>;
}