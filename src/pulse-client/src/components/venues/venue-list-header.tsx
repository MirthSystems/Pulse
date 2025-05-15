import { Box, Button, Typography } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { VenueListFilter } from './venue-list-filter';
import { type GetVenuesRequest } from '../../models';

interface VenueListHeaderProps {
  onFilterChange: (filters: GetVenuesRequest) => void;
  onAddNew: () => void;
  initialFilters?: GetVenuesRequest;
  totalCount?: number;
  isLoading?: boolean;
}

export const VenueListHeader = ({ 
  onFilterChange, 
  onAddNew, 
  initialFilters,
  totalCount,
  isLoading = false
}: VenueListHeaderProps) => {
  return (
    <Box sx={{ mb: 3 }}>
      <Box 
        sx={{ 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center',
          mb: 2
        }}
      >
        <Typography variant="h5" component="h2">
          Venues
          {totalCount !== undefined && (
            <Typography 
              component="span" 
              variant="body2" 
              color="text.secondary"
              sx={{ ml: 1 }}
            >
              ({totalCount})
            </Typography>
          )}
        </Typography>
        
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={onAddNew}
        >
          New Venue
        </Button>
      </Box>
        <VenueListFilter 
        onChange={onFilterChange}
        initialFilters={initialFilters}
        isLoading={isLoading}
      />
    </Box>
  );
};
