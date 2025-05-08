import { DateTime } from 'luxon';
import { ISpecial, ISpecialQueryParams, ISpecialResponse, Special } from '../types/models';
import { ApiService } from './ApiService';

export class SpecialsApiService extends ApiService {
  private readonly basePath = '/api/specials';

  async getSpecials(params: ISpecialQueryParams): Promise<ISpecialResponse[]> {
    const queryParams = { ...params };
    
    if (params.searchDateTime && typeof params.searchDateTime === 'object' && 'toISO' in params.searchDateTime) {
      queryParams.searchDateTime = (params.searchDateTime as DateTime).toISO() || undefined;
    }
    
    const response = await this.get<ISpecialResponse[]>(this.basePath, queryParams);
    return response;
  }

  async searchSpecials(
    address: string,
    options: {
      radius?: number;
      searchDateTime?: DateTime | string;
      specialTypeId?: number;
      page?: number;
      pageSize?: number;
    } = {}
  ): Promise<ISpecialResponse[]> {
    const searchParams: ISpecialQueryParams = {
      address,
      radius: options.radius,
      specialTypeId: options.specialTypeId,
      page: options.page,
      pageSize: options.pageSize
    };

    if (options.searchDateTime) {
      searchParams.searchDateTime = typeof options.searchDateTime === 'object' && 'toISO' in options.searchDateTime
        ? options.searchDateTime.toISO() || undefined
        : options.searchDateTime;
    }

    return this.getSpecials(searchParams);
  }

  async getSpecial(id: string): Promise<ISpecialResponse> {
    const endpoint = `${this.basePath}/${id}`;
    const response = await this.get<ISpecialResponse>(endpoint);
    return response;
  }

  async createSpecial(special: Special | ISpecial): Promise<string> {
    const request = special instanceof Special
      ? {
          content: special.content,
          type: special.type,
          startDate: special.startDate,
          startTime: special.startTime,
          endTime: special.endTime,
          expirationDate: special.expirationDate,
          isRecurring: special.isRecurring,
          cronSchedule: special.cronSchedule,
          venueId: special.venueId
        }
      : special;
    
    const response = await this.post<{ id: string }>(this.basePath, request);
    return response.id;
  }

  async updateSpecial(id: string, special: Special | ISpecial): Promise<void> {
    const endpoint = `${this.basePath}/${id}`;
    
    const request = special instanceof Special
      ? {
          content: special.content,
          type: special.type,
          startDate: special.startDate,
          startTime: special.startTime,
          endTime: special.endTime,
          expirationDate: special.expirationDate,
          isRecurring: special.isRecurring,
          cronSchedule: special.cronSchedule,
          venueId: special.venueId
        }
      : special;
    
    await this.put<void>(endpoint, request);
  }

  async deleteSpecial(id: string): Promise<void> {
    const endpoint = `${this.basePath}/${id}`;
    await this.delete<void>(endpoint);
  }
}