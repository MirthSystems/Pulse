import { PagedResult } from '@models/common';
import { OperatingScheduleItem } from '@models/operatingSchedule';
import { SpecialItem } from '@models/special';
import {
  CreateVenueRequest,
  GetVenuesRequest,
  UpdateVenueRequest,
  VenueItem,
  VenueItemExtended,
  VenueSearchParams
} from '@models/venue';
import { AxiosInstance } from 'axios';
import { createQueryString, publicApiClient } from './apiClient';

export const VenueService = {
  // Convert search params to API request format
  mapSearchParamsToRequest: (params: VenueSearchParams): GetVenuesRequest => {
    return {
      page: params.page || 1,
      pageSize: params.pageSize || 20,
      searchText: params.searchText,
      address: params.address,
      radiusInMiles: params.radius,
      openOnDayOfWeek: params.openDay,
      timeOfDay: params.openTime,
      hasActiveSpecials: params.hasSpecials,
      specialTypeId: params.specialType,
      includeAddressDetails: params.includeAddress ?? true,
      includeBusinessHours: params.includeHours ?? false,
      sortOrder: params.sort ?? 0
    };
  },

  // Get venues with filtering and pagination
  getVenues: async (params: VenueSearchParams): Promise<PagedResult<VenueItem>> => {
    const request = VenueService.mapSearchParamsToRequest(params);
    const queryString = createQueryString(request);
    const response = await publicApiClient.get(`/venues${queryString}`);
    return response.data;
  },

  // Get a specific venue by ID
  getVenueById: async (id: string): Promise<VenueItemExtended> => {
    const response = await publicApiClient.get(`/venues/${id}`);
    return response.data;
  },

  // Get business hours for a venue
  getVenueBusinessHours: async (id: string): Promise<OperatingScheduleItem[]> => {
    const response = await publicApiClient.get(`/venues/${id}/business-hours`);
    return response.data;
  },

  // Get specials for a venue
  getVenueSpecials: async (venueId: string): Promise<SpecialItem[]> => {
    try {
      // Use the correct API endpoint format
      const response = await publicApiClient.get<SpecialItem[]>(`/api/specials/venue/${venueId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching venue specials:', error);
      throw error;
    }
  },

  // Create a new venue (requires authentication)
  createVenue: async (data: CreateVenueRequest, apiClient: AxiosInstance): Promise<VenueItemExtended> => {
    const response = await apiClient.post('/venues', data);
    return response.data;
  },

  // Update an existing venue (requires authentication)
  updateVenue: async (id: string, data: UpdateVenueRequest, apiClient: AxiosInstance): Promise<VenueItemExtended> => {
    const response = await apiClient.put(`/venues/${id}`, data);
    return response.data;
  },

  // Delete a venue (requires authentication)
  deleteVenue: async (id: string, apiClient: AxiosInstance): Promise<boolean> => {
    const response = await apiClient.delete(`/venues/${id}`);
    return response.data;
  }
};
