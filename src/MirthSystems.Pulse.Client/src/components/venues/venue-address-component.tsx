import { Box, TextField, Typography } from '@mui/material';
import { VenueItemExtended } from '../../models';

interface VenueAddressComponentProps {
  venue: VenueItemExtended;
  isEditing: boolean;
  onChange: (addressUpdates: Partial<Pick<VenueItemExtended, 'streetAddress' | 'secondaryAddress' | 'postcode' | 'locality' | 'region' | 'country'>>) => void;
}

export const VenueAddressComponent = ({ venue, isEditing, onChange }: VenueAddressComponentProps) => {
  const handleChange = (field: keyof Pick<VenueItemExtended, 'streetAddress' | 'secondaryAddress' | 'postcode' | 'locality' | 'region' | 'country'>, value: string) => {
    onChange({ [field]: value });
  };

  return (
    <Box>
      <Typography variant="h6">Address</Typography>
      {isEditing ? (
        <>
          <TextField label="Street Address" value={venue.streetAddress} onChange={e => handleChange('streetAddress', e.target.value)} fullWidth margin="dense" />
          <TextField label="Secondary Address" value={venue.secondaryAddress} onChange={e => handleChange('secondaryAddress', e.target.value)} fullWidth margin="dense" />
          <TextField label="Postcode" value={venue.postcode} onChange={e => handleChange('postcode', e.target.value)} fullWidth margin="dense" />
          <TextField label="Locality" value={venue.locality} onChange={e => handleChange('locality', e.target.value)} fullWidth margin="dense" />
          <TextField label="Region" value={venue.region} onChange={e => handleChange('region', e.target.value)} fullWidth margin="dense" />
          <TextField label="Country" value={venue.country} onChange={e => handleChange('country', e.target.value)} fullWidth margin="dense" />
        </>
      ) : (
        <>
          <Typography>{venue.streetAddress}</Typography>
          <Typography>{venue.secondaryAddress}</Typography>
          <Typography>{venue.postcode}, {venue.locality}, {venue.region}, {venue.country}</Typography>
        </>
      )}
    </Box>
  );
};
