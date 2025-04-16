import type { SpecialItem } from './special-item-model';
import type { TagItem } from './tag-item-model';
import type { VenueItem } from './venue-item-model';

/**
 * Represents a special with detailed information including tags and venue details.
 */
export interface SpecialWithDetails extends SpecialItem {
  tags: TagItem[];
  venue?: VenueItem;
}
