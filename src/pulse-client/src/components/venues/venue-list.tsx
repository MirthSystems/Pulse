import { useEffect, useState } from 'react';
import { 
  Box, 
  Pagination, 
  CircularProgress, 
  Alert,
  Typography,
  useMediaQuery,
  useTheme
} from '@mui/material';
import { useVenuesStore } from '../../store';
import { type GetVenuesRequest } from '../../models';
import { VenueItemCard } from './venue-item-card';
import { VenueListHeader } from './venue-list-header';

export const VenueList = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
  const { venues, isLoading, error, pagingInfo, fetchVenues, deleteVenue } = useVenuesStore();
  const [filters, setFilters] = useState<GetVenuesRequest>({
    page: 1,
    pageSize: 12,
    searchText: '',
  });
  
  useEffect(() => {
    fetchVenues(filters);
  }, [fetchVenues, filters]);
  
  const handleFilterChange = (newFilters: GetVenuesRequest) => {
    // Keep the page size but reset to page 1
    setFilters({
      ...newFilters,
      pageSize: filters.pageSize,
      page: 1
    });
  };
  
  const handlePageChange = (_event: React.ChangeEvent<unknown>, page: number) => {
    setFilters(prev => ({ ...prev, page }));
  };
  
  const handleViewVenue = (id: string) => {
    // Here you would typically navigate to a venue details page
    console.log(`View venue with id: ${id}`);
  };
  
  const handleEditVenue = (id: string) => {
    // Here you would typically navigate to a venue edit page or open modal
    console.log(`Edit venue with id: ${id}`);
  };
  
  const handleDeleteVenue = async (id: string) => {
    const success = await deleteVenue(id);
    if (success) {
      // Refresh with current filters
      fetchVenues(filters);
    }
  };
  
  const handleAddNew = () => {
    // Here you would typically navigate to create venue page or open modal
    console.log('Add new venue clicked');
  };
  
  return (
    <Box>
      <VenueListHeader 
        onFilterChange={handleFilterChange} 
        onAddNew={handleAddNew}
        initialFilters={filters}
        totalCount={pagingInfo?.totalCount}
      />
      
      {isLoading && (
        <Box display="flex" justifyContent="center" my={4}>
          <CircularProgress />
        </Box>
      )}
      
      {error && (
        <Alert severity="error" sx={{ my: 2 }}>{error}</Alert>
      )}
      
      {!isLoading && !error && venues.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 8 }}>
          <Typography variant="h6" color="text.secondary">No venues found</Typography>
          <Typography variant="body2" color="text.secondary">
            Try adjusting your search or filters
          </Typography>
        </Box>
      )}
      
      {!isLoading && !error && venues.length > 0 && (
        <>
          <Box>
            {venues.map((venue) => (
              <VenueItemCard
                key={venue.id}
                venue={venue}
                onView={handleViewVenue}
                onEdit={handleEditVenue}
                onDelete={handleDeleteVenue}
              />
            ))}
          </Box>
          
          {pagingInfo && pagingInfo.totalPages > 1 && (
            <Box display="flex" justifyContent="center" mt={4}>
              <Pagination
                count={pagingInfo.totalPages}
                page={pagingInfo.currentPage}
                onChange={handlePageChange}
                color="primary"
                size={isMobile ? "small" : "medium"}
              />
            </Box>
          )}
        </>
      )}
    </Box>
  );
};
