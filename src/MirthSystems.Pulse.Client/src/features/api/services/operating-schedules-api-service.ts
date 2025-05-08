import { BaseApiService } from './base-api-service';
import { IOperatingSchedule, IOperatingScheduleResponse } from '../types/models';

/**
 * Service for managing operating schedule-related API endpoints
 */
export class OperatingSchedulesApiService extends BaseApiService {
  private readonly baseEndpoint = '/api/operating-schedules';

  /**
   * Get operating schedules for a venue
   * [AllowAnonymous] endpoint
   */
  async getVenueOperatingSchedules(venueId: string): Promise<IOperatingScheduleResponse[]> {
    return this.get<IOperatingScheduleResponse[]>(`${this.baseEndpoint}/venue/${venueId}`);
  }

  /**
   * Get a specific operating schedule by ID
   * [AllowAnonymous] endpoint
   */
  async getById(id: string): Promise<IOperatingScheduleResponse> {
    return this.get<IOperatingScheduleResponse>(`${this.baseEndpoint}/${id}`);
  }

  /**
   * Create a new operating schedule
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  async create(schedule: IOperatingSchedule): Promise<string> {
    const response = await this.post<{ id: string }>(this.baseEndpoint, schedule, true);
    return response.id;
  }

  /**
   * Update an existing operating schedule
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  async update(id: string, schedule: IOperatingSchedule): Promise<void> {
    await this.put<void>(`${this.baseEndpoint}/${id}`, schedule, true);
  }

  /**
   * Delete an operating schedule
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  async delete(id: string): Promise<void> {
    await super.delete(`${this.baseEndpoint}/${id}`, true);
  }

  /**
   * Post multiple business hours for a venue
   * [Authorize(Roles = "Content.Manager,System.Administrator")] endpoint
   */
  async postVenueBusinessHours(venueId: string, schedules: IOperatingSchedule[]): Promise<string[]> {
    const response = await this.post<{ ids: string[] }>(`${this.baseEndpoint}/venue/${venueId}`, schedules, true);
    return response.ids;
  }
}