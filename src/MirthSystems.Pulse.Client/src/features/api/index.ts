// Export main client
export { PulseApiClient } from './client';

// Export models for easy access
export * from './types/models';

// Export enums
export * from './types/enums';

// Export services and errors
export { ApiError } from './services';

// Export config
export type { ApiConfig } from './config/apiConfig';

// Create and export default client instance
import { PulseApiClient } from './client';
export const apiClient = new PulseApiClient();
export default apiClient;