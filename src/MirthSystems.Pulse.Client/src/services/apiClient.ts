import axios, { AxiosInstance, AxiosRequestConfig } from 'axios';
import { useAuth0 } from '@auth0/auth0-react';
import { useEffect, useState } from 'react';

// Base API URL - you may want to put this in an environment variable
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || '/api';

// Create a standard axios instance for anonymous requests
export const anonymousClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Hook to get an authenticated API client
export function useApiClient() {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();
  const [apiClient, setApiClient] = useState<AxiosInstance>(anonymousClient);

  useEffect(() => {
    // Only attempt to get a token if the user is authenticated
    if (isAuthenticated) {
      const configureClient = async () => {
        try {
          const token = await getAccessTokenSilently();
          
          const client = axios.create({
            baseURL: API_BASE_URL,
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${token}`
            }
          });
          
          // Add response interceptor to handle common errors
          client.interceptors.response.use(
            response => response,
            error => {
              if (error.response) {
                // Log auth errors for debugging (you might remove this in production)
                if (error.response.status === 401) {
                  console.error('Authentication error:', error.response.data);
                }
              }
              return Promise.reject(error);
            }
          );
          
          setApiClient(client);
        } catch (error) {
          console.error('Error getting access token:', error);
          // Fall back to anonymous client if token acquisition fails
          setApiClient(anonymousClient);
        }
      };

      configureClient();
    } else {
      setApiClient(anonymousClient);
    }
  }, [getAccessTokenSilently, isAuthenticated]);

  return apiClient;
}
