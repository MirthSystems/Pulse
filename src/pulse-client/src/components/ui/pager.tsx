import { Pagination, Box } from '@mui/material';

export interface PagerProps {
  pagingInfo: {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
  };
  onPageChange: (page: number) => void;
  disabled?: boolean;
}

export const Pager = ({ pagingInfo, onPageChange, disabled }: PagerProps) => {
  const { currentPage, totalPages } = pagingInfo;
  if (totalPages <= 1) return null;
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', my: 2 }}>
      <Pagination
        count={totalPages}
        page={currentPage}
        onChange={(_e, page) => onPageChange(page)}
        color="primary"
        disabled={disabled}
        showFirstButton
        showLastButton
      />
    </Box>
  );
};
