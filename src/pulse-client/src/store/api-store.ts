import { create } from 'zustand';
import { ApiClient } from '../api/client';

interface ApiState {
  apiClient: ApiClient;
  refreshClient: (token?: string | null) => void;
}

export const useApiStore = create<ApiState>()((set) => {
  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL as string;
  
  return {
    apiClient: new ApiClient(apiBaseUrl),
    refreshClient: (token) => set(() => ({
      apiClient: new ApiClient(apiBaseUrl, token || undefined)
    }))
  };
});
