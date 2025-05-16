import { Box, TextField, Button, CircularProgress } from '@mui/material';
import { useState, useEffect, FormEvent } from 'react';
import { GetVenuesRequest } from '../../models';

interface VenueListFilterProps {
  onChange: (filters: Partial<GetVenuesRequest>) => void;
  initialFilters?: Partial<GetVenuesRequest>;
  isLoading?: boolean;
}

export const VenueListFilter = ({ onChange, initialFilters, isLoading = false }: VenueListFilterProps) => {
  const [filters, setFilters] = useState<Partial<GetVenuesRequest>>(initialFilters || {
    searchText: '',
    address: '',
    radiusInMiles: 10,
    hasActiveSpecials: false,
    includeAddressDetails: false,
    includeBusinessHours: false,
    sortOrder: 0
  });

  useEffect(() => {
    if (initialFilters) setFilters(initialFilters);
  }, [initialFilters]);

  const handleChange = (field: keyof GetVenuesRequest, value: string | number | boolean) => {
    const newFilters = { ...filters, [field]: value };
    setFilters(newFilters);
    onChange(newFilters);
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    onChange(filters);
  };

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ display: 'flex', gap: 2, mb: 2 }}>
      <TextField
        label="Search"
        value={filters.searchText || ''}
        onChange={e => handleChange('searchText', e.target.value)}
        size="small"
        disabled={isLoading}
      />
      <TextField
        label="Address"
        value={filters.address || ''}
        onChange={e => handleChange('address', e.target.value)}
        size="small"
        disabled={isLoading}
      />
      <Button type="submit" variant="contained" color="primary" disabled={isLoading}>
        {isLoading ? <CircularProgress size={20} /> : 'Filter'}
      </Button>
    </Box>
  );
};
