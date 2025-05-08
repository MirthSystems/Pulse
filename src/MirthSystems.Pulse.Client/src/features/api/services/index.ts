import { VenuesApiService } from './venues-api-service';
import { SpecialsApiService } from './specials-api-service';
import { OperatingSchedulesApiService } from './operating-schedules-api-service';
import { BaseApiService } from './base-api-service';

// Export the service base class
export { BaseApiService };

// Create singleton instances of each service
export const venuesService = new VenuesApiService();
export const specialsService = new SpecialsApiService();
export const operatingSchedulesService = new OperatingSchedulesApiService();

// Types can be exported here as needed
export type { VenuesApiService as VenuesService, SpecialsApiService as SpecialsService, OperatingSchedulesApiService as OperatingSchedulesService };