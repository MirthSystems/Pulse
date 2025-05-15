import { create } from 'zustand';
import { ApiClient } from '../api/client';

interface ApiState {
  apiClient: ApiClient; // This is non-nullable
  refreshClient: (token?: string | null) => void;
}

export const useApiStore = create<ApiState>()((set) => {
  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL as string;
  
  return {
    apiClient: new ApiClient(apiBaseUrl), // Always initialized
    refreshClient: (token) => set(() => ({
      apiClient: new ApiClient(apiBaseUrl, token || undefined)
    }))
  };
});
