import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { ApiClient } from '../api/client';
import { User } from '@auth0/auth0-react';

interface AuthState {
  isAuthenticated: boolean;
  user: User | null;
  token: string | null;
  apiClient: ApiClient | null;
  setAuthState: (isAuthenticated: boolean, user: User | null, token: string | null) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => {
      const apiBaseUrl = import.meta.env.VITE_API_BASE_URL as string;
      return {
        isAuthenticated: false,
        user: null,
        token: null,
        apiClient: new ApiClient(apiBaseUrl),
        setAuthState: (isAuthenticated: boolean, user: User | null, token: string | null) => {
          const apiClient = new ApiClient(apiBaseUrl, token || undefined);
          set({ isAuthenticated, user, token, apiClient });
        },
        logout: () => {
          const apiClient = new ApiClient(apiBaseUrl);
          set({ isAuthenticated: false, user: null, token: null, apiClient });
        },
      };
    },
    {
      name: 'auth-storage',
      partialize: (state) => ({
        isAuthenticated: state.isAuthenticated,
        user: state.user,
        token: state.token,
      }),
      onRehydrateStorage: (state) => {
        if (state) {
          const apiBaseUrl = import.meta.env.VITE_API_BASE_URL as string;
          state.apiClient = new ApiClient(apiBaseUrl, state.token || undefined);
        }
      },
    }
  )
);
