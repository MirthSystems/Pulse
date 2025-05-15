import { Box } from '@mui/material';
import { type VenueItem } from '../../models';
import { VenueItemCard } from './venue-item-card';
import { PaginatedList } from '../ui/paginated-list';

interface VenueListProps {
  venues: VenueItem[];
  isLoading: boolean;
  error: string | null;
  pagingInfo: {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
  } | null;
  onPageChange: (page: number) => void;
}

export const VenueList = ({ 
  venues, 
  isLoading, 
  error, 
  pagingInfo, 
  onPageChange 
}: VenueListProps) => {  // Ensure pagingInfo has default values if null
  const safePagingInfo = pagingInfo || {
    currentPage: 1, 
    totalPages: 1, 
    totalCount: 0, 
    pageSize: 12, 
    hasPreviousPage: false, 
    hasNextPage: false 
  };

  return (
    <Box>
      <PaginatedList
        items={venues || []}
        pagingInfo={safePagingInfo}
        isLoading={isLoading}
        error={error}
        onPageChange={onPageChange}
        renderItem={(venue) => (
          <Box key={venue.id} sx={{ mb: 2 }}>
            <VenueItemCard
              venue={venue}
              onView={(id) => console.log(`View venue with id: ${id}`)}
              onDelete={(id) => console.log(`Delete venue with id: ${id}`)}
            />
          </Box>
        )}
        emptyMessage="No venues found. Try adjusting your search or filters."
      />
    </Box>
  );
};
