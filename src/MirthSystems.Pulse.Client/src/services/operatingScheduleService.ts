import { AxiosInstance } from 'axios';
import { 
  OperatingScheduleItemExtended,
  CreateOperatingScheduleRequest,
  UpdateOperatingScheduleRequest
} from '@models/operatingSchedule';

export const OperatingScheduleService = {
  // Get an operating schedule by ID
  getOperatingScheduleById: async (id: string, apiClient: AxiosInstance): Promise<OperatingScheduleItemExtended> => {
    const response = await apiClient.get(`/operating-schedules/${id}`);
    return response.data;
  },

  // Create a new operating schedule
  createOperatingSchedule: async (
    data: CreateOperatingScheduleRequest, 
    apiClient: AxiosInstance
  ): Promise<OperatingScheduleItemExtended> => {
    const response = await apiClient.post('/operating-schedules', data);
    return response.data;
  },

  // Update an existing operating schedule
  updateOperatingSchedule: async (
    id: string, 
    data: UpdateOperatingScheduleRequest, 
    apiClient: AxiosInstance
  ): Promise<OperatingScheduleItemExtended> => {
    const response = await apiClient.put(`/operating-schedules/${id}`, data);
    return response.data;
  },

  // Delete an operating schedule
  deleteOperatingSchedule: async (id: string, apiClient: AxiosInstance): Promise<boolean> => {
    const response = await apiClient.delete(`/operating-schedules/${id}`);
    return response.data;
  }
};
