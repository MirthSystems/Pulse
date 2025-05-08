import { BaseApiService } from './base-api-service';
import { ISpecial, ISpecialResponse, ISpecialQueryParams } from '../types/models';
import { DateTime } from 'luxon';

/**
 * Service for managing specials-related API endpoints
 */
export class SpecialsApiService extends BaseApiService {
  private readonly baseEndpoint = '/api/specials';

  /**
   * Get specials with query parameters
   * [AllowAnonymous] endpoint
   */
  async getSpecials(params: ISpecialQueryParams): Promise<ISpecialResponse[]> {
    const queryParams = { ...params };
    
    // Convert DateTime objects to ISO strings
    if (params.searchDateTime && typeof params.searchDateTime === 'object' && 'toISO' in params.searchDateTime) {
      queryParams.searchDateTime = (params.searchDateTime as DateTime).toISO() || undefined;
    }
    
    return this.get<ISpecialResponse[]>(this.baseEndpoint, queryParams);
  }

  /**
   * Get a specific special by ID
   * [AllowAnonymous] endpoint
   */
  async getById(id: string): Promise<ISpecialResponse> {
    return this.get<ISpecialResponse>(`${this.baseEndpoint}/${id}`);
  }

  /**
   * Create a new special
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  async create(special: ISpecial): Promise<string> {
    const response = await this.post<{ id: string }>(this.baseEndpoint, special, true);
    return response.id;
  }

  /**
   * Update an existing special
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  async update(id: string, special: ISpecial): Promise<void> {
    await this.put<void>(`${this.baseEndpoint}/${id}`, special, true);
  }

  /**
   * Delete a special
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  async delete(id: string): Promise<void> {
    await super.delete(`${this.baseEndpoint}/${id}`, true);
  }
}