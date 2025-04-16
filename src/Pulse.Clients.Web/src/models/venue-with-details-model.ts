import type { OperatingScheduleItem } from './operating-schedule-item-model';
import type { SpecialItem } from './special-item-model';
import type { VenueItem } from './venue-item-model';

/**
 * Represents a venue with detailed information including business hours and specials.
 */
export interface VenueWithDetails extends VenueItem {
  businessHours: OperatingScheduleItem[];
  specials: SpecialItem[];
}
