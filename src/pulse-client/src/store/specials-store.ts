import { create } from 'zustand';
import { useApiStore } from './api-store';
import { 
  SpecialItem, 
  SpecialItemExtended, 
  SearchSpecialsResult,
  type GetSpecialsRequest,
  type CreateSpecialRequest,
  type UpdateSpecialRequest
} from '../models';

interface SpecialsState {
  searchResults: SearchSpecialsResult[];
  currentSpecial: SpecialItemExtended | null;
  venueSpecials: SpecialItem[];
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
  
  searchSpecials: (request: GetSpecialsRequest) => Promise<void>;
  fetchSpecialById: (id: string) => Promise<void>;
  fetchVenueSpecials: (venueId: string) => Promise<void>;
  createSpecial: (special: CreateSpecialRequest) => Promise<string | null>;
  updateSpecial: (id: string, special: UpdateSpecialRequest) => Promise<boolean>;
  deleteSpecial: (id: string) => Promise<boolean>;
  clearCurrentSpecial: () => void;
  setError: (error: string | null) => void;
}

export const useSpecialsStore = create<SpecialsState>()((set, get) => ({
  searchResults: [],
  currentSpecial: null,
  venueSpecials: [],
  isLoading: false,
  error: null,
  pagingInfo: null,
  
  searchSpecials: async (request: GetSpecialsRequest) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const result = await apiClient.specials.searchSpecials(request);
      
      set({ 
        searchResults: result.items, 
        pagingInfo: result.pagingInfo,
        isLoading: false 
      });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to search specials', 
        isLoading: false 
      });
    }
  },
  
  fetchSpecialById: async (id: string) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const special = await apiClient.specials.getSpecialById(id);
      set({ currentSpecial: special, isLoading: false });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch special details', 
        isLoading: false 
      });
    }
  },
  
  fetchVenueSpecials: async (venueId: string) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const specials = await apiClient.venues.getVenueSpecials(venueId);
      set({ venueSpecials: specials, isLoading: false });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch venue specials', 
        isLoading: false 
      });
    }
  },
  
  createSpecial: async (special: CreateSpecialRequest) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const result = await apiClient.specials.createSpecial(special);
      set({ isLoading: false });
      return result.id;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to create special', 
        isLoading: false 
      });
      return null;
    }
  },
  
  updateSpecial: async (id: string, special: UpdateSpecialRequest) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      await apiClient.specials.updateSpecial(id, special);
      
      if (get().currentSpecial?.id === id) {
        const updatedSpecial = await apiClient.specials.getSpecialById(id);
        set({ currentSpecial: updatedSpecial });
      }
      
      set({ isLoading: false });
      return true;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to update special', 
        isLoading: false 
      });
      return false;
    }
  },
  
  deleteSpecial: async (id: string) => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const result = await apiClient.specials.deleteSpecial(id);
      
      if (result) {
        set(state => ({
          venueSpecials: state.venueSpecials.filter(s => s.id !== id),
          currentSpecial: state.currentSpecial && state.currentSpecial.id === id ? null : state.currentSpecial
        }));
      }
      
      set({ isLoading: false });
      return result;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to delete special', 
        isLoading: false 
      });
      return false;
    }
  },
  
  clearCurrentSpecial: () => {
    set({ currentSpecial: null });
  },
  
  setError: (error) => {
    set({ error });
  }
}));
