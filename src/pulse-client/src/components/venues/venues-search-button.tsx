import { Button } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';

interface VenuesSearchButtonProps {
  onClick: () => void;
  isLoading?: boolean;
}

export const VenuesSearchButton = ({ onClick, isLoading = false }: VenuesSearchButtonProps) => {
  return (
    <Button
      variant="contained"
      color="primary"
      onClick={onClick}
      disabled={isLoading}
      startIcon={<SearchIcon />}
      sx={{ height: '100%', minWidth: '100px' }}
    >
      {isLoading ? 'Searching...' : 'Search'}
    </Button>
  );
};
