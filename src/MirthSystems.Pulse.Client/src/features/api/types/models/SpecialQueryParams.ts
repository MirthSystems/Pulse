export interface ISpecialQueryParams {
  address: string;
  radius?: number;
  searchDateTime?: string;
  searchTerm?: string;
  venueId?: string;
  specialTypeId?: number;
  isCurrentlyRunning?: boolean;
  page?: number;
  pageSize?: number;
}