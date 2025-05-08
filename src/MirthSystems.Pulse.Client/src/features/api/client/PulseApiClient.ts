import { ApiConfig, defaultApiConfig } from '../config/apiConfig';
import { OperatingScheduleService, SpecialService, VenueService } from '../services';

export class PulseApiClient {
  private config: ApiConfig;
  
  readonly venues: VenueService;
  readonly operatingSchedules: OperatingScheduleService;
  readonly specials: SpecialService;

  constructor(config: Partial<ApiConfig> = {}) {
    this.config = { ...defaultApiConfig, ...config };
    
    this.venues = new VenueService(this.config);
    this.operatingSchedules = new OperatingScheduleService(this.config);
    this.specials = new SpecialService(this.config);
  }

  setAuthToken(token: string): void {
    this.venues.setAuthToken(token);
    this.operatingSchedules.setAuthToken(token);
    this.specials.setAuthToken(token);
  }

  clearAuthToken(): void {
    this.venues.clearAuthToken();
    this.operatingSchedules.clearAuthToken();
    this.specials.clearAuthToken();
  }
}