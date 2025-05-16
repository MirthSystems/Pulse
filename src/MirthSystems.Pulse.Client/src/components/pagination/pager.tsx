import { Pagination, Select, MenuItem, Box, Typography } from '@mui/material';

interface PagerProps {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  onPageChange: (page: number) => void;
  onPageSizeChange?: (size: number) => void;
}

export const Pager = ({
  currentPage,
  totalPages,
  pageSize,
  totalCount,
  onPageChange,
  onPageSizeChange,
}: PagerProps) => (
  <Box display="flex" alignItems="center" justifyContent="space-between" mt={2}>
    <Pagination
      count={totalPages}
      page={currentPage}
      onChange={(_, page) => onPageChange(page)}
      color="primary"
    />
    <Typography variant="body2" sx={{ mx: 2 }}>
      {`Total: ${totalCount}`}
    </Typography>
    {onPageSizeChange && (
      <Select
        value={pageSize}
        onChange={e => onPageSizeChange(Number(e.target.value))}
        size="small"
        sx={{ ml: 2 }}
      >
        {[10, 20, 50, 100, 200, 500, 1000].map(size => (
          <MenuItem key={size} value={size}>{size} / page</MenuItem>
        ))}
      </Select>
    )}
  </Box>
);
