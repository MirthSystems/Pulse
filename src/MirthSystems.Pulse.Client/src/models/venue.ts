import { OperatingScheduleItem } from './operatingSchedule';

export interface VenueItem {
  id?: string;
  name: string;
  description?: string;
  locality: string;
  region: string;
  profileImage?: string;
  latitude?: number;
  longitude?: number;
}

export interface VenueItemExtended extends VenueItem {
  phoneNumber: string;
  website: string;
  email: string;
  streetAddress: string;
  secondaryAddress: string;
  postcode: string;
  country: string;
  businessHours: OperatingScheduleItem[];
  createdAt: string;
  updatedAt?: string;
}

export interface AddressRequest {
  streetAddress: string;
  secondaryAddress?: string;
  locality: string;
  region: string;
  postcode: string;
  country: string;
}

export interface CreateVenueRequest {
  name: string;
  description?: string;
  phoneNumber?: string;
  website?: string;
  email?: string;
  profileImage?: string;
  address: AddressRequest;
  hoursOfOperation: OperatingHours[];
}

export interface UpdateVenueRequest {
  name: string;
  description?: string;
  phoneNumber?: string;
  website?: string;
  email?: string;
  profileImage?: string;
  address: AddressRequest;
}

export interface OperatingHours {
  dayOfWeek: number;
  timeOfOpen: string;
  timeOfClose: string;
  isClosed: boolean;
}

export interface GetVenuesRequest {
  page: number;
  pageSize: number;
  searchText?: string;
  address?: string;
  radiusInMiles?: number;
  openOnDayOfWeek?: number;
  timeOfDay?: string;
  hasActiveSpecials?: boolean;
  specialTypeId?: number;
  includeAddressDetails: boolean;
  includeBusinessHours: boolean;
  sortOrder: number;
}

export interface VenueSearchParams {
  page: number;
  pageSize: number;
  searchText?: string;
  address?: string;
  radius?: number;
  openDay?: number;
  openTime?: string;
  hasSpecials?: boolean;
  specialType?: number;
  includeAddress?: boolean;
  includeHours?: boolean;
  sort?: number;
}
