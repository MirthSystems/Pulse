export interface PagedResult<T> {
    items: T[];
    pagingInfo: {
        currentPage: number;
        pageSize: number;
        totalCount: number;
        totalPages: number;
        hasPreviousPage: boolean;
        hasNextPage: boolean;
    };
}