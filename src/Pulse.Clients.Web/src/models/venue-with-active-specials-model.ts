import type { SpecialItem } from './special-item-model';
import type { VenueItem } from './venue-item-model';

/**
 * Represents a venue with active specials and distance information.
 */
export interface VenueWithActiveSpecials extends VenueItem {
  distanceMiles: number;
  activeSpecials: SpecialItem[];
}
