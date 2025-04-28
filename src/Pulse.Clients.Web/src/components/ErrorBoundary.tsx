import React from 'react';
import ErrorComponent from './ErrorComponent';
import { createAppError, logError } from '../utils/errorHandler';
import { ErrorSeverity, ErrorSource } from '@/types/error-types';

interface ErrorBoundaryProps {
  children: React.ReactNode;
  fallback?: React.ReactNode;
}

interface ErrorBoundaryState {
  hasError: boolean;
  error: Error | null;
}

/**
 * Catches JavaScript errors anywhere in the child component tree and displays a fallback UI.
 * Uses the application's error handling utilities for consistent error management.
 */
class ErrorBoundary extends React.Component<ErrorBoundaryProps, ErrorBoundaryState> {
  constructor(props: ErrorBoundaryProps) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error: Error): ErrorBoundaryState {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    // Create and log a standardized application error
    const appError = createAppError(
      'An unexpected error occurred in the application',
      ErrorSeverity.HIGH,
      ErrorSource.UI,
      error,
      { 
        componentStack: errorInfo.componentStack,
        // Add any additional context that might be helpful for debugging
        url: window.location.href,
        timestamp: new Date().toISOString()
      }
    );
    
    logError(appError);

    // In production, you might want to send this to an error monitoring service
    // Example: Sentry.captureException(error, { extra: errorInfo });
  }

  render() {
    if (this.state.hasError) {
      // Use the custom fallback if provided
      if (this.props.fallback) {
        return this.props.fallback;
      }
      
      // Otherwise use our enhanced ErrorComponent
      return (
        <ErrorComponent 
          error={this.state.error || undefined}
          title="Something went wrong"
          showRetry={true}
          showHome={true}
        />
      );
    }
    
    return this.props.children;
  }
}

export default ErrorBoundary;
