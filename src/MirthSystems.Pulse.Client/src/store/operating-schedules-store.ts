import { create } from 'zustand';
import { useApiStore } from './api-store';
import {
  OperatingScheduleItem,
  OperatingScheduleItemExtended,
  type CreateOperatingScheduleRequest,
  type UpdateOperatingScheduleRequest,
} from "../models";

interface OperatingSchedulesState {
  currentSchedule: OperatingScheduleItemExtended | null;
  venueSchedules: OperatingScheduleItem[];
  isLoading: boolean;
  error: string | null;
  
  fetchScheduleById: (id: string) => Promise<void>;
  fetchVenueBusinessHours: (venueId: string) => Promise<void>;
  createSchedule: (schedule: CreateOperatingScheduleRequest) => Promise<string | null>;
  updateSchedule: (id: string, schedule: UpdateOperatingScheduleRequest) => Promise<boolean>;
  deleteSchedule: (id: string) => Promise<boolean>;
  clearCurrentSchedule: () => void;
  setError: (error: string | null) => void;
}

export const useOperatingSchedulesStore = create<OperatingSchedulesState>()((set, get) => ({
  currentSchedule: null,
  venueSchedules: [],
  isLoading: false,
  error: null,
  
  fetchScheduleById: async (id: string) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const schedule = await apiClient.operatingSchedules.getOperatingScheduleById(id);
      set({ currentSchedule: schedule, isLoading: false });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch schedule details', 
        isLoading: false 
      });
    }
  },
  
  fetchVenueBusinessHours: async (venueId: string) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const schedules = await apiClient.venues.getVenueBusinessHours(venueId);
      set({ venueSchedules: schedules, isLoading: false });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch venue business hours', 
        isLoading: false 
      });
    }
  },
  
  createSchedule: async (schedule: CreateOperatingScheduleRequest) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const result = await apiClient.operatingSchedules.createOperatingSchedule(schedule);
      set({ isLoading: false });
      return result.id;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to create schedule', 
        isLoading: false 
      });
      return null;
    }
  },
  
  updateSchedule: async (id: string, schedule: UpdateOperatingScheduleRequest) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      await apiClient.operatingSchedules.updateOperatingSchedule(id, schedule);
      
      // Refresh the current schedule if we're updating the one we're viewing
      if (get().currentSchedule?.id === id) {
        const updatedSchedule = await apiClient.operatingSchedules.getOperatingScheduleById(id);
        set({ currentSchedule: updatedSchedule });
      }
      
      set({ isLoading: false });
      return true;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to update schedule', 
        isLoading: false 
      });
      return false;
    }
  },
  
  deleteSchedule: async (id: string) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const result = await apiClient.operatingSchedules.deleteOperatingSchedule(id);
      
      // Remove from local state if successful
      if (result) {
        set(state => ({
          venueSchedules: state.venueSchedules.filter(s => s.id !== id),
          currentSchedule: state.currentSchedule && state.currentSchedule.id === id ? null : state.currentSchedule
        }));
      }
      
      set({ isLoading: false });
      return result;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to delete schedule', 
        isLoading: false 
      });
      return false;
    }
  },
  
  clearCurrentSchedule: () => {
    set({ currentSchedule: null });
  },
  
  setError: (error) => {
    set({ error });
  }
}));
