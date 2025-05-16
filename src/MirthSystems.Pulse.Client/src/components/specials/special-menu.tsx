import PhoneIcon from '@mui/icons-material/Phone';
import WebIcon from '@mui/icons-material/Web';
import { Box, Chip, Divider, Grid, Paper, Typography } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { SearchSpecialsResult, SpecialItem, SpecialTypes } from '../../models';

interface SpecialMenuProps {
    searchResult: SearchSpecialsResult;
    // distance?: number; // If distance calculation is available
}

const getSpecialTypeLabel = (type: SpecialTypes): string => {
    return Object.keys(SpecialTypes)[Object.values(SpecialTypes).indexOf(type)] || 'Special';
}

export const SpecialMenu = ({ searchResult }: SpecialMenuProps) => {
    const { venue, specials } = searchResult;

    const specialsByType: Record<string, SpecialItem[]> = specials.items.reduce((acc, special) => {
        const typeLabel = getSpecialTypeLabel(special.type);
        if (!acc[typeLabel]) {
            acc[typeLabel] = [];
        }
        acc[typeLabel].push(special);
        return acc;
    }, {} as Record<string, SpecialItem[]>);


    return (
        <Paper elevation={2} sx={{ mb: 3, p: 2 }}>
            {/* Venue Header */}
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                {venue.profileImage && (
                    <Box
                        component="img"
                        src={venue.profileImage}
                        alt={`${venue.name} profile`}
                        sx={{ width: 60, height: 60, borderRadius: '50%', mr: 2, objectFit: 'cover' }}
                    />
                )}
                <Box>
                    <Typography
                        variant="h5"
                        component={RouterLink}
                        to={`/backoffice/venues/${venue.id}`}
                        sx={{ textDecoration: 'none', color: 'primary.main', '&:hover': { textDecoration: 'underline' } }}
                    >
                        {venue.name}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                        {venue.streetAddress}, {venue.locality}, {venue.region} {venue.postcode}
                    </Typography>
                    {/* {distance && <Typography variant="caption" color="text.secondary">~{distance.toFixed(1)} miles away</Typography>} */}
                </Box>
            </Box>
            <Grid container spacing={1} sx={{ mb: 1 }}>
                {venue.phoneNumber && (
                    <Grid sx={{ gridColumn: 'auto' }}>
                        <Chip icon={<PhoneIcon />} label={venue.phoneNumber} size="small" />
                    </Grid>
                )}
                {venue.website && (
                    <Grid sx={{ gridColumn: 'auto' }}>
                        <Chip icon={<WebIcon />} label={venue.website} size="small" component="a" href={venue.website} target="_blank" clickable />
                    </Grid>
                )}
            </Grid>

            <Divider sx={{ my: 2 }} />

            {/* Specials List */}
            {Object.entries(specialsByType).length > 0 ? (
                Object.entries(specialsByType).map(([type, items]) => (
                    <Box key={type} sx={{ mb: 2 }}>
                        <Typography variant="h6" gutterBottom sx={{ textTransform: 'capitalize' }}>{type} Specials</Typography>
                        {items.map((special) => (
                            <Paper key={special.id} variant="outlined" sx={{ p: 1.5, mb: 1, backgroundColor: 'background.default' }}>
                                <Typography variant="subtitle1" fontWeight="bold">{special.content}</Typography>
                                <Typography variant="body2" color="text.secondary">
                                    {special.startDate ? `Starts: ${special.startDate.toLocaleString()}` : 'Active Now'}
                                    {special.endDate && ` - Ends: ${special.endDate.toLocaleString()}`}
                                </Typography>
                            </Paper>
                        ))}
                    </Box>
                ))
            ) : (
                <Typography color="text.secondary">No active specials listed for this venue at the moment.</Typography>
            )}
        </Paper>
    );
};
