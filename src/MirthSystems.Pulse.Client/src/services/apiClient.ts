import axios, { AxiosError, AxiosInstance } from 'axios';
import { useAuth0 } from '@auth0/auth0-react';
import { ApiError } from '@models/common';

const API_URL = import.meta.env.VITE_API_SERVER_URL || 'https://localhost:7253';

// Base axios instance for public (non-authenticated) requests
const axiosInstance = axios.create({
  baseURL: `${API_URL}/api`,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Add response interceptor to handle errors consistently
axiosInstance.interceptors.response.use(
  response => response,
  (error: AxiosError) => {
    const apiError: ApiError = {
      status: error.response?.status || 500,
      message: 'An unexpected error occurred',
      errors: {}
    };

    // Handle different response formats from the backend
    if (error.response?.data) {
      const data = error.response.data as any;
      
      // Backend sometimes returns error.message, sometimes just a string
      if (typeof data === 'string') {
        apiError.message = data;
      } else if (data.message) {
        apiError.message = data.message;
      } else if (data.title) {
        // ASP.NET Core validation errors sometimes use 'title'
        apiError.message = data.title;
      }
      
      // Handle validation errors (ModelState)
      if (data.errors) {
        apiError.errors = data.errors;
      }
    }

    return Promise.reject(apiError);
  }
);

// Custom hook to create an authenticated API client
export const useApiClient = () => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();
  
  // Create a new request interceptor for authentication
  const requestInterceptor = axiosInstance.interceptors.request.use(
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

  // Clean up interceptor when the component unmounts
  // This is important to prevent memory leaks
  return axiosInstance;
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
