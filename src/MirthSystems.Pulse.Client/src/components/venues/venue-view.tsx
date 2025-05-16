import { Avatar, Box, Card, CardContent, Divider, Grid, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { VenueItemExtended } from '../../models';
import { useVenuesStore } from '../../store/venues-store';
import { VenueSpecialsBoard } from '../specials/venue-specials-board';
import { EditButton } from '../ui/edit';
import { VenueBusinessHours } from './venue-business-hours';
import { VenueContactInformation } from './venue-contact-information';

export const VenueView = () => {
    const { id } = useParams<{ id: string }>();
    const { currentVenue, fetchVenueById, isLoading, updateVenue } = useVenuesStore();
    const [editData, setEditData] = useState<VenueItemExtended | null>(currentVenue);
    const [isEditing, setIsEditing] = useState(false);

    useEffect(() => {
        if (id) fetchVenueById(id);
    }, [id, fetchVenueById]);

    useEffect(() => {
        setEditData(currentVenue);
    }, [currentVenue]);

    const handleEdit = () => setIsEditing(true);
    const handleCancel = () => setIsEditing(false);

    const handleContactChange = (contactUpdates: Partial<Pick<VenueItemExtended, 'phoneNumber' | 'email' | 'website'>>) => {
        if (editData) {
            // Create a new VenueItemExtended with the updated properties
            const updatedVenue = new VenueItemExtended(
                editData.toModel() // Convert to model
            );

            // Update the properties
            if (contactUpdates.phoneNumber !== undefined) {
                updatedVenue.phoneNumber = contactUpdates.phoneNumber;
            }
            if (contactUpdates.email !== undefined) {
                updatedVenue.email = contactUpdates.email;
            }
            if (contactUpdates.website !== undefined) {
                updatedVenue.website = contactUpdates.website;
            }

            setEditData(updatedVenue);
        }
    };

    const handleSave = async () => {
        if (!editData || !id) return;
        await updateVenue(id, {
            name: editData.name,
            description: editData.description,
            phoneNumber: editData.phoneNumber,
            website: editData.website,
            email: editData.email,
            profileImage: editData.profileImage,
            address: {
                streetAddress: editData.streetAddress,
                secondaryAddress: editData.secondaryAddress,
                locality: editData.locality,
                region: editData.region,
                postcode: editData.postcode,
                country: editData.country,
            },
        });
        setIsEditing(false);
    };

    if (isLoading || !editData) return <Typography>Loading...</Typography>;

    return (
        <Box>
            <Box display="flex" justifyContent="flex-end" mb={2}>
                <EditButton
                    editing={isEditing}
                    onEdit={handleEdit}
                    onSave={handleSave}
                    onCancel={handleCancel}
                />
            </Box>
            {/* Top Section: Venue Info and Business Hours side by side */}
            <Grid container spacing={2} sx={{ mb: 4 }}>
                {/* Left: General Info + Contact Info */}
                <Grid sx={{ gridColumn: { xs: 'span 12', md: 'span 7' } }}>
                    <Card sx={{ mb: 2 }}>
                        <CardContent>
                            <Box display="flex" alignItems="center" gap={2}>
                                <Avatar src={editData.profileImage} sx={{ width: 64, height: 64 }} />
                                <Box>
                                    <Typography variant="h5">{editData.name}</Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        {editData.streetAddress}, {editData.secondaryAddress && `${editData.secondaryAddress}, `}{editData.locality}, {editData.region}, {editData.postcode}, {editData.country}
                                    </Typography>
                                </Box>
                            </Box>
                        </CardContent>
                    </Card>
                    <Card>
                        <CardContent>
                            <VenueContactInformation venue={editData} isEditing={isEditing} onChange={handleContactChange} />
                        </CardContent>
                    </Card>
                </Grid>
                {/* Right: Business Hours as vertical list */}
                <Grid sx={{ gridColumn: { xs: 'span 12', md: 'span 5' }, display: 'flex', flexDirection: 'column', justifyContent: 'flex-start' }}>
                    <Card sx={{ flex: 1 }}>
                        <CardContent>
                            <VenueBusinessHours venueId={id!} isEditing={isEditing} listView />
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <Divider sx={{ my: 2 }} />
            {/* Bottom Section: Specials Board */}
            <Box>
                <VenueSpecialsBoard venueId={id!} />
            </Box>
        </Box>
    );
};
