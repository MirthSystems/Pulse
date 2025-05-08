import { ApiConfig, defaultApiConfig } from '../config/apiConfig';
import { OperatingScheduleService, SpecialService, VenueService } from '../services';

/**
 * Main API client for Pulse API
 * Integrates all API services for easy access
 */
export class PulseApiClient {
  private config: ApiConfig;
  
  // Services
  readonly venues: VenueService;
  readonly operatingSchedules: OperatingScheduleService;
  readonly specials: SpecialService;

  constructor(config: Partial<ApiConfig> = {}) {
    this.config = { ...defaultApiConfig, ...config };
    
    // Initialize services
    this.venues = new VenueService(this.config);
    this.operatingSchedules = new OperatingScheduleService(this.config);
    this.specials = new SpecialService(this.config);
  }

  /**
   * Set authentication token for all services
   */
  setAuthToken(token: string): void {
    this.venues.setAuthToken(token);
    this.operatingSchedules.setAuthToken(token);
    this.specials.setAuthToken(token);
  }

  /**
   * Clear authentication token from all services
   */
  clearAuthToken(): void {
    this.venues.clearAuthToken();
    this.operatingSchedules.clearAuthToken();
    this.specials.clearAuthToken();
  }
}