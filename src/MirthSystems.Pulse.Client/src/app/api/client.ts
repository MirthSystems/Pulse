// API client configuration
const API_URL = import.meta.env.VITE_API_URL;

export interface ApiResponse<T = object> {
  data: T;
  error?: string;
  success: boolean;
}

/**
 * Base API client with common fetch configuration
 */
export const apiClient = {
  get: async <T>(endpoint: string): Promise<ApiResponse<T>> => {
    try {
      const token = localStorage.getItem('auth_token'); // This will be updated with Auth0 token later
      
      const response = await fetch(`${API_URL}${endpoint}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          ...(token ? { Authorization: `Bearer ${token}` } : {})
        }
      });
      
      const data = await response.json();
      
      return {
        data,
        success: response.ok
      };
    } catch (error) {
      return {
        data: null as unknown as T,
        error: error instanceof Error ? error.message : 'An unknown error occurred',
        success: false
      };
    }
  },
  
  post: async <T>(endpoint: string, body: object): Promise<ApiResponse<T>> => {
    try {
      const token = localStorage.getItem('auth_token'); // This will be updated with Auth0 token later
      
      const response = await fetch(`${API_URL}${endpoint}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          ...(token ? { Authorization: `Bearer ${token}` } : {})
        },
        body: JSON.stringify(body)
      });
      
      const data = await response.json();
      
      return {
        data,
        success: response.ok
      };
    } catch (error) {
      return {
        data: null as unknown as T,
        error: error instanceof Error ? error.message : 'An unknown error occurred',
        success: false
      };
    }
  },
  
  // Add other methods (PUT, DELETE, etc.) as needed
};