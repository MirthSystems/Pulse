import { useState, useEffect } from 'react';
import { Box, Paper } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useVenuesStore } from '../../store';
import { type GetVenuesRequest } from '../../models';
import { VenueListHeader } from './venue-list-header';
import { VenueList } from './venue-list';

interface VenueListViewProps {
  initialFilters?: GetVenuesRequest;
}

export const VenueListView = ({ initialFilters }: VenueListViewProps) => {
  const navigate = useNavigate();
  const { venues, isLoading, error, pagingInfo, fetchVenues } = useVenuesStore();  const [filters, setFilters] = useState<GetVenuesRequest>(initialFilters || {
    page: 1,
    pageSize: 12,
    searchText: '',
    address: '',
    radiusInMiles: 10,
    hasActiveSpecials: false,
    includeAddressDetails: false,
    includeBusinessHours: false,
    sortOrder: 0
  });

  useEffect(() => {
    fetchVenues(filters);
  }, [fetchVenues, filters]);

  const handleFilterChange = (newFilters: GetVenuesRequest) => {
    setFilters({
      ...filters,
      ...newFilters,
      page: 1, // Reset to the first page when filters change
    });
  };

  const handleAddNew = () => {
    navigate('/venues/new');
  };  return (
    <Box sx={{ width: '100%', py: 2 }}>
      <Paper sx={{ p: 3, width: '100%' }}>
        <VenueListHeader 
          onFilterChange={handleFilterChange} 
          onAddNew={handleAddNew}
          initialFilters={filters}
          totalCount={pagingInfo?.totalCount}
          isLoading={isLoading}
        />
        
        <Box sx={{ mt: 3, width: '100%' }}>
          <VenueList 
            venues={venues || []}
            isLoading={isLoading}
            error={error}
            pagingInfo={pagingInfo}
            onPageChange={(page) => setFilters({ ...filters, page })}
          />
        </Box>
      </Paper>
    </Box>
  );
};
