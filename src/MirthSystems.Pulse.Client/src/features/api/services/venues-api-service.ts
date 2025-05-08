import { BaseApiService } from './base-api-service';
import { IVenue, IVenueResponse, IOperatingScheduleResponse, ISpecialResponse } from '../types/models';

/**
 * Service for managing venue-related API endpoints
 */
export class VenuesApiService extends BaseApiService {
  private readonly baseEndpoint = '/api/venues';

  /**
   * Get all venues with optional pagination
   * [AllowAnonymous] endpoint
   */
  async getAll(page?: number, pageSize?: number): Promise<IVenueResponse[]> {
    const queryParams: Record<string, unknown> = {};
    if (page !== undefined) queryParams.page = page;
    if (pageSize !== undefined) queryParams.pageSize = pageSize;
    
    return this.get<IVenueResponse[]>(this.baseEndpoint, queryParams);
  }

  /**
   * Get a specific venue by ID
   * [AllowAnonymous] endpoint
   */
  async getById(id: string): Promise<IVenueResponse> {
    return this.get<IVenueResponse>(`${this.baseEndpoint}/${id}`);
  }

  /**
   * Create a new venue
   * [Authorize(Roles = "System.Administrator")] endpoint
   */
  async create(venue: IVenue): Promise<string> {
    const response = await this.post<{ id: string }>(this.baseEndpoint, venue, true);
    return response.id;
  }

  /**
   * Update an existing venue
   * [Authorize(Roles = "System.Administrator")] endpoint
   */
  async update(id: string, venue: IVenue): Promise<void> {
    await this.put<void>(`${this.baseEndpoint}/${id}`, venue, true);
  }

  /**
   * Delete a venue
   * [Authorize(Roles = "System.Administrator")] endpoint
   */
  async delete(id: string): Promise<void> {
    await super.delete(`${this.baseEndpoint}/${id}`, true);
  }

  /**
   * Get business hours for a venue
   * [AllowAnonymous] endpoint
   */
  async getBusinessHours(venueId: string): Promise<IOperatingScheduleResponse[]> {
    return this.get<IOperatingScheduleResponse[]>(`${this.baseEndpoint}/${venueId}/business-hours`);
  }

  /**
   * Get specials for a venue
   * [AllowAnonymous] endpoint
   */
  async getSpecials(venueId: string): Promise<ISpecialResponse[]> {
    return this.get<ISpecialResponse[]>(`${this.baseEndpoint}/${venueId}/specials`);
  }
}