import { useCallback, useState } from 'react';
import { apiClient } from '../client';
import { ISpecial, ISpecialQueryParams, ISpecialResponse, Special } from '../types/models';
import { DateTime } from 'luxon';
import { useAuth } from '../../user/hooks/useAuth';

/**
 * Hook providing access to specials-related API endpoints
 */
export function useSpecialsApi() {
  const { getToken } = useAuth();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  /**
   * Handles API request execution with authentication and error handling
   */
  const executeRequest = useCallback(async <T>(
    requestFn: () => Promise<T>
  ): Promise<T> => {
    setIsLoading(true);
    setError(null);
    
    try {
      // Ensure we have a valid token
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

  /**
   * Get specials with query parameters
   */
  const getSpecials = useCallback((params: ISpecialQueryParams) => {
    const queryParams = { ...params };
    
    // Convert DateTime objects to ISO strings
    if (params.searchDateTime && typeof params.searchDateTime === 'object' && 'toISO' in params.searchDateTime) {
      queryParams.searchDateTime = (params.searchDateTime as DateTime).toISO() || undefined;
    }
    
    return executeRequest(() => 
      apiClient.get<ISpecialResponse[]>('/api/specials', queryParams)
    );
  }, [executeRequest]);

  /**
   * Search specials with more friendly parameters
   */
  const searchSpecials = useCallback((
    address: string,
    options: {
      radius?: number;
      searchDateTime?: DateTime | string;
      specialTypeId?: number;
      venueId?: string;
      isCurrentlyRunning?: boolean;
      page?: number;
      pageSize?: number;
    } = {}
  ) => {
    const searchParams: ISpecialQueryParams = {
      address,
      radius: options.radius,
      specialTypeId: options.specialTypeId,
      venueId: options.venueId,
      isCurrentlyRunning: options.isCurrentlyRunning,
      page: options.page,
      pageSize: options.pageSize
    };

    if (options.searchDateTime) {
      searchParams.searchDateTime = typeof options.searchDateTime === 'object' && 'toISO' in options.searchDateTime
        ? options.searchDateTime.toISO() || undefined
        : options.searchDateTime;
    }

    return getSpecials(searchParams);
  }, [getSpecials]);

  /**
   * Get a specific special by ID
   */
  const getSpecial = useCallback((id: string) => {
    return executeRequest(() => 
      apiClient.get<ISpecialResponse>(`/api/specials/${id}`)
    );
  }, [executeRequest]);

  /**
   * Create a new special
   */
  const createSpecial = useCallback((special: Special | ISpecial) => {
    const request = special instanceof Special
      ? {
          content: special.content,
          type: special.type,
          startDate: special.startDate,
          startTime: special.startTime,
          endTime: special.endTime,
          expirationDate: special.expirationDate,
          isRecurring: special.isRecurring,
          cronSchedule: special.cronSchedule,
          venueId: special.venueId
        }
      : special;
    
    return executeRequest(async () => {
      const response = await apiClient.post<{ id: string }>('/api/specials', request);
      return response.id;
    });
  }, [executeRequest]);

  /**
   * Update an existing special
   */
  const updateSpecial = useCallback((id: string, special: Special | ISpecial) => {
    const request = special instanceof Special
      ? {
          content: special.content,
          type: special.type,
          startDate: special.startDate,
          startTime: special.startTime,
          endTime: special.endTime,
          expirationDate: special.expirationDate,
          isRecurring: special.isRecurring,
          cronSchedule: special.cronSchedule
        }
      : special;
    
    return executeRequest(() => 
      apiClient.put<void>(`/api/specials/${id}`, request)
    );
  }, [executeRequest]);

  /**
   * Delete a special
   */
  const deleteSpecial = useCallback((id: string) => {
    return executeRequest(() => 
      apiClient.delete<void>(`/api/specials/${id}`)
    );
  }, [executeRequest]);

  return {
    // API state
    isLoading,
    error,
    
    // Special methods
    getSpecials,
    searchSpecials,
    getSpecial,
    createSpecial,
    updateSpecial,
    deleteSpecial,
  };
}
