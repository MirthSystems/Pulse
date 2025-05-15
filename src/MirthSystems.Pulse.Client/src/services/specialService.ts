import { AxiosInstance } from 'axios';
import { 
  SpecialItem, 
  SpecialItemExtended, 
  SpecialSearchParams, 
  CreateSpecialRequest, 
  UpdateSpecialRequest,
  SearchSpecialsResult
} from '@models/special';
import { PagedResult } from '@models/common';
import { anonymousClient } from './apiClient';

export class SpecialService {
  // Anonymous endpoints
  static async searchSpecials(params: SpecialSearchParams): Promise<PagedResult<SearchSpecialsResult>> {
    // Map frontend params to backend expected format
    const requestParams: any = {
      page: params.page,
      pageSize: params.pageSize,
      address: params.address,
      radius: params.radius,
      searchDateTime: params.dateTime,
      searchTerm: params.term,
      venueId: params.venueId,
      specialTypeId: params.type,
      isCurrentlyRunning: params.active
    };

    const response = await anonymousClient.get('/specials', { params: requestParams });
    return response.data;
  }

  static async getSpecialById(id: string): Promise<SpecialItemExtended> {
    const response = await anonymousClient.get(`/specials/${id}`);
    return response.data;
  }

  // Authenticated endpoints
  static async createSpecial(specialData: CreateSpecialRequest, apiClient: AxiosInstance): Promise<SpecialItemExtended> {
    const response = await apiClient.post('/specials', specialData);
    return response.data;
  }

  static async updateSpecial(id: string, specialData: UpdateSpecialRequest, apiClient: AxiosInstance): Promise<SpecialItemExtended> {
    const response = await apiClient.put(`/specials/${id}`, specialData);
    return response.data;
  }

  static async deleteSpecial(id: string, apiClient: AxiosInstance): Promise<boolean> {
    const response = await apiClient.delete(`/specials/${id}`);
    return response.data;
  }
}
