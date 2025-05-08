import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { apiClient } from '..';

// This is a companion redux slice to track API state
// while using your existing API services implementation
interface ApiState {
  isLoading: { [key: string]: boolean };
  errors: { [key: string]: string | null };
  lastFetched: { [key: string]: number | null };
}

const initialState: ApiState = {
  isLoading: {},
  errors: {},
  lastFetched: {}
};

export const apiSlice = createSlice({
  name: 'api',
  initialState,
  reducers: {
    setLoading: (state, action: PayloadAction<{ key: string; isLoading: boolean }>) => {
      const { key, isLoading } = action.payload;
      state.isLoading[key] = isLoading;
    },
    setError: (state, action: PayloadAction<{ key: string; error: string | null }>) => {
      const { key, error } = action.payload;
      state.errors[key] = error;
    },
    setLastFetched: (state, action: PayloadAction<{ key: string; timestamp: number | null }>) => {
      const { key, timestamp } = action.payload;
      state.lastFetched[key] = timestamp;
    },
    clearErrors: (state) => {
      state.errors = {};
    },
    resetApiState: () => initialState
  }
});

export const { 
  setLoading, 
  setError, 
  setLastFetched, 
  clearErrors,
  resetApiState
} = apiSlice.actions;

export const apiReducer = apiSlice.reducer;
export const apiReducerPath = 'api';

// This middleware integrates with the existing apiClient
// to maintain consistent authentication state
export const apiMiddleware = () => next => action => {
  if (action.type === 'user/setToken') {
    const token = action.payload;
    if (token) {
      apiClient.setAuthToken(token);
    } else {
      apiClient.clearAuthToken();
    }
  }
  
  return next(action);
};