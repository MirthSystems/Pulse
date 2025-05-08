import { IOperatingScheduleResponse, ISpecialResponse, IVenue, IVenueResponse, Venue } from '../types/models';
import { ApiService } from './ApiService';

export class VenuesApiService extends ApiService {
  private readonly basePath = '/api/venues';

  async getVenues(searchTerm?: string, page?: number, pageSize?: number): Promise<IVenueResponse[]> {
    const queryParams: Record<string, unknown> = {};
    
    if (searchTerm) {
      queryParams.searchTerm = searchTerm;
    }
    
    if (page !== undefined) {
      queryParams.page = page;
    }
    
    if (pageSize !== undefined) {
      queryParams.pageSize = pageSize;
    }
    
    const response = await this.get<IVenueResponse[]>(this.basePath, queryParams);
    return response;
  }

  async getVenue(id: string): Promise<IVenueResponse> {
    const endpoint = `${this.basePath}/${id}`;
    const response = await this.get<IVenueResponse>(endpoint);
    return response;
  }

  async createVenue(venue: Venue | IVenue): Promise<string> {
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
    
    const response = await this.post<{ id: string }>(this.basePath, request);
    return response.id;
  }

  async updateVenue(id: string, venue: Venue | IVenue): Promise<void> {
    const endpoint = `${this.basePath}/${id}`;
    
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
    
    await this.put<void>(endpoint, request);
  }

  async deleteVenue(id: string): Promise<void> {
    const endpoint = `${this.basePath}/${id}`;
    await this.delete<void>(endpoint);
  }

  async searchVenuesByLocation(
    address: string,
    radius?: number,
    page?: number,
    pageSize?: number
  ): Promise<IVenueResponse[]> {
    const queryParams: Record<string, unknown> = {
      address
    };
    
    if (radius !== undefined) {
      queryParams.radius = radius;
    }
    
    if (page !== undefined) {
      queryParams.page = page;
    }
    
    if (pageSize !== undefined) {
      queryParams.pageSize = pageSize;
    }
    
    const endpoint = `${this.basePath}/search`;
    const response = await this.get<IVenueResponse[]>(endpoint, queryParams);
    return response;
  }

  async getBusinessHours(venueId: string): Promise<IOperatingScheduleResponse[]> {
    const endpoint = `${this.basePath}/${venueId}/operatingschedules`;
    const response = await this.get<IOperatingScheduleResponse[]>(endpoint);
    return response;
  }

  async getVenueSpecials(venueId: string): Promise<ISpecialResponse[]> {
    const endpoint = `${this.basePath}/${venueId}/specials`;
    const response = await this.get<ISpecialResponse[]>(endpoint);
    return response;
  }
}