import { useCallback, useEffect, useMemo } from 'react';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { setLoading, setError, setLastFetched } from '../redux/api';

/**
 * Custom hook to manage API requests with Redux integration
 * @param key A unique identifier for this API request
 * @param apiCall The function that makes the API call
 * @param dependencies Optional dependencies array for when to refresh the data
 * @param options Configuration options
 */
export const useApiRequest = <T>(
  key: string,
  apiCall: () => Promise<T>,
  dependencies: unknown[] = [],
  options: {
    initialData?: T;
    skipInitialCall?: boolean;
    cacheDuration?: number;
  } = {}
) => {
  const { initialData, skipInitialCall = false, cacheDuration = 60000 } = options;

  const dispatch = useAppDispatch();
  const isLoading = useAppSelector((state) => state.api.isLoading[key] || false);
  const error = useAppSelector((state) => state.api.errors[key] || null);
  const lastFetched = useAppSelector((state) => state.api.lastFetched[key] || null);

  const stableDependencies = useMemo(() => dependencies, [dependencies]);

  const isStale = useCallback(() => {
    if (lastFetched === null) return true;
    return Date.now() - lastFetched > cacheDuration;
  }, [lastFetched, cacheDuration]);

  const execute = useCallback(async (): Promise<T | undefined> => {
    if (!isStale() && initialData !== undefined) {
      return initialData;
    }

    try {
      dispatch(setLoading({ key, isLoading: true }));
      dispatch(setError({ key, error: null }));

      const result = await apiCall();

      dispatch(setLastFetched({ key, timestamp: Date.now() }));
      return result;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Unknown error occurred';
      dispatch(setError({ key, error: errorMessage }));
      return undefined;
    } finally {
      dispatch(setLoading({ key, isLoading: false }));
    }
  }, [apiCall, dispatch, key, isStale, initialData]);

  useEffect(() => {
    if (!skipInitialCall) {
      execute();
    }
  }, [execute, skipInitialCall, stableDependencies]);

  return {
    isLoading,
    error,
    lastFetched,
    execute,
    isStale: isStale(),
  };
};