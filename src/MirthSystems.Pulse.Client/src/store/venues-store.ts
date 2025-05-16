import { create } from 'zustand';
import { useApiStore } from './api-store';
import {
  VenueItem,
  VenueItemExtended,
  type GetVenuesRequest,
  type CreateVenueRequest,
  type UpdateVenueRequest
} from '../models';
import { PagingInfo } from '../models/paging';

interface VenuesState {
  venues: VenueItem[];
  currentVenue: VenueItemExtended | null;
  isLoading: boolean;
  error: string | null;
  pagingInfo: PagingInfo;
  filters: Partial<GetVenuesRequest>;
  sort: string | null;
  lastRequest: GetVenuesRequest | null;

  fetchVenues: () => Promise<void>;
  refreshVenues: () => Promise<void>;
  setPage: (page: number) => void;
  setPageSize: (size: number) => void;
  setFilters: (filters: Partial<GetVenuesRequest>) => void;
  setSort: (sort: string | null) => void;
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
  pagingInfo: PagingInfo.default(),
  filters: {},
  sort: null,
  lastRequest: null,

  fetchVenues: async () => {
    try {
      set({ isLoading: true, error: null });
      const apiClient = useApiStore.getState().apiClient!;
      const { filters, sort, pagingInfo } = get();
      const request: GetVenuesRequest = {
        ...filters,
        page: pagingInfo.currentPage,
        pageSize: pagingInfo.pageSize,
        sort: sort || undefined
      };
      const result = await apiClient.venues.getVenues(request);
      set({
        venues: result.items,
        pagingInfo: PagingInfo.fromResult(result),
        isLoading: false,
        lastRequest: request
      });
    } catch (error) {
      set({
        error: error instanceof Error ? error.message : 'Failed to fetch venues',
        isLoading: false
      });
    }
  },

  refreshVenues: async () => {
    await get().fetchVenues();
  },

  setPage: (page: number) => {
    set(state => ({
      pagingInfo: new PagingInfo({ ...state.pagingInfo, currentPage: page })
    }));
    get().fetchVenues();
  },

  setPageSize: (size: number) => {
    set(state => ({
      pagingInfo: new PagingInfo({ ...state.pagingInfo, pageSize: size, currentPage: 1 })
    }));
    get().fetchVenues();
  },

  setFilters: (filters: Partial<GetVenuesRequest>) => {
    set({ filters });
    set(state => ({
      pagingInfo: new PagingInfo({ ...state.pagingInfo, currentPage: 1 })
    }));
    get().fetchVenues();
  },

  setSort: (sort: string | null) => {
    set({ sort });
    set(state => ({
      pagingInfo: new PagingInfo({ ...state.pagingInfo, currentPage: 1 })
    }));
    get().fetchVenues();
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
      await get().refreshVenues();
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
      await get().refreshVenues();
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
      if (result) {
        set(state => ({
          venues: state.venues.filter(v => v.id !== id),
          currentVenue: state.currentVenue && state.currentVenue.id === id ? null : state.currentVenue
        }));
        await get().refreshVenues();
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
