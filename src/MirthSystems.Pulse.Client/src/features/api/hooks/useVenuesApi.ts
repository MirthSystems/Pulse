import { useCallback } from 'react';
import { IVenue, IVenueResponse, Venue, IOperatingScheduleResponse, ISpecialResponse } from '../types/models';
import { apiClient, useApiClient } from './useApiClient';

/**
 * Hook providing access to venue-related API endpoints
 */
export function useVenuesApi() {
  const { isLoading, error, executeRequest, executeProtectedRequest } = useApiClient();

  /**
   * Get all venues with optional pagination
   * [AllowAnonymous] endpoint
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
   * [AllowAnonymous] endpoint
   */
  const getVenue = useCallback((id: string) => {
    return executeRequest(() => 
      apiClient.get<IVenueResponse>(`/api/venues/${id}`)
    );
  }, [executeRequest]);

  /**
   * Create a new venue
   * [Authorize(Roles = "System.Administrator")] endpoint
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
    
    return executeProtectedRequest(async () => {
      const response = await apiClient.post<{ id: string }>('/api/venues', request);
      return response.id;
    });
  }, [executeProtectedRequest]);

  /**
   * Update an existing venue
   * [Authorize(Roles = "System.Administrator")] endpoint
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
    
    return executeProtectedRequest(() => 
      apiClient.put<void>(`/api/venues/${id}`, request)
    );
  }, [executeProtectedRequest]);

  /**
   * Delete a venue
   * [Authorize(Roles = "System.Administrator")] endpoint
   */
  const deleteVenue = useCallback((id: string) => {
    return executeProtectedRequest(() => 
      apiClient.delete<void>(`/api/venues/${id}`)
    );
  }, [executeProtectedRequest]);

  /**
   * Get business hours for a venue
   * [AllowAnonymous] endpoint
   */
  const getBusinessHours = useCallback((venueId: string) => {
    return executeRequest(() => 
      apiClient.get<IOperatingScheduleResponse[]>(`/api/venues/${venueId}/business-hours`)
    );
  }, [executeRequest]);

  /**
   * Get specials for a venue
   * [AllowAnonymous] endpoint
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
