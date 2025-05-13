import { VenueItem } from './venue';

export enum SpecialTypes {
  Food = 0,
  Drink = 1,
  Entertainment = 2,
}

export interface SpecialItem {
  id?: string;
  venueId: string;
  type: SpecialTypes;
  typeName: string;
  content: string;
  startDate: string;
  startTime: string;
  endTime?: string;
  isCurrentlyRunning: boolean;
  isRecurring: boolean;
}

export interface SpecialItemExtended extends SpecialItem {
  venue: VenueItem;
  expirationDate?: string;
  cronSchedule?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateSpecialRequest {
  venueId: string;
  content: string;
  type: SpecialTypes;
  startDate: string;
  startTime: string;
  endTime?: string;
  expirationDate?: string;
  isRecurring: boolean;
  cronSchedule?: string;
}

export interface UpdateSpecialRequest {
  content: string;
  type: SpecialTypes;
  startDate: string;
  startTime: string;
  endTime?: string;
  expirationDate?: string;
  isRecurring: boolean;
  cronSchedule?: string;
}

export interface GetSpecialsRequest {
  page: number;
  pageSize: number;
  address: string;
  radius: number;
  searchDateTime?: string;
  searchTerm?: string;
  venueId?: string;
  specialTypeId?: number;
  isCurrentlyRunning?: boolean;
}

export interface SpecialSearchParams {
  page: number;
  pageSize: number;
  address: string;
  radius: number;
  dateTime?: string; // Will map to searchDateTime in the API
  term?: string; // Will map to searchTerm in the API
  venueId?: string;
  type?: number; // Will map to specialTypeId in the API
  active?: boolean; // Will map to isCurrentlyRunning in the API
}

export interface SpecialMenu {
  items: SpecialItem[];
}

export interface SearchSpecialsResult {
  venue: VenueItem;
  specials: SpecialMenu;
}
