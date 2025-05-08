import { useCallback, useState } from 'react';
import { useAuth } from '../../user/hooks/useAuth';
import { ApiError } from '../client';

/**
 * Hook for interacting with API resources that provides error handling and loading state
 */
export function useApiClient() {
  const { getToken } = useAuth();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<ApiError | null>(null);

  /**
   * Executes a request with error handling
   * For public endpoints that don't require authentication
   */
  const executeRequest = useCallback(async <T>(
    requestFn: () => Promise<T>
  ): Promise<T> => {
    setIsLoading(true);
    setError(null);
    
    try {
      return await requestFn();
    } catch (err) {
      const apiError = err instanceof ApiError 
        ? err 
        : new ApiError(err instanceof Error ? err.message : 'An unknown error occurred', 500);
      
      setError(apiError);
      throw apiError;
    } finally {
      setIsLoading(false);
    }
  }, []);

  /**
   * Executes a request with authentication and error handling
   * For protected endpoints that require authentication
   */
  const executeProtectedRequest = useCallback(async <T>(
    requestFn: () => Promise<T>
  ): Promise<T> => {
    setIsLoading(true);
    setError(null);
    
    try {
      // Ensure we have a valid token for protected endpoints
      await getToken();
      return await requestFn();
    } catch (err) {
      const apiError = err instanceof ApiError 
        ? err 
        : new ApiError(err instanceof Error ? err.message : 'An unknown error occurred', 500);
      
      setError(apiError);
      
      // Handle authentication errors specially
      if (apiError.isAuthError()) {
        // Try to refresh the token once
        try {
          await getToken();
          // If token refresh succeeded, retry the request
          return await requestFn();
        } catch {
          // If token refresh fails, throw the original error
          throw apiError;
        }
      }
      
      throw apiError;
    } finally {
      setIsLoading(false);
    }
  }, [getToken]);

  return {
    isLoading,
    error,
    executeRequest,
    executeProtectedRequest
  };
}