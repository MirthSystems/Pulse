import { Box, Typography, Button, Grid } from '@mui/material';
import { useParams } from 'react-router-dom';
import { useVenuesStore } from '../../store/venues-store';
import { useState, useEffect } from 'react';
import { VenueContactInformation } from './venue-contact-information';
import { VenueAddressComponent } from './venue-address-component';
import { VenueBusinessHours } from './venue-business-hours';
import { VenueSpecialsBoard } from '../specials/venue-specials-board';

export const VenueView = () => {
    const { id } = useParams<{ id: string }>();
    const { currentVenue, fetchVenueById, isLoading, updateVenue } = useVenuesStore();
    const [isEditing, setIsEditing] = useState(false);
    const [editData, setEditData] = useState(currentVenue);

    useEffect(() => {
        if (id) fetchVenueById(id);
    }, [id, fetchVenueById]);

    useEffect(() => {
        setEditData(currentVenue);
    }, [currentVenue]);

    const handleEdit = () => setIsEditing(true);
    const handleCancel = () => {
        setIsEditing(false);
        setEditData(currentVenue);
    };
    const handleSave = async () => {
        if (editData && id) {
            await updateVenue(id, {
                ...editData,
            });
            setIsEditing(false);
        }
    };

    if (isLoading || !editData) return <Typography>Loading...</Typography>;

    return (
        <Box>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h5">{editData.name}</Typography>
                {isEditing ? (
                    <Box>
                        <Button onClick={handleSave} variant="contained" color="primary" sx={{ mr: 1 }}>Save</Button>
                        <Button onClick={handleCancel} variant="outlined">Cancel</Button>
                    </Box>
                ) : (
                    <Button onClick={handleEdit} variant="contained">Edit</Button>
                )}
            </Box>
            <Grid container spacing={2}>
                <Grid item xs={12} md={4}>
                    <VenueContactInformation venue={editData} isEditing={isEditing} onChange={setEditData} />
                </Grid>
                <Grid item xs={12} md={4}>
                    <VenueAddressComponent venue={editData} isEditing={isEditing} onChange={setEditData} />
                </Grid>
                <Grid item xs={12} md={4}>
                    <VenueBusinessHours venueId={id!} isEditing={isEditing} />
                </Grid>
            </Grid>
            <Box mt={4}>
                <VenueSpecialsBoard venueId={id!} />
            </Box>
        </Box>
    );
};
