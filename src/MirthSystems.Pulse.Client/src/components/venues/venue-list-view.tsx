import { Box, Typography, Button } from '@mui/material';
import { useVenuesStore } from '../../store/venues-store';
import { VenueListFilter } from './venue-list-filter';
import { VenueList } from './venue-list';
import { Pager } from '../pagination';
import { useUiStore } from '../../store/ui-store';
import { useEffect, useState } from 'react';

export const VenueListView = () => {
  const { venues, isLoading, pagingInfo, setPage, setPageSize, setFilters, fetchVenues } = useVenuesStore();
  const { openVenueDialog } = useUiStore();
  const [hasLoaded, setHasLoaded] = useState(false);

  useEffect(() => {
    fetchVenues();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (!hasLoaded) {
      fetchVenues();
      setHasLoaded(true);
    }
  }, [fetchVenues, hasLoaded]);

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
        <Typography variant="h5">Venues</Typography>
        <Button variant="contained" color="primary" onClick={() => openVenueDialog()}>New Venue</Button>
      </Box>
      <VenueListFilter onChange={setFilters} isLoading={isLoading} />
      <VenueList venues={venues} isLoading={isLoading} pagingInfo={pagingInfo} onPageChange={setPage} />
      <Pager
        currentPage={pagingInfo.currentPage}
        totalPages={pagingInfo.totalPages}
        pageSize={pagingInfo.pageSize}
        totalCount={pagingInfo.totalCount}
        onPageChange={setPage}
        onPageSizeChange={setPageSize}
      />
    </Box>
  );
};
