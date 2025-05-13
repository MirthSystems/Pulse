import { AxiosInstance } from 'axios';
import { publicApiClient, createQueryString } from './apiClient';
import { 
  SpecialItem, 
  SpecialItemExtended, 
  SpecialSearchParams, 
  SearchSpecialsResult 
} from '@models/special';
import { PagedResult } from '@models/common';

export class SpecialService {
  static async searchSpecials(params: SpecialSearchParams): Promise<PagedResult<SearchSpecialsResult>> {
    try {
      const queryString = createQueryString({
        page: params.page,
        pageSize: params.pageSize,
        address: params.address,
        radius: params.radius,
        searchTerm: params.term,
        specialTypeId: params.type,
        isCurrentlyRunning: params.active !== undefined ? params.active.toString() : 'true',
        venueId: params.venueId,
        searchDateTime: params.dateTime
      });

      const response = await publicApiClient.get(`/specials${queryString}`);
      return response.data;
    } catch (error) {
      console.error('Error in searchSpecials:', error);
      throw error;
    }
  }

  static async getSpecialById(id: string): Promise<SpecialItemExtended> {
    try {
      const response = await publicApiClient.get(`/specials/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching special ${id}:`, error);
      throw error;
    }
  }

  static async createSpecial(specialData: any, apiClient: AxiosInstance): Promise<SpecialItemExtended> {
    try {
      const response = await apiClient.post('/specials', specialData);
      return response.data;
    } catch (error) {
      console.error('Error creating special:', error);
      throw error;
    }
  }

  static async updateSpecial(id: string, specialData: any, apiClient: AxiosInstance): Promise<SpecialItemExtended> {
    try {
      const response = await apiClient.put(`/specials/${id}`, specialData);
      return response.data;
    } catch (error) {
      console.error('Error updating special:', error);
      throw error;
    }
  }

  static async deleteSpecial(id: string, apiClient: AxiosInstance): Promise<boolean> {
    try {
      await apiClient.delete(`/specials/${id}`);
      return true;
    } catch (error) {
      console.error('Error deleting special:', error);
      throw error;
    }
  }
}
