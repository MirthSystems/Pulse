/**
 * React Query configuration and utility functions
 * Provides optimized data fetching with caching, background updates, and error handling
 */
import { 
  QueryClient, 
  DefaultOptions,
  QueryKey,
  UseQueryOptions,
  UseMutationOptions,
  QueryFunction
} from 'react-query';
import { createAppError, logError } from './errorHandler';
import { ErrorSeverity, ErrorSource } from '../types/error-types';

// Default query configuration
const defaultQueryOptions: DefaultOptions = {
  queries: {
    refetchOnWindowFocus: false,
    refetchOnMount: true,
    refetchOnReconnect: true,
    retry: 1,
    staleTime: 5 * 60 * 1000, // 5 minutes
    cacheTime: 10 * 60 * 1000, // 10 minutes
    onError: (error) => {
      // Log all query errors with our error handler
      const appError = createAppError(
        error instanceof Error ? error.message : 'Query error',
        ErrorSeverity.MEDIUM,
        ErrorSource.APP_API,
        error instanceof Error ? error : new Error(String(error))
      );
      logError(appError);
    }
  },
  mutations: {
    retry: 0,
    onError: (error) => {
      // Log all mutation errors with our error handler
      const appError = createAppError(
        error instanceof Error ? error.message : 'Mutation error',
        ErrorSeverity.MEDIUM,
        ErrorSource.APP_API,
        error instanceof Error ? error : new Error(String(error))
      );
      logError(appError);
    }
  }
};

// Create and configure QueryClient
export const queryClient = new QueryClient({
  defaultOptions: defaultQueryOptions
});

/**
 * Creates a type-safe query key factory for a specific domain/feature
 * @param domain The domain or feature name (e.g., 'users', 'specials')
 * @returns A function to create query keys for that domain
 */
export const createQueryKeyFactory = <_T extends string>(domain: string) => {
  const createKey = <K extends string = never>(
    subKey?: K, 
    ...params: unknown[]
  ): [string, K?, ...unknown[]] => {
    return [domain, subKey, ...params].filter(Boolean) as [string, K?, ...unknown[]];
  };
  
  return createKey;
};

/**
 * Creates default query options with integrated error handling
 */
export function createQueryOptions<
  TQueryFnData = unknown,
  TError = unknown,
  TData = TQueryFnData,
  TQueryKey extends QueryKey = QueryKey
>(
  options: Omit<UseQueryOptions<TQueryFnData, TError, TData, TQueryKey>, 'queryKey' | 'queryFn'> & {
    queryKey: TQueryKey;
    queryFn: QueryFunction<TQueryFnData, TQueryKey>;
    errorSource?: ErrorSource;
    errorSeverity?: ErrorSeverity;
  }
): UseQueryOptions<TQueryFnData, TError, TData, TQueryKey> {
  const { errorSource = ErrorSource.APP_API, errorSeverity = ErrorSeverity.MEDIUM, ...rest } = options;
  
  return {
    ...rest,
    onError: (error) => {
      const appError = createAppError(
        error instanceof Error ? error.message : 'Query error',
        errorSeverity,
        errorSource,
        error instanceof Error ? error : new Error(String(error))
      );
      logError(appError);
      
      // Call the original onError if provided
      if (options.onError) {
        options.onError(error);
      }
    }
  };
}

/**
 * Creates default mutation options with integrated error handling
 */
export function createMutationOptions<
  TData = unknown,
  TError = unknown,
  TVariables = void,
  TContext = unknown
>(
  options: Omit<UseMutationOptions<TData, TError, TVariables, TContext>, 'mutationFn'> & {
    mutationFn: (variables: TVariables) => Promise<TData>;
    errorSource?: ErrorSource;
    errorSeverity?: ErrorSeverity;
  }
): UseMutationOptions<TData, TError, TVariables, TContext> {
  const { errorSource = ErrorSource.APP_API, errorSeverity = ErrorSeverity.MEDIUM, ...rest } = options;
  
  return {
    ...rest,
    onError: (error, variables, context) => {
      const appError = createAppError(
        error instanceof Error ? error.message : 'Mutation error',
        errorSeverity,
        errorSource,
        error instanceof Error ? error : new Error(String(error))
      );
      logError(appError);
      
      // Call the original onError if provided
      if (options.onError) {
        options.onError(error, variables, context);
      }
    }
  };
}