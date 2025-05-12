import axios, { AxiosError, AxiosInstance, AxiosRequestConfig } from 'axios';
import { useAuth0 } from '@auth0/auth0-react';
import { ApiError } from '@models/common';

const API_URL = import.meta.env.VITE_API_SERVER_URL;

// Base axios instance
const axiosInstance = axios.create({
  baseURL: `${API_URL}/api`,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Custom hook to create an authenticated API client
export const useApiClient = () => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();
  
  const apiClient: AxiosInstance = axios.create({
    baseURL: `${API_URL}/api`,
    headers: {
      'Content-Type': 'application/json'
    }
  });
  
  // Request interceptor to add auth token
  apiClient.interceptors.request.use(
    async (config) => {
      if (isAuthenticated) {
        try {
          const token = await getAccessTokenSilently();
          if (token) {
            config.headers.Authorization = `Bearer ${token}`;
          }
        } catch (error) {
          console.error('Error getting access token:', error);
        }
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  // Response interceptor for error handling
  apiClient.interceptors.response.use(
    (response) => response,
    (error: AxiosError) => {
      const apiError: ApiError = {
        status: error.response?.status || 500,
        message: 'An unexpected error occurred',
        errors: {}
      };

      if (error.response?.data) {
        const data = error.response.data as any;
        if (data.message) {
          apiError.message = data.message;
        }
        if (data.errors) {
          apiError.errors = data.errors;
        }
      }

      return Promise.reject(apiError);
    }
  );

  return apiClient;
};

// Public API client (no auth required)
export const publicApiClient = axiosInstance;

// Helper function to create query string from params
export const createQueryString = (params: Record<string, any>): string => {
  const query = Object.entries(params)
    .filter(([_, value]) => value !== undefined && value !== null && value !== '')
    .map(([key, value]) => {
      if (typeof value === 'boolean') {
        return `${encodeURIComponent(key)}=${value}`;
      }
      return `${encodeURIComponent(key)}=${encodeURIComponent(value)}`;
    })
    .join('&');
    
  return query ? `?${query}` : '';
};
