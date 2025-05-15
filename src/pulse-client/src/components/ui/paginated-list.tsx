import { Box, CircularProgress, Typography } from '@mui/material';
import { Pager } from './pager';

export interface PaginatedListProps<T> {
  items: T[];
  pagingInfo: {
    currentPage: number;
    totalPages: number;
    totalCount: number;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
  };
  isLoading?: boolean;
  error?: string | null;
  onPageChange: (page: number) => void;
  renderItem: (item: T, index: number) => React.ReactNode;
  emptyMessage?: string;
}

export function PaginatedList<T>({
  items,
  pagingInfo,
  isLoading,
  error,
  onPageChange,
  renderItem,
  emptyMessage = 'No results found.'
}: PaginatedListProps<T>) {
  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
        <CircularProgress />
      </Box>
    );
  }
  if (error) {
    return (
      <Typography color="error" align="center" sx={{ my: 4 }}>{error}</Typography>
    );
  }
  // Check if items is undefined or empty
  if (!items || items.length === 0) {
    return (
      <Typography align="center" sx={{ my: 4 }}>{emptyMessage}</Typography>
    );
  }return (
    <Box sx={{ width: '100%' }}>
      <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2, width: '100%' }}>
        {items.map((item, idx) => renderItem(item, idx))}
      </Box>      {pagingInfo && (
        <Pager
          pagingInfo={pagingInfo}
          onPageChange={onPageChange}
          disabled={isLoading}
        />
      )}
    </Box>
  );
}
