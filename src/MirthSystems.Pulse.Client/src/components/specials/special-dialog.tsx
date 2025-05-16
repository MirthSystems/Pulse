import { Box, Button, Checkbox, Dialog, DialogActions, DialogContent, DialogTitle, FormControlLabel, Grid, MenuItem, TextField } from '@mui/material';
import { DateTime } from 'luxon';
import { useEffect, useState } from 'react';
import type { CreateSpecialRequest, UpdateSpecialRequest } from '../../models';
import { SpecialTypes, type SpecialItem } from '../../models/special';

interface SpecialDialogProps {
    open: boolean;
    onClose: () => void;
    onSave: (special: CreateSpecialRequest | UpdateSpecialRequest) => void;
    venueId: string;
    special?: SpecialItem | null;
}

interface SpecialFormState {
    id?: string;
    name: string;
    content: string;
    description?: string;
    type: SpecialTypes;
    price?: number;
    startDate: string;
    startTime: string;
    endDate?: string;
    endTimeString?: string;
    isRecurring: boolean;
    cronSchedule?: string;
    cronDescription?: string;
}

const initialFormState = (specialToEdit?: SpecialItem | null): SpecialFormState => {
    if (specialToEdit) {
        return {
            id: specialToEdit.id,
            name: '',
            content: specialToEdit.content || '',
            description: '',
            type: specialToEdit.type,
            price: undefined,
            startDate: specialToEdit.startDate.toISODate() || '',
            startTime: specialToEdit.startDate.toFormat('HH:mm') || '',
            endDate: specialToEdit.endDate?.toISODate() || undefined,
            endTimeString: specialToEdit.endDate?.toFormat('HH:mm') || undefined,
            isRecurring: false,
            cronSchedule: '',
            cronDescription: '',
        };
    }
    return {
        name: '',
        content: '',
        description: '',
        type: SpecialTypes.Food,
        price: undefined,
        startDate: DateTime.now().toISODate() || '',
        startTime: DateTime.now().toFormat('HH:mm') || '',
        endDate: undefined,
        endTimeString: undefined,
        isRecurring: false,
        cronSchedule: '',
        cronDescription: '',
    };
};

export const SpecialDialog = ({ open, onClose, onSave, venueId, special: specialToEdit }: SpecialDialogProps) => {
    const [formState, setFormState] = useState<SpecialFormState>(initialFormState(specialToEdit));

    useEffect(() => {
        setFormState(initialFormState(specialToEdit));
    }, [specialToEdit, open]);

    const handleChange = (field: keyof SpecialFormState, value: string | number | boolean | undefined | SpecialTypes) => {
        setFormState(prevState => ({ ...prevState, [field]: value }));
    };

    const handleSubmit = () => {
        const basePayload = {
            venueId,
            name: formState.name,
            content: formState.content,
            type: formState.type,
            startDate: formState.startDate,
            startTime: formState.startTime,
            expirationDate: formState.endDate,
            endTime: formState.endTimeString,
            isRecurring: formState.isRecurring,
            cronSchedule: formState.cronSchedule,
        };

        let payload: CreateSpecialRequest | UpdateSpecialRequest;
        if (specialToEdit && formState.id) {
            payload = {
                id: formState.id,
                ...basePayload,
            } as UpdateSpecialRequest;
        } else {
            payload = basePayload as CreateSpecialRequest;
        }
        onSave(payload);
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
            <DialogTitle>{specialToEdit ? 'Edit Special' : 'Create New Special'}</DialogTitle>
            <DialogContent>
                <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
                    <TextField
                        label="Name (for internal reference/future use)"
                        name="name"
                        value={formState.name}
                        onChange={e => handleChange('name', e.target.value)}
                        fullWidth
                    />
                    <TextField
                        label="Content / Title (Short, Publicly Visible)"
                        name="content"
                        value={formState.content}
                        onChange={e => handleChange('content', e.target.value)}
                        fullWidth
                        required
                    />
                    <TextField
                        label="Description (Optional, for internal reference/future use)"
                        name="description"
                        value={formState.description || ''}
                        onChange={e => handleChange('description', e.target.value)}
                        fullWidth
                        multiline
                        rows={3}
                    />
                    <TextField
                        select
                        label="Type"
                        name="type"
                        value={formState.type}
                        onChange={e => handleChange('type', Number(e.target.value) as SpecialTypes)}
                        fullWidth
                    >
                        {Object.entries(SpecialTypes).map(([key, value]) => (
                            typeof value === 'number' && <MenuItem key={key} value={value}>{key}</MenuItem>
                        ))}
                    </TextField>
                    <TextField
                        label="Price (Optional, for internal reference/future use)"
                        name="price"
                        type="number"
                        value={formState.price === undefined ? '' : formState.price}
                        onChange={e => handleChange('price', e.target.value === '' ? undefined : parseFloat(e.target.value))}
                        fullWidth
                        InputProps={{ inputProps: { min: 0, step: 0.01 } }}
                    />
                    <Grid container spacing={2}>
                        <Grid size={{ xs: 12, sm: 6 }}>
                            <TextField
                                label="Start Date"
                                name="startDate"
                                type="date"
                                value={formState.startDate}
                                onChange={e => handleChange('startDate', e.target.value)}
                                InputLabelProps={{ shrink: true }}
                                fullWidth
                                required
                            />
                        </Grid>
                        <Grid size={{ xs: 12, sm: 6 }}>
                            <TextField
                                label="Start Time"
                                name="startTime"
                                type="time"
                                value={formState.startTime}
                                onChange={e => handleChange('startTime', e.target.value)}
                                InputLabelProps={{ shrink: true }}
                                fullWidth
                                required
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid size={{ xs: 12, sm: 6 }}>
                            <TextField
                                label="End/Expiration Date (Optional)"
                                name="endDate"
                                type="date"
                                value={formState.endDate || ''}
                                onChange={e => handleChange('endDate', e.target.value || undefined)}
                                InputLabelProps={{ shrink: true }}
                                fullWidth
                            />
                        </Grid>
                        <Grid size={{ xs: 12, sm: 6 }}>
                            <TextField
                                label="End Time (Optional)"
                                name="endTimeString"
                                type="time"
                                value={formState.endTimeString || ''}
                                onChange={e => handleChange('endTimeString', e.target.value || undefined)}
                                InputLabelProps={{ shrink: true }}
                                fullWidth
                            />
                        </Grid>
                    </Grid>
                    <FormControlLabel
                        control={<Checkbox checked={formState.isRecurring} onChange={e => handleChange('isRecurring', e.target.checked)} />}
                        label="Is Recurring"
                    />
                    {formState.isRecurring && (
                        <>
                            <TextField
                                label="CRON Schedule (e.g., '0 18 * * MON-FRI')"
                                name="cronSchedule"
                                value={formState.cronSchedule || ''}
                                onChange={e => handleChange('cronSchedule', e.target.value)}
                                fullWidth
                            />
                            <TextField
                                label="CRON Description (Optional, for internal reference/future use)"
                                name="cronDescription"
                                value={formState.cronDescription || ''}
                                onChange={e => handleChange('cronDescription', e.target.value)}
                                fullWidth
                            />
                        </>
                    )}
                </Box>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose}>Cancel</Button>
                <Button onClick={handleSubmit} variant="contained">
                    {specialToEdit ? 'Save Changes' : 'Create Special'}
                </Button>
            </DialogActions>
        </Dialog>
    );
};
