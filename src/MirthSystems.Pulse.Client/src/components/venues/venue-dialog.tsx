import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, TextField } from '@mui/material';
import { useState } from 'react';
import { type CreateVenueRequest } from '../../models';

interface VenueDialogProps {
    open: boolean;
    onClose: () => void;
    onSubmit: (venue: CreateVenueRequest) => void;
}

export const VenueDialog = ({ open, onClose, onSubmit }: VenueDialogProps) => {
    const [venue, setVenue] = useState<CreateVenueRequest>({
        name: '',
        address: {
            streetAddress: '',
            locality: '',
            region: '',
            postcode: '',
            country: '',
        },
        hoursOfOperation: [],
    });

    const handleChange = (field: keyof CreateVenueRequest, value: string) => {
        setVenue({ ...venue, [field]: value });
    };

    const handleSubmit = () => {
        onSubmit(venue);
    };

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>New Venue</DialogTitle>
            <DialogContent>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
                    <TextField
                        label="Venue Name"
                        value={venue.name}
                        onChange={e => handleChange('name', e.target.value)}
                        fullWidth
                        required
                    />
                    {/* Add more fields for address, etc. */}
                </Box>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose}>Cancel</Button>
                <Button onClick={handleSubmit} variant="contained">Create</Button>
            </DialogActions>
        </Dialog>
    );
};
