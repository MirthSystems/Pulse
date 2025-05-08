import { useCallback } from 'react';
import { useApiClient } from './useApiClient';
import { venuesService } from '../services';
import { IVenue, Venue } from '../types/models';

/**
 * Hook providing access to venue-related API endpoints with loading and error handling
 */
export function useVenuesApi() {
  const { isLoading, error, executeRequest, executeProtectedRequest } = useApiClient();

  /**
   * Get all venues with optional pagination
   * [AllowAnonymous] endpoint
   */
  const getVenues = useCallback((page?: number, pageSize?: number) => {
    return executeRequest(() => 
      venuesService.getAll(page, pageSize)
    );
  }, [executeRequest]);

  /**
   * Get a specific venue by ID
   * [AllowAnonymous] endpoint
   */
  const getVenue = useCallback((id: string) => {
    return executeRequest(() => 
      venuesService.getById(id)
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
    
    return executeProtectedRequest(() => 
      venuesService.create(request)
    );
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
      venuesService.update(id, request)
    );
  }, [executeProtectedRequest]);

  /**
   * Delete a venue
   * [Authorize(Roles = "System.Administrator")] endpoint
   */
  const deleteVenue = useCallback((id: string) => {
    return executeProtectedRequest(() => 
      venuesService.delete(id)
    );
  }, [executeProtectedRequest]);

  /**
   * Get business hours for a venue
   * [AllowAnonymous] endpoint
   */
  const getBusinessHours = useCallback((venueId: string) => {
    return executeRequest(() => 
      venuesService.getBusinessHours(venueId)
    );
  }, [executeRequest]);

  /**
   * Get specials for a venue
   * [AllowAnonymous] endpoint
   */
  const getVenueSpecials = useCallback((venueId: string) => {
    return executeRequest(() => 
      venuesService.getSpecials(venueId)
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
