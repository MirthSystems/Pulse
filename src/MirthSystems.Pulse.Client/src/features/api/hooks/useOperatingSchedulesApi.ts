import { useCallback, useState } from 'react';
import { apiClient } from '../client';
import { IOperatingSchedule, IOperatingScheduleResponse, OperatingSchedule } from '../types/models';
import { useAuth } from '../../user/hooks/useAuth';

/**
 * Hook providing access to operating schedule-related API endpoints
 */
export function useOperatingSchedulesApi() {
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
   * Get operating schedules for a venue
   */
  const getVenueOperatingSchedules = useCallback((venueId: string) => {
    return executeRequest(() => 
      apiClient.get<IOperatingScheduleResponse[]>(`/api/operating-schedules/venue/${venueId}`)
    );
  }, [executeRequest]);

  /**
   * Get a specific operating schedule by ID
   */
  const getOperatingSchedule = useCallback((id: string) => {
    return executeRequest(() => 
      apiClient.get<IOperatingScheduleResponse>(`/api/operating-schedules/${id}`)
    );
  }, [executeRequest]);

  /**
   * Create a new operating schedule
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
    
    return executeRequest(async () => {
      const response = await apiClient.post<{ id: string }>('/api/operating-schedules', request);
      return response.id;
    });
  }, [executeRequest]);

  /**
   * Update an existing operating schedule
   */
  const updateOperatingSchedule = useCallback((id: string, schedule: OperatingSchedule | IOperatingSchedule) => {
    const request = schedule instanceof OperatingSchedule
      ? {
          timeOfOpen: schedule.timeOfOpen,
          timeOfClose: schedule.timeOfClose,
          isClosed: schedule.isClosed
        }
      : schedule;
    
    return executeRequest(() => 
      apiClient.put<void>(`/api/operating-schedules/${id}`, request)
    );
  }, [executeRequest]);

  /**
   * Delete an operating schedule
   */
  const deleteOperatingSchedule = useCallback((id: string) => {
    return executeRequest(() => 
      apiClient.delete<void>(`/api/operating-schedules/${id}`)
    );
  }, [executeRequest]);

  /**
   * Post multiple business hours for a venue
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
    
    return executeRequest(async () => {
      const response = await apiClient.post<{ ids: string[] }>(`/api/operating-schedules/venue/${venueId}`, requests);
      return response.ids;
    });
  }, [executeRequest]);

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
