import { useCallback, useState } from 'react';
import { apiClient } from '../client';
import { IVenue, IVenueResponse, Venue, IOperatingScheduleResponse, ISpecialResponse } from '../types/models';
import { useAuth } from '../../user/hooks/useAuth';

/**
 * Hook providing access to venue-related API endpoints
 */
export function useVenuesApi() {
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
   * Get all venues with optional pagination
   */
  const getVenues = useCallback((page?: number, pageSize?: number) => {
    const queryParams: Record<string, unknown> = {};
    if (page !== undefined) queryParams.page = page;
    if (pageSize !== undefined) queryParams.pageSize = pageSize;
    
    return executeRequest(() => 
      apiClient.get<IVenueResponse[]>('/api/venues', queryParams)
    );
  }, [executeRequest]);

  /**
   * Get a specific venue by ID
   */
  const getVenue = useCallback((id: string) => {
    return executeRequest(() => 
      apiClient.get<IVenueResponse>(`/api/venues/${id}`)
    );
  }, [executeRequest]);

  /**
   * Create a new venue
   */
  const createVenue = useCallback((venue: Venue | IVenue) => {
    const request = venue instanceof Venue
      ? {
          name: venue.name,
          description: venue.description,
          phoneNumber: venue.phoneNumber,
          website: venue.website,
          email: venue.email,
          profileImage: venue.profileImage,
          address: venue.address,
          businessHours: venue.businessHours?.map(h => ({
            dayOfWeek: h.dayOfWeek,
            timeOfOpen: h.timeOfOpen,
            timeOfClose: h.timeOfClose,
            isClosed: h.isClosed
          }))
        }
      : venue;
    
    return executeRequest(async () => {
      const response = await apiClient.post<{ id: string }>('/api/venues', request);
      return response.id;
    });
  }, [executeRequest]);

  /**
   * Update an existing venue
   */
  const updateVenue = useCallback((id: string, venue: Venue | IVenue) => {
    const request = venue instanceof Venue
      ? {
          name: venue.name,
          description: venue.description,
          phoneNumber: venue.phoneNumber,
          website: venue.website,
          email: venue.email,
          profileImage: venue.profileImage,
          address: venue.address
        }
      : venue;
    
    return executeRequest(() => 
      apiClient.put<void>(`/api/venues/${id}`, request)
    );
  }, [executeRequest]);

  /**
   * Delete a venue
   */
  const deleteVenue = useCallback((id: string) => {
    return executeRequest(() => 
      apiClient.delete<void>(`/api/venues/${id}`)
    );
  }, [executeRequest]);

  /**
   * Get business hours for a venue
   */
  const getBusinessHours = useCallback((venueId: string) => {
    return executeRequest(() => 
      apiClient.get<IOperatingScheduleResponse[]>(`/api/venues/${venueId}/business-hours`)
    );
  }, [executeRequest]);

  /**
   * Get specials for a venue
   */
  const getVenueSpecials = useCallback((venueId: string) => {
    return executeRequest(() => 
      apiClient.get<ISpecialResponse[]>(`/api/venues/${venueId}/specials`)
    );
  }, [executeRequest]);

  return {
    // API state
    isLoading,
    error,
    
    // Venue methods
    getVenues,
    getVenue,
    createVenue,
    updateVenue,
    deleteVenue,
    getBusinessHours,
    getVenueSpecials,
  };
}
