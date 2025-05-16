import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { User } from '@auth0/auth0-react';
import { useApiStore } from './api-store';

interface AuthState {
  isAuthenticated: boolean;
  user: User | null;
  token: string | null;
  setAuthState: (isAuthenticated: boolean, user: User | null, token: string | null) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => {
      return {
        isAuthenticated: false,
        user: null,
        token: null,
        setAuthState: (isAuthenticated: boolean, user: User | null, token: string | null) => {
          set({ isAuthenticated, user, token });
          useApiStore.getState().refreshClient(token);
        },
        logout: () => {
          set({ isAuthenticated: false, user: null, token: null });
          useApiStore.getState().refreshClient(null);
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
    }
  )
);
