/**
 * API configuration for the Pulse API
 */
export interface ApiConfig {
  baseUrl: string;
  apiVersion: string;
  timeout: number;
}

/**
 * Default API configuration
 */
export const defaultApiConfig: ApiConfig = {
  baseUrl: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000',
  apiVersion: import.meta.env.VITE_API_VERSION || 'v1.0.0',
  timeout: 30000 // 30 seconds
};