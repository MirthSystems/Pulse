import { Box, CircularProgress } from '@mui/material';
import { VenueItemCard } from './venue-item-card';
import { useUiStore } from '../../store/ui-store';
import { VenueItem } from '../../models';

interface VenueListProps {
  venues: VenueItem[];
  isLoading: boolean;
}

export const VenueList = ({ venues, isLoading }: VenueListProps) => {
  const { openDeleteDialog } = useUiStore();

  if (isLoading) {
    return <Box display="flex" justifyContent="center" alignItems="center" minHeight={200}><CircularProgress /></Box>;
  }

  if (!venues.length) {
    return <Box p={2}>No venues found.</Box>;
  }

  return (
    <Box>
      {venues.map((venue) => (
        <VenueItemCard
          key={venue.id}
          venue={venue}
          onView={() => window.location.href = `/backoffice/venues/${venue.id}`}
          onDelete={() => openDeleteDialog('venue', venue.id)}
        />
      ))}
    </Box>
  );
};
