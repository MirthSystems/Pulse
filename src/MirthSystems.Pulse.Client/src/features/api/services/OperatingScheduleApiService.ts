import { IOperatingSchedule, IOperatingScheduleResponse, OperatingSchedule } from '../types/models';
import { ApiService } from './ApiService';

/**
 * Service for managing venue operating schedules
 */
export class OperatingScheduleApiService extends ApiService {
  private readonly basePath = '/api/operating-schedules';

  async getVenueOperatingSchedules(venueId: string): Promise<IOperatingScheduleResponse[]> {
    const endpoint = `${this.basePath}/venue/${venueId}`;
    const response = await this.get<IOperatingScheduleResponse[]>(endpoint);
    return response;
  }

  async getOperatingSchedule(id: string): Promise<IOperatingScheduleResponse> {
    const endpoint = `${this.basePath}/${id}`;
    const response = await this.get<IOperatingScheduleResponse>(endpoint);
    return response;
  }

  async createOperatingSchedule(schedule: OperatingSchedule | IOperatingSchedule): Promise<string> {
    const request = schedule instanceof OperatingSchedule 
      ? { ...schedule }
      : schedule;
    
    const response = await this.post<{ id: string }>(this.basePath, request);
    return response.id;
  }

  async updateOperatingSchedule(id: string, schedule: OperatingSchedule | IOperatingSchedule): Promise<void> {
    const endpoint = `${this.basePath}/${id}`;
    const request = schedule instanceof OperatingSchedule
      ? { ...schedule }
      : schedule;
    
    await this.put<void>(endpoint, request);
  }

  async deleteOperatingSchedule(id: string): Promise<void> {
    const endpoint = `${this.basePath}/${id}`;
    await this.delete<void>(endpoint);
  }

  async postVenueBusinessHours(venueId: string, schedules: OperatingSchedule[] | IOperatingSchedule[]): Promise<string[]> {
    const endpoint = `${this.basePath}/venue/${venueId}`;
    
    const requests = schedules.map(schedule => 
      schedule instanceof OperatingSchedule ? { ...schedule } : schedule
    );
    
    const response = await this.post<{ ids: string[] }>(endpoint, requests);
    return response.ids;
  }
}