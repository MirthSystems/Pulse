import { Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField, Box, MenuItem } from '@mui/material';
import { useState } from 'react';
import { CreateSpecialRequest, SpecialTypes } from '../../models';

interface SpecialDialogProps {
    open: boolean;
    onClose: () => void;
    onSubmit: (special: CreateSpecialRequest) => void;
    venueId: string;
}

export const SpecialDialog = ({ open, onClose, onSubmit, venueId }: SpecialDialogProps) => {
    const [special, setSpecial] = useState<CreateSpecialRequest>({
        venueId,
        content: '',
        type: SpecialTypes.Food,
        startDate: '',
        startTime: '',
        isRecurring: false,
    });

    const handleChange = (field: keyof CreateSpecialRequest, value: string | number | boolean) => {
        setSpecial({ ...special, [field]: value });
    };

    const handleSubmit = () => {
        onSubmit(special);
    };

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>New Special</DialogTitle>
            <DialogContent>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
                    <TextField
                        label="Content"
                        value={special.content}
                        onChange={e => handleChange('content', e.target.value)}
                        fullWidth
                        required
                    />
                    <TextField
                        select
                        label="Type"
                        value={special.type}
                        onChange={e => handleChange('type', Number(e.target.value))}
                        fullWidth
                    >
                        <MenuItem value={SpecialTypes.Food}>Food</MenuItem>
                        <MenuItem value={SpecialTypes.Drink}>Drink</MenuItem>
                        <MenuItem value={SpecialTypes.Entertainment}>Entertainment</MenuItem>
                    </TextField>
                    {/* Add more fields for startDate, startTime, etc. */}
                </Box>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose}>Cancel</Button>
                <Button onClick={handleSubmit} variant="contained">Create</Button>
            </DialogActions>
        </Dialog>
    );
};
