// Export types and classes from api-client.ts
export { ApiClient, ApiError, type ApiConfig } from './api-client';

// Create and export singleton instance of the ApiClient
import { ApiClient } from './api-client';
export const apiClient = new ApiClient();