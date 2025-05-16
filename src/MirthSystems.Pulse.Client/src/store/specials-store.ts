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
import { PagingInfo } from '../models/paging';

interface SpecialsState {
  searchResults: SearchSpecialsResult[];
  currentSpecial: SpecialItemExtended | null;
  venueSpecials: SpecialItem[];
  isLoading: boolean;
  error: string | null;
  pagingInfo: PagingInfo;
  filters: Partial<GetSpecialsRequest>;
  sort: string | null;
  lastRequest: GetSpecialsRequest | null;

  searchSpecials: () => Promise<void>;
  setPage: (page: number) => void;
  setPageSize: (size: number) => void;
  setFilters: (filters: Partial<GetSpecialsRequest>) => void;
  setSort: (sort: string | null) => void;
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
  pagingInfo: PagingInfo.default(),
  filters: {},
  sort: null,
  lastRequest: null,

  searchSpecials: async () => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient;
      const { filters, sort, pagingInfo } = get();
      const request: GetSpecialsRequest = {
        ...filters,
        page: pagingInfo.currentPage,
        pageSize: pagingInfo.pageSize,
        sort: sort || undefined
      };
      const result = await apiClient.specials.searchSpecials(request);
      set({ 
        searchResults: result.items, 
        pagingInfo: PagingInfo.fromResult(result),
        isLoading: false,
        lastRequest: request
      });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to search specials', 
        isLoading: false 
      });
    }
  },

  setPage: (page: number) => {
    set(state => ({
      pagingInfo: new PagingInfo({ ...state.pagingInfo, currentPage: page })
    }));
    get().searchSpecials();
  },

  setPageSize: (size: number) => {
    set(state => ({
      pagingInfo: new PagingInfo({ ...state.pagingInfo, pageSize: size, currentPage: 1 })
    }));
    get().searchSpecials();
  },

  setFilters: (filters: Partial<GetSpecialsRequest>) => {
    set({ filters });
    set(state => ({
      pagingInfo: new PagingInfo({ ...state.pagingInfo, currentPage: 1 })
    }));
    get().searchSpecials();
  },

  setSort: (sort: string | null) => {
    set({ sort });
    set(state => ({
      pagingInfo: new PagingInfo({ ...state.pagingInfo, currentPage: 1 })
    }));
    get().searchSpecials();
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
