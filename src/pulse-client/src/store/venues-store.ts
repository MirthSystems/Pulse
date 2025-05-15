import { create } from 'zustand';
import { useApiStore } from './api-store';
import { 
  VenueItem, 
  VenueItemExtended, 
  type GetVenuesRequest,
  type CreateVenueRequest,
  type UpdateVenueRequest
} from '../models';

interface VenuesState {
  venues: VenueItem[];
  currentVenue: VenueItemExtended | null;
  isLoading: boolean;
  error: string | null;
  pagingInfo: {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
  } | null;
  
  // Actions
  fetchVenues: (request: GetVenuesRequest) => Promise<void>;
  fetchVenueById: (id: string) => Promise<void>;
  createVenue: (venue: CreateVenueRequest) => Promise<string | null>;
  updateVenue: (id: string, venue: UpdateVenueRequest) => Promise<boolean>;
  deleteVenue: (id: string) => Promise<boolean>;
  clearCurrentVenue: () => void;
  setError: (error: string | null) => void;
}

export const useVenuesStore = create<VenuesState>()((set, get) => ({
  venues: [],
  currentVenue: null,
  isLoading: false,
  error: null,
  pagingInfo: null,
  
  fetchVenues: async (request: GetVenuesRequest) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient!;
      const result = await apiClient.venues.getVenues(request);
      
      set({ 
        venues: result.items, 
        pagingInfo: result.pagingInfo,
        isLoading: false 
      });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch venues', 
        isLoading: false 
      });
    }
  },
  
  fetchVenueById: async (id: string) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient!;
      const venue = await apiClient.venues.getVenueById(id);
      set({ currentVenue: venue, isLoading: false });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch venue details', 
        isLoading: false 
      });
    }
  },
  
  createVenue: async (venue: CreateVenueRequest) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient!;
      const result = await apiClient.venues.createVenue(venue);
      set({ isLoading: false });
      return result.id;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to create venue', 
        isLoading: false 
      });
      return null;
    }
  },
  
  updateVenue: async (id: string, venue: UpdateVenueRequest) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient!;
      await apiClient.venues.updateVenue(id, venue);
      
      const currentVenue = get().currentVenue;
      if (currentVenue && currentVenue.id === id) {
        const updatedVenue = await apiClient.venues.getVenueById(id);
        set({ currentVenue: updatedVenue });
      }
      
      set({ isLoading: false });
      return true;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to update venue', 
        isLoading: false 
      });
      return false;
    }
  },
  
  deleteVenue: async (id: string) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient!;
      const result = await apiClient.venues.deleteVenue(id);
      
      // Remove from local state if successful
      if (result) {
        set(state => ({
          venues: state.venues.filter(v => v.id !== id),
          currentVenue: state.currentVenue && state.currentVenue.id === id ? null : state.currentVenue
        }));
      }
      
      set({ isLoading: false });
      return result;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to delete venue', 
        isLoading: false 
      });
      return false;
    }
  },
  
  clearCurrentVenue: () => {
    set({ currentVenue: null });
  },
  
  setError: (error) => {
    set({ error });
  }
}));
