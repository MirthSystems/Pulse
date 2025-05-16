import { Avatar, Box, Card, CardContent, Container, Divider, Grid, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { VenueSpecialsBoard } from '../components/specials/venue-specials-board';
import { EditButton } from '../components/ui/edit';
import { VenueAddressComponent, VenueBusinessHours, VenueContactInformation } from '../components/venues';
import type { UpdateVenueRequest } from '../models';
import { VenueItemExtended, type VenueItemExtendedModel } from '../models';
import { useVenuesStore } from '../store/venues-store';

export const VenuePage = () => {
    const { id } = useParams<{ id: string }>();
    const { currentVenue, fetchVenueById, isLoading, updateVenue, error } = useVenuesStore();
    const [editData, setEditData] = useState<VenueItemExtended | null>(null);
    const [isEditing, setIsEditing] = useState(false);

    useEffect(() => {
        if (id) {
            fetchVenueById(id);
        }
    }, [id, fetchVenueById]);

    useEffect(() => {
        if (currentVenue) {
            if (currentVenue instanceof VenueItemExtended && typeof currentVenue.toModel === 'function') {
                setEditData(new VenueItemExtended(currentVenue.toModel()));
            } else {
                console.warn("currentVenue is not an instance of VenueItemExtended or toModel is missing. Attempting to construct from model.");
                setEditData(new VenueItemExtended(currentVenue as unknown as VenueItemExtendedModel));
            }
        } else {
            setEditData(null);
        }
    }, [currentVenue]);

    const handleEdit = () => setIsEditing(true);

    const handleCancel = () => {
        setIsEditing(false);
        if (currentVenue) {
            if (currentVenue instanceof VenueItemExtended && typeof currentVenue.toModel === 'function') {
                setEditData(new VenueItemExtended(currentVenue.toModel()));
            } else {
                setEditData(new VenueItemExtended(currentVenue as unknown as VenueItemExtendedModel));
            }
        }
    };

    const handleSave = async () => {
        if (!editData || !id) return;
        if (!(editData instanceof VenueItemExtended) || typeof editData.toModel !== 'function') {
            console.error("editData is not a full VenueItemExtended instance with toModel method.");
            return;
        }
        const modelToSave = editData.toModel();

        const updateRequest: UpdateVenueRequest = {
            name: modelToSave.name,
            description: modelToSave.description,
            phoneNumber: modelToSave.phoneNumber,
            website: modelToSave.website,
            email: modelToSave.email,
            profileImage: modelToSave.profileImage,
            address: {
                streetAddress: modelToSave.streetAddress,
                secondaryAddress: modelToSave.secondaryAddress,
                locality: modelToSave.locality,
                region: modelToSave.region,
                postcode: modelToSave.postcode,
                country: modelToSave.country,
            },
        };

        const success = await updateVenue(id, updateRequest);
        if (success) {
            setIsEditing(false);
            if (id) fetchVenueById(id);
        }
    };

    const handleContactChange = (updatedContactData: Partial<Pick<VenueItemExtendedModel, 'name' | 'description' | 'phoneNumber' | 'website' | 'email' | 'profileImage'>>) => {
        setEditData(prevData => {
            if (!prevData || !(prevData instanceof VenueItemExtended) || typeof prevData.toModel !== 'function') return prevData;
            const model = prevData.toModel();
            return new VenueItemExtended({ ...model, ...updatedContactData });
        });
    };

    const handleAddressChange = (addressUpdates: Partial<Pick<VenueItemExtendedModel, 'streetAddress' | 'secondaryAddress' | 'locality' | 'region' | 'postcode' | 'country'>>) => {
        setEditData(prevEditData => {
            if (!prevEditData || !(prevEditData instanceof VenueItemExtended) || typeof prevEditData.toModel !== 'function') return prevEditData;
            const currentModel = prevEditData.toModel();
            const newModelData: VenueItemExtendedModel = {
                ...currentModel,
                ...addressUpdates,
            };
            return new VenueItemExtended(newModelData);
        });
    };

    if (isLoading && !editData) return <Container><Typography>Loading venue details...</Typography></Container>;
    if (error && !editData) return <Container><Typography color="error">Error loading venue: {error}</Typography></Container>;
    if (!editData) return <Container><Typography>Venue not found.</Typography></Container>;

    return (
        <Container maxWidth="xl">
            <Box sx={{ mb: 3 }}>
                <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                    <Typography variant="h4" component="h1">
                        {editData.name} - Management
                    </Typography>
                    <EditButton
                        editing={isEditing}
                        onEdit={handleEdit}
                        onSave={handleSave}
                        onCancel={handleCancel}
                    />
                </Box>

                <Grid container spacing={3} sx={{ mb: 3 }}>
                    <Grid size={{ xs: 12, md: 7 }}>
                        <Card sx={{ mb: 2 }}>
                            <CardContent>
                                <Box display="flex" alignItems="center" gap={2} mb={2}>
                                    <Avatar src={editData.profileImage} sx={{ width: 80, height: 80 }} />
                                    <Box>
                                        <Typography variant="h5">{editData.name}</Typography>
                                        <Typography variant="body2" color="text.secondary">
                                            {editData.description || 'No description available.'}
                                        </Typography>
                                    </Box>
                                </Box>
                                <Divider sx={{ my: 2 }} />
                                <VenueContactInformation venue={editData} isEditing={isEditing} onChange={handleContactChange} />
                                <Divider sx={{ my: 2 }} />
                                <VenueAddressComponent venue={editData} isEditing={isEditing} onChange={handleAddressChange} />
                            </CardContent>
                        </Card>
                    </Grid>

                    <Grid size={{ xs: 12, md: 5 }}>
                        <Card sx={{ height: '100%' }}>
                            <CardContent>
                                {id && <VenueBusinessHours venueId={id} isEditing={isEditing} listView={false} />}
                            </CardContent>
                        </Card>
                    </Grid>
                </Grid>

                <Divider sx={{ my: 3 }} />

                <Box>
                    <Typography variant="h5" gutterBottom>Specials Management</Typography>
                    {id && <VenueSpecialsBoard venueId={id} />}
                </Box>
            </Box>
        </Container>
    );
};
