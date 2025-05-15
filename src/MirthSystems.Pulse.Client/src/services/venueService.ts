import { AxiosInstance } from 'axios';
import { 
  VenueItem, 
  VenueItemExtended, 
  VenueSearchParams, 
  CreateVenueRequest, 
  UpdateVenueRequest
} from '@models/venue';
import { OperatingScheduleItem } from '@models/operatingSchedule';
import { SpecialItem } from '@models/special';
import { PagedResult } from '@models/common';
import { anonymousClient } from './apiClient';

export class VenueService {
  // Anonymous endpoints (no auth required)
  static async getVenues(params: VenueSearchParams): Promise<PagedResult<VenueItem>> {
    // Map frontend params to backend expected format
    const requestParams: any = {
      page: params.page,
      pageSize: params.pageSize,
      searchText: params.searchText,
      address: params.address,
      radiusInMiles: params.radius,
      openOnDayOfWeek: params.openDay,
      timeOfDay: params.openTime,
      hasActiveSpecials: params.hasSpecials,
      specialTypeId: params.specialType,
      includeAddressDetails: params.includeAddress !== false, // Default to true
      includeBusinessHours: params.includeHours === true, // Default to false
      sortOrder: params.sort || 0
    };

    const response = await anonymousClient.get('/venues', { params: requestParams });
    return response.data;
  }

  static async getVenueById(id: string): Promise<VenueItemExtended> {
    const response = await anonymousClient.get(`/venues/${id}`);
    return response.data;
  }

  static async getVenueBusinessHours(id: string): Promise<OperatingScheduleItem[]> {
    const response = await anonymousClient.get(`/venues/${id}/business-hours`);
    return response.data;
  }

  static async getVenueSpecials(id: string): Promise<SpecialItem[]> {
    const response = await anonymousClient.get(`/venues/${id}/specials`);
    return response.data;
  }

  // Authenticated endpoints (require auth token)
  static async createVenue(venueData: CreateVenueRequest, apiClient: AxiosInstance): Promise<VenueItemExtended> {
    const response = await apiClient.post('/venues', venueData);
    return response.data;
  }

  static async updateVenue(id: string, venueData: UpdateVenueRequest, apiClient: AxiosInstance): Promise<VenueItemExtended> {
    const response = await apiClient.put(`/venues/${id}`, venueData);
    return response.data;
  }

  static async deleteVenue(id: string, apiClient: AxiosInstance): Promise<boolean> {
    const response = await apiClient.delete(`/venues/${id}`);
    return response.data;
  }
}
