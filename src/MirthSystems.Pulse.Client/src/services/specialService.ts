import { AxiosInstance } from 'axios';
import { publicApiClient, createQueryString } from './apiClient';
import { 
  SpecialItem, 
  SpecialItemExtended, 
  CreateSpecialRequest, 
  UpdateSpecialRequest,
  SpecialSearchParams,
  GetSpecialsRequest,
  SearchSpecialsResult
} from '@models/special';
import { PagedResult } from '@models/common';

export const SpecialService = {
  // Convert search params to API request format
  mapSearchParamsToRequest: (params: SpecialSearchParams): GetSpecialsRequest => {
    return {
      page: params.page || 1,
      pageSize: params.pageSize || 20,
      address: params.address,
      radius: params.radius || 5,
      searchDateTime: params.dateTime,
      searchTerm: params.term,
      venueId: params.venueId,
      specialTypeId: params.type,
      isCurrentlyRunning: params.active
    };
  },

  // Search for specials with filtering and pagination
  searchSpecials: async (params: SpecialSearchParams): Promise<PagedResult<SearchSpecialsResult>> => {
    const request = SpecialService.mapSearchParamsToRequest(params);
    const queryString = createQueryString(request);
    const response = await publicApiClient.get(`/specials${queryString}`);
    return response.data;
  },

  // Get a specific special by ID
  getSpecialById: async (id: string): Promise<SpecialItemExtended> => {
    const response = await publicApiClient.get(`/specials/${id}`);
    return response.data;
  },

  // Create a new special (requires authentication)
  createSpecial: async (data: CreateSpecialRequest, apiClient: AxiosInstance): Promise<SpecialItemExtended> => {
    const response = await apiClient.post('/specials', data);
    return response.data;
  },

  // Update an existing special (requires authentication)
  updateSpecial: async (id: string, data: UpdateSpecialRequest, apiClient: AxiosInstance): Promise<SpecialItemExtended> => {
    const response = await apiClient.put(`/specials/${id}`, data);
    return response.data;
  },

  // Delete a special (requires authentication)
  deleteSpecial: async (id: string, apiClient: AxiosInstance): Promise<boolean> => {
    const response = await apiClient.delete(`/specials/${id}`);
    return response.data;
  }
};
