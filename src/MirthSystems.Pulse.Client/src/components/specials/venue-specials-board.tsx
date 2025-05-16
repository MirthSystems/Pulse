import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { Alert, Box, Button, Card, CardActions, CardContent, Chip, CircularProgress, Grid, IconButton, Typography } from '@mui/material';
import { DateTime } from 'luxon';
import { useEffect, useMemo, useState } from 'react';
import type { CreateSpecialRequest, SpecialItem, UpdateSpecialRequest } from '../../models';
import { SpecialTypes } from '../../models';
import { useSpecialsStore } from '../../store/specials-store';
import { SpecialDialog } from './special-dialog';

interface VenueSpecialsBoardProps {
    venueId: string;
}

const getSpecialTypeDisplayName = (typeValue: SpecialTypes): string => {
    const entry = Object.entries(SpecialTypes).find(([, value]) => value === typeValue);
    return entry ? entry[0] : 'Unknown Type';
};

export const VenueSpecialsBoard = ({ venueId }: VenueSpecialsBoardProps) => {
    const {
        venueSpecials,
        fetchVenueSpecials,
        createSpecial,
        updateSpecial,
        deleteSpecial,
        isLoading,
        error,
    } = useSpecialsStore();

    const [isSpecialDialogOpen, setIsSpecialDialogOpen] = useState(false);
    const [editingSpecial, setEditingSpecial] = useState<SpecialItem | null>(null);

    useEffect(() => {
        if (venueId) {
            fetchVenueSpecials(venueId);
        }
    }, [venueId, fetchVenueSpecials]);

    const handleOpenCreateDialog = () => {
        setEditingSpecial(null);
        setIsSpecialDialogOpen(true);
    };

    const handleOpenEditDialog = (special: SpecialItem) => {
        setEditingSpecial(special);
        setIsSpecialDialogOpen(true);
    };

    const handleCloseDialog = () => {
        setIsSpecialDialogOpen(false);
        setEditingSpecial(null);
    };

    const handleSaveSpecial = async (payloadFromDialog: CreateSpecialRequest | UpdateSpecialRequest) => {
        let success = false;
        if (editingSpecial) {
            success = await updateSpecial(editingSpecial.id, payloadFromDialog as UpdateSpecialRequest);
        } else {
            const createPayload = payloadFromDialog as CreateSpecialRequest;
            if (!createPayload.venueId) {
                createPayload.venueId = venueId;
            }
            const createdId = await createSpecial(createPayload);
            success = !!createdId;
        }

        if (success) {
            handleCloseDialog();
            fetchVenueSpecials(venueId);
        }
    };

    const handleDeleteSpecial = async (specialId: string) => {
        const confirmed = window.confirm("Are you sure you want to delete this special?");
        if (confirmed) {
            const success = await deleteSpecial(specialId);
            if (success) {
                fetchVenueSpecials(venueId);
            }
        }
    };

    const groupedSpecials = useMemo(() => {
        if (!venueSpecials) return {};
        return venueSpecials.reduce((acc: Record<string, Record<string, SpecialItem[]>>, special: SpecialItem) => {
            const typeKey = getSpecialTypeDisplayName(special.type);
            if (!acc[typeKey]) {
                acc[typeKey] = {};
            }

            const scheduleKey = special.endDate ? `Ends: ${special.endDate.toFormat('DDD')}` : 'Ongoing / Ad-hoc';

            if (!acc[typeKey][scheduleKey]) {
                acc[typeKey][scheduleKey] = [];
            }
            acc[typeKey][scheduleKey].push(special);
            return acc;
        }, {} as Record<string, Record<string, SpecialItem[]>>);
    }, [venueSpecials]);

    if (isLoading && (!venueSpecials || venueSpecials.length === 0)) {
        return <CircularProgress />;
    }

    if (error) {
        return <Alert severity="error">Error loading specials: {error}</Alert>;
    }

    return (
        <Box>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                <Button
                    variant="contained"
                    startIcon={<AddCircleOutlineIcon />}
                    onClick={handleOpenCreateDialog}
                    sx={{ ml: 'auto' }}
                >
                    Add Special
                </Button>
            </Box>

            {Object.keys(groupedSpecials).length === 0 && !isLoading && (
                <Typography sx={{ textAlign: 'center', my: 3 }}>No specials found for this venue. Click "Add Special" to create one.</Typography>
            )}

            {Object.entries(groupedSpecials).map(([typeGroupName, scheduleGroups]) => (
                <Box key={typeGroupName} mb={3}>
                    <Typography variant="h5" gutterBottom>
                        {typeGroupName}
                    </Typography>
                    {Object.entries(scheduleGroups).map(([scheduleKey, specialItems]) => (
                        <Box key={scheduleKey} mb={2}>
                            <Typography variant="subtitle1" color="text.secondary" gutterBottom>
                                {scheduleKey}
                            </Typography>
                            <Grid container spacing={2}>
                                {specialItems.map((special: SpecialItem) => (
                                    <Grid key={special.id} size={{ xs: 12, sm: 6, md: 4, lg: 3 }}>
                                        <Card sx={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
                                            <CardContent sx={{ flexGrow: 1 }}>
                                                <Typography variant="h6" gutterBottom>{special.content}</Typography>
                                                <Chip label={getSpecialTypeDisplayName(special.type)} size="small" sx={{ mb: 1 }} />
                                                <Typography variant="body2" color="text.secondary">
                                                    Starts: {special.startDate.toLocaleString(DateTime.DATETIME_MED)}
                                                </Typography>
                                                {special.endDate && (
                                                    <Typography variant="body2" color="text.secondary">
                                                        Ends: {special.endDate.toLocaleString(DateTime.DATETIME_MED)}
                                                    </Typography>
                                                )}
                                            </CardContent>
                                            <CardActions sx={{ justifyContent: 'flex-end' }}>
                                                <IconButton onClick={() => handleOpenEditDialog(special)} size="small" title="Edit Special">
                                                    <EditIcon />
                                                </IconButton>
                                                <IconButton onClick={() => handleDeleteSpecial(special.id)} size="small" title="Delete Special">
                                                    <DeleteIcon />
                                                </IconButton>
                                            </CardActions>
                                        </Card>
                                    </Grid>
                                ))}
                            </Grid>
                        </Box>
                    ))}
                </Box>
            ))}

            {isSpecialDialogOpen && (
                <SpecialDialog
                    open={isSpecialDialogOpen}
                    onClose={handleCloseDialog}
                    onSave={handleSaveSpecial}
                    special={editingSpecial}
                    venueId={venueId}
                />
            )}
        </Box>
    );
};
