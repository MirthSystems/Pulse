import { VenueType } from './venue-type';

/**
 * Search parameters for specials
 */
export type SpecialSearchParams = {
  location?: string;
  keyword?: string;
  venueType?: VenueType;
  tags?: string[];
};