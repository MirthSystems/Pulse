import { useCallback } from 'react';
import { ISpecial, ISpecialQueryParams, ISpecialResponse, Special } from '../types/models';
import { DateTime } from 'luxon';
import { apiClient, useApiClient } from './useApiClient';

/**
 * Hook providing access to specials-related API endpoints
 */
export function useSpecialsApi() {
  const { isLoading, error, executeRequest, executeProtectedRequest } = useApiClient();

  /**
   * Get specials with query parameters
   * [AllowAnonymous] endpoint
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
   * [AllowAnonymous] endpoint
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
   * [AllowAnonymous] endpoint
   */
  const getSpecial = useCallback((id: string) => {
    return executeRequest(() => 
      apiClient.get<ISpecialResponse>(`/api/specials/${id}`)
    );
  }, [executeRequest]);

  /**
   * Create a new special
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
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
    
    return executeProtectedRequest(async () => {
      const response = await apiClient.post<{ id: string }>('/api/specials', request);
      return response.id;
    });
  }, [executeProtectedRequest]);

  /**
   * Update an existing special
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
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
    
    return executeProtectedRequest(() => 
      apiClient.put<void>(`/api/specials/${id}`, request)
    );
  }, [executeProtectedRequest]);

  /**
   * Delete a special
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  const deleteSpecial = useCallback((id: string) => {
    return executeProtectedRequest(() => 
      apiClient.delete<void>(`/api/specials/${id}`)
    );
  }, [executeProtectedRequest]);

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
