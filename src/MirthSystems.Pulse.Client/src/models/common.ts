export interface PagingInfo {
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface PagedResult<T> {
  items: T[];
  pagingInfo: PagingInfo;
}

export interface ApiError {
  status: number;
  message: string;
  errors?: Record<string, string[]>;
}
