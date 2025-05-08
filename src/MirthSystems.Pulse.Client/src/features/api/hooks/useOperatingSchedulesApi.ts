import { useCallback } from 'react';
import { IOperatingSchedule, IOperatingScheduleResponse, OperatingSchedule } from '../types/models';
import { apiClient, useApiClient } from './useApiClient';

/**
 * Hook providing access to operating schedule-related API endpoints
 */
export function useOperatingSchedulesApi() {
  const { isLoading, error, executeRequest, executeProtectedRequest } = useApiClient();

  /**
   * Get operating schedules for a venue
   * [AllowAnonymous] endpoint
   */
  const getVenueOperatingSchedules = useCallback((venueId: string) => {
    return executeRequest(() => 
      apiClient.get<IOperatingScheduleResponse[]>(`/api/operating-schedules/venue/${venueId}`)
    );
  }, [executeRequest]);

  /**
   * Get a specific operating schedule by ID
   * [AllowAnonymous] endpoint
   */
  const getOperatingSchedule = useCallback((id: string) => {
    return executeRequest(() => 
      apiClient.get<IOperatingScheduleResponse>(`/api/operating-schedules/${id}`)
    );
  }, [executeRequest]);

  /**
   * Create a new operating schedule
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  const createOperatingSchedule = useCallback((schedule: OperatingSchedule | IOperatingSchedule) => {
    const request = schedule instanceof OperatingSchedule
      ? {
          venueId: schedule.venueId,
          dayOfWeek: schedule.dayOfWeek,
          timeOfOpen: schedule.timeOfOpen,
          timeOfClose: schedule.timeOfClose,
          isClosed: schedule.isClosed
        }
      : schedule;
    
    return executeProtectedRequest(async () => {
      const response = await apiClient.post<{ id: string }>('/api/operating-schedules', request);
      return response.id;
    });
  }, [executeProtectedRequest]);

  /**
   * Update an existing operating schedule
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  const updateOperatingSchedule = useCallback((id: string, schedule: OperatingSchedule | IOperatingSchedule) => {
    const request = schedule instanceof OperatingSchedule
      ? {
          timeOfOpen: schedule.timeOfOpen,
          timeOfClose: schedule.timeOfClose,
          isClosed: schedule.isClosed
        }
      : schedule;
    
    return executeProtectedRequest(() => 
      apiClient.put<void>(`/api/operating-schedules/${id}`, request)
    );
  }, [executeProtectedRequest]);

  /**
   * Delete an operating schedule
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  const deleteOperatingSchedule = useCallback((id: string) => {
    return executeProtectedRequest(() => 
      apiClient.delete<void>(`/api/operating-schedules/${id}`)
    );
  }, [executeProtectedRequest]);

  /**
   * Post multiple business hours for a venue
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  const postVenueBusinessHours = useCallback((venueId: string, schedules: OperatingSchedule[] | IOperatingSchedule[]) => {
    const requests = schedules.map(schedule => 
      schedule instanceof OperatingSchedule
        ? {
            dayOfWeek: schedule.dayOfWeek,
            timeOfOpen: schedule.timeOfOpen,
            timeOfClose: schedule.timeOfClose,
            isClosed: schedule.isClosed
          }
        : schedule
    );
    
    return executeProtectedRequest(async () => {
      const response = await apiClient.post<{ ids: string[] }>(`/api/operating-schedules/venue/${venueId}`, requests);
      return response.ids;
    });
  }, [executeProtectedRequest]);

  return {
    // API state
    isLoading,
    error,
    
    // Operating schedule methods
    getVenueOperatingSchedules,
    getOperatingSchedule,
    createOperatingSchedule,
    updateOperatingSchedule,
    deleteOperatingSchedule,
    postVenueBusinessHours,
  };
}
