import { Box, Typography, TextField } from '@mui/material';
import { VenueItemExtended } from '../../models';

interface VenueContactInformationProps {
  venue: VenueItemExtended;
  isEditing: boolean;
  onChange: (venue: VenueItemExtended) => void;
}

export const VenueContactInformation = ({ venue, isEditing, onChange }: VenueContactInformationProps) => {
  const handleChange = (field: keyof VenueItemExtended, value: string) => {
    onChange({ ...venue, [field]: value });
  };

  return (
    <Box>
      <Typography variant="h6">Contact Information</Typography>
      {isEditing ? (
        <>
          <TextField label="Phone" value={venue.phoneNumber} onChange={e => handleChange('phoneNumber', e.target.value)} fullWidth margin="dense" />
          <TextField label="Email" value={venue.email} onChange={e => handleChange('email', e.target.value)} fullWidth margin="dense" />
          <TextField label="Website" value={venue.website} onChange={e => handleChange('website', e.target.value)} fullWidth margin="dense" />
        </>
      ) : (
        <>
          <Typography>Phone: {venue.phoneNumber}</Typography>
          <Typography>Email: {venue.email}</Typography>
          <Typography>Website: {venue.website}</Typography>
        </>
      )}
    </Box>
  );
};
