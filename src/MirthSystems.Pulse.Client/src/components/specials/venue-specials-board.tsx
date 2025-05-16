import { Box, Typography, CircularProgress, Button } from '@mui/material';
import { useEffect, useMemo } from 'react';
import { useSpecialsStore } from '../../store/specials-store';
import { useUiStore } from '../../store/ui-store';

interface VenueSpecialsBoardProps {
    venueId: string;
}

function groupSpecialsByTypeAndCron(specials: SpecialItem[]) {
    const groups: Record<string, Record<string, SpecialItem[]>> = {};
    specials.forEach(special => {
        const type = special.typeName;
        const cron = special.cronSchedule || 'No Schedule';
        if (!groups[type]) groups[type] = {};
        if (!groups[type][cron]) groups[type][cron] = [];
        groups[type][cron].push(special);
    });
    return groups;
}

export const VenueSpecialsBoard = ({ venueId }: VenueSpecialsBoardProps) => {
    const { venueSpecials, isLoading, fetchVenueSpecials } = useSpecialsStore();
    const { openSpecialDialog } = useUiStore();

    useEffect(() => {
        fetchVenueSpecials(venueId);
    }, [venueId, fetchVenueSpecials]);

    const grouped = useMemo(() => groupSpecialsByTypeAndCron(venueSpecials), [venueSpecials]);

    if (isLoading) return <CircularProgress />;

    return (
        <Box>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h6">Specials</Typography>
                <Button variant="contained" onClick={() => openSpecialDialog()}>Add Special</Button>
            </Box>
            {Object.entries(grouped).map(([type, crons]) => (
                <Box key={type} mb={2}>
                    <Typography variant="subtitle1">{type}</Typography>
                    {Object.entries(crons).map(([cron, specials]) => (
                        <Box key={cron} ml={2} mb={1}>
                            <Typography variant="caption">{cron}</Typography>
                            {specials.map(special => (
                                <Box key={special.id} p={1} border={1} borderRadius={1} mb={1}>
                                    <Typography>{special.content}</Typography>
                                </Box>
                            ))}
                        </Box>
                    ))}
                </Box>
            ))}
        </Box>
    );
};
