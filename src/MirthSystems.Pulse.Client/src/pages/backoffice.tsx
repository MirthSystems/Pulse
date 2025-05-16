import { Box, Button, Container, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { Pager } from '../components/pagination';
import { VenueList, VenueListFilter } from '../components/venues';
import { useUiStore } from '../store/ui-store';
import { useVenuesStore } from '../store/venues-store';

export const BackofficePage = () => {
  const { venues, isLoading, pagingInfo, filters, setPage, setPageSize, setFilters, fetchVenues } = useVenuesStore();
  const { openVenueDialog } = useUiStore();
  const [hasLoaded, setHasLoaded] = useState(false);

  useEffect(() => {
    fetchVenues();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (!hasLoaded) {
      setHasLoaded(true);
    } else {
      fetchVenues();
    }
  }, [fetchVenues, pagingInfo.currentPage, pagingInfo.pageSize, filters, hasLoaded]);

  return (
    <Container maxWidth="xl">
      <Box sx={{ mb: 3 }}>
        <Typography variant="h4" component="h1">
          Backoffice
        </Typography>
      </Box>
      <Box>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
          <Typography variant="h5">Venues</Typography>
          <Button variant="contained" color="primary" onClick={() => openVenueDialog()}>New Venue</Button>
        </Box>
        <VenueListFilter onChange={setFilters} isLoading={isLoading} initialFilters={filters} />
        <VenueList venues={venues} isLoading={isLoading} />
        <Pager
          currentPage={pagingInfo.currentPage}
          totalPages={pagingInfo.totalPages}
          pageSize={pagingInfo.pageSize}
          totalCount={pagingInfo.totalCount}
          onPageChange={setPage}
          onPageSizeChange={setPageSize}
        />
      </Box>
    </Container>
  );
};
