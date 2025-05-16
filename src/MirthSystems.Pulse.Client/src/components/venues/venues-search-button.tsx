import { Button, Box } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';

interface VenuesSearchButtonProps {
  onClick: () => void;
  isLoading?: boolean;
}

export const VenuesSearchButton = ({ onClick, isLoading = false }: VenuesSearchButtonProps) => {
  return (
    <Box sx={{ height: '100%', display: 'flex', alignItems: 'center' }}>
      <Button
        variant="contained"
        color="primary"
        onClick={onClick}
        disabled={isLoading}
        startIcon={<SearchIcon />}
        fullWidth
        sx={{ height: '40px' }}
      >
        {isLoading ? 'Searching...' : 'Search'}
      </Button>
    </Box>
  );
};
