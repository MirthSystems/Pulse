import { useCallback } from 'react';
import { ISpecial, ISpecialQueryParams, Special } from '../types/models';
import { DateTime } from 'luxon';
import { useApiClient } from './useApiClient';
import { specialsService } from '../services';

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
    return executeRequest(() => 
      specialsService.getSpecials(params)
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
      specialsService.getById(id)
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
    
    return executeProtectedRequest(() => 
      specialsService.create(request)
    );
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
      specialsService.update(id, request)
    );
  }, [executeProtectedRequest]);

  /**
   * Delete a special
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  const deleteSpecial = useCallback((id: string) => {
    return executeProtectedRequest(() => 
      specialsService.delete(id)
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
