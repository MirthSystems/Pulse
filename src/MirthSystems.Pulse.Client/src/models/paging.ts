export interface PagedResult<T> {
    items: T[];
    pagingInfo: PagingInfo;
}

export interface PageQueryParams {
    page?: number;
    pageSize?: number;
}

export class PagingInfo {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;

    constructor(data: {
        currentPage: number;
        pageSize: number;
        totalCount: number;
        totalPages: number;
        hasPreviousPage: boolean;
        hasNextPage: boolean;
    }) {
        this.currentPage = data.currentPage;
        this.pageSize = data.pageSize;
        this.totalCount = data.totalCount;
        this.totalPages = data.totalPages;
        this.hasPreviousPage = data.hasPreviousPage;
        this.hasNextPage = data.hasNextPage;
    }

    static fromResult<T>(result: PagedResult<T>): PagingInfo {
        return new PagingInfo(result.pagingInfo);
    }

    static default(pageSize = 10): PagingInfo {
        return new PagingInfo({
            currentPage: 1,
            pageSize,
            totalCount: 0,
            totalPages: 0,
            hasPreviousPage: false,
            hasNextPage: false
        });
    }
}