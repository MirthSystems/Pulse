import { apiClient, ApiError } from '../client';

/**
 * Base service class that all resource-specific services extend
 * Provides common functionality for API interactions
 */
export abstract class BaseApiService {
  /**
   * Makes a GET request to the API
   * @param endpoint The API endpoint
   * @param queryParams Optional query parameters
   * @param requireAuth Whether the request requires authentication
   */
  protected async get<T>(endpoint: string, queryParams?: Record<string, unknown>, requireAuth: boolean = false): Promise<T> {
    try {
      return await apiClient.get<T>(endpoint, queryParams, requireAuth);
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  /**
   * Makes a POST request to the API
   * @param endpoint The API endpoint
   * @param data The data to send
   * @param requireAuth Whether the request requires authentication
   */
  protected async post<T>(endpoint: string, data?: unknown, requireAuth: boolean = false): Promise<T> {
    try {
      return await apiClient.post<T>(endpoint, data, requireAuth);
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  /**
   * Makes a PUT request to the API
   * @param endpoint The API endpoint
   * @param data The data to send
   * @param requireAuth Whether the request requires authentication
   */
  protected async put<T>(endpoint: string, data?: unknown, requireAuth: boolean = false): Promise<T> {
    try {
      return await apiClient.put<T>(endpoint, data, requireAuth);
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  /**
   * Makes a DELETE request to the API
   * @param endpoint The API endpoint
   * @param requireAuth Whether the request requires authentication
   */
  protected async delete(endpoint: string, requireAuth: boolean = false): Promise<unknown> {
    try {
      return await apiClient.delete(endpoint, requireAuth);
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  /**
   * Common error handling logic
   * Can be overridden by child classes for specific error handling
   */
  protected handleError(error: unknown): void {
    if (error instanceof ApiError) {
      // Log API errors with details
      console.error(`API Error [${error.status}]:`, error.message);
      
      // Additional handling could be added here
      // e.g., tracking, notifications, etc.
    } else {
      console.error('Unexpected error:', error);
    }
  }
}