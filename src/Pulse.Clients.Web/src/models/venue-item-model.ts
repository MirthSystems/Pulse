/**
 * Represents a venue item.
 */
export interface VenueItem {
  id: number;
  name: string;
  description?: string;
  addressLine1: string;
  addressLine2?: string;
  addressLine3?: string;
  addressLine4?: string;
  locality: string;
  region: string;
  postcode: string;
  country: string;
  phoneNumber?: string;
  email?: string;
  website?: string;
  imageLink?: string;
  venueTypeId: number;
  venueTypeName?: string;
}
