import { SpecialTypes } from "../enums";

export interface ISpecialQueryParams {
  address: string;
  radius?: number;
  searchDateTime?: string;
  searchTerm?: string;
  venueId?: string;
  specialTypeId?: number | SpecialTypes;
  isCurrentlyRunning?: boolean;
  page?: number;
  pageSize?: number;
}