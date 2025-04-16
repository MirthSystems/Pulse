import type { SpecialItem } from './special-item-model';
import type { VenueItem } from './venue-item-model';

/**
 * Represents a special with its associated venue.
 */
export interface SpecialWithVenue extends SpecialItem {
  venue?: VenueItem;
}
