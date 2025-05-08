import { useCallback, useState } from 'react';
import { useAuth } from '../../user/hooks/useAuth';
import { ApiClient } from '../client';

export const apiClient = new ApiClient();

export function useApiClient() {
  const { getToken } = useAuth();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

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
      const errorMessage = err instanceof Error ? err.message : 'An unknown error occurred';
      setError(errorMessage);
      throw err;
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
      const errorMessage = err instanceof Error ? err.message : 'An unknown error occurred';
      setError(errorMessage);
      throw err;
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