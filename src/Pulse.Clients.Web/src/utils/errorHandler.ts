/**
 * Error handling utilities for standardized error management throughout the application
 */
import { ErrorSeverity, ErrorSource, AppError } from '../types/error-types';

/**
 * Creates a standardized application error
 */
export function createAppError(
  message: string,
  severity: ErrorSeverity = ErrorSeverity.MEDIUM,
  source: ErrorSource = ErrorSource.APP_CODE,
  originalError?: Error | unknown,
  context?: Record<string, unknown>,
  code?: string
): AppError {
  return {
    message,
    severity,
    source,
    originalError,
    timestamp: Date.now(),
    code,
    context
  };
}

/**
 * Logs an error with appropriate handling based on severity
 */
export function logError(error: AppError): void {
  // Add timestamp if not present
  if (!error.timestamp) {
    error.timestamp = Date.now();
  }

  // Standard console logging for all errors
  const logMethod = error.severity === ErrorSeverity.LOW 
    ? console.info 
    : error.severity === ErrorSeverity.MEDIUM 
      ? console.warn 
      : console.error;
    
  logMethod(
    `[${error.severity.toUpperCase()}] [${error.source}] ${error.message}`,
    error.originalError || '',
    error.context || ''
  );

  // For high and fatal errors, you might want to report to monitoring service
  if (error.severity === ErrorSeverity.HIGH || error.severity === ErrorSeverity.FATAL) {
    // Example: send to monitoring service
    // reportToMonitoringService(error);
    
    // Record in performance monitoring
    if (typeof window !== 'undefined' && window.performance) {
      try {
        const perfEntryName = `error:${error.source}:${error.severity}`;
        performance.mark(perfEntryName);
      } catch (e) {
        // Silently fail if performance marking fails
      }
    }
  }
}

/**
 * Gets a user-friendly error message based on error details
 */
export function getUserFriendlyMessage(error: AppError | Error | unknown): string {
  if (isAppError(error)) {
    // For AppError, use the message directly
    return error.message;
  } else if (error instanceof Error) {
    // For standard Error, clean up the message
    return cleanErrorMessage(error.message);
  } else if (typeof error === 'string') {
    return cleanErrorMessage(error);
  } else {
    return 'An unexpected error occurred. Please try again later.';
  }
}

/**
 * Type guard to check if an error is an AppError
 */
export function isAppError(error: any): error is AppError {
  return (
    error &&
    typeof error === 'object' &&
    'severity' in error &&
    'source' in error &&
    'message' in error
  );
}

/**
 * Clean up error messages to be more user-friendly
 */
function cleanErrorMessage(message: string): string {
  // Remove technical details like stack traces, error codes, etc.
  message = message.split('\n')[0]; // Only take first line
  
  // Remove common prefixes like "Error:", "TypeError:", etc.
  message = message.replace(/^(Error|TypeError|ReferenceError|SyntaxError|RangeError):\s*/i, '');
  
  // Capitalize first letter if needed
  if (message.length > 0) {
    message = message.charAt(0).toUpperCase() + message.slice(1);
  }
  
  // Ensure message ends with punctuation
  if (message.length > 0 && !message.endsWith('.') && !message.endsWith('!') && !message.endsWith('?')) {
    message += '.';
  }
  
  return message || 'An unexpected error occurred.';
}

/**
 * Utility for handling async operations with standardized error handling
 */
export async function handleAsyncError<T>(
  operation: () => Promise<T>,
  errorContext: {
    message?: string;
    severity?: ErrorSeverity;
    source?: ErrorSource;
    context?: Record<string, unknown>;
  } = {}
): Promise<[T | null, AppError | null]> {
  try {
    const result = await operation();
    return [result, null];
  } catch (error) {
    const appError = createAppError(
      errorContext.message || (error instanceof Error ? error.message : 'An error occurred'),
      errorContext.severity || ErrorSeverity.MEDIUM,
      errorContext.source || ErrorSource.APP_CODE,
      error,
      errorContext.context
    );
    
    logError(appError);
    return [null, appError];
  }
}