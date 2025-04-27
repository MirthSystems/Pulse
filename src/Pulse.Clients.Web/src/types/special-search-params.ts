import { VenueType } from './venue-type';

/**
 * Search parameters for specials
 */
export interface SpecialSearchParams {
  location?: string;
  keyword?: string;
  venueType?: VenueType;
  tags?: string[];
}