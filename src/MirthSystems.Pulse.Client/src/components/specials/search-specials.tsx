import NightlifeIcon from '@mui/icons-material/Nightlife'; // Changed from ExploreIcon to NightlifeIcon
import SearchIcon from '@mui/icons-material/Search';
import { Box, Button, Paper, Typography, useTheme } from '@mui/material';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { SearchSpecialsFilter } from '../../components/specials/search-specials-filter';
import type { GetSpecialsRequest } from '../../models';
import { useSpecialsStore } from '../../store';

export const SearchSpecials = () => {
    const theme = useTheme();
    const navigate = useNavigate();
    const { setFilters: setSpecialsFilters } = useSpecialsStore();
    const [filters, setFilters] = useState<Partial<GetSpecialsRequest>>({
        address: '',
        radius: 10, // Default radius
        // searchDateTime will be set to now by default in the store/API if not provided
        searchTerm: '',
        isCurrentlyRunning: true, // Default to currently running specials
    });
    const [isLoading, setIsLoading] = useState(false); const handleFilterChange = (newFilters: Partial<GetSpecialsRequest>) => {
        setFilters(prevFilters => ({ ...prevFilters, ...newFilters }));
    };

    const handleSearch = async () => {
        setIsLoading(true);

        // Update the filters in the global store to be used by search results page
        setSpecialsFilters(filters);

        try {
            // Navigate to search results page where fetching will happen
            navigate('/search-results');
        } finally {
            // Ensure loading state is reset even if navigation fails
            setIsLoading(false);
        }
    };

    return (
        <Box sx={{ mb: 4 }}>
            <Box
                sx={{
                    textAlign: 'center',
                    mb: 3,
                    position: 'relative',
                    pt: 2,
                    '&:before': {
                        content: '""',
                        position: 'absolute',
                        top: -10,
                        left: '50%',
                        transform: 'translateX(-50%)',
                        width: 60,
                        height: 4,
                        borderRadius: 2,
                        backgroundColor: theme.palette.mode === 'light'
                            ? theme.palette.primary.main
                            : theme.palette.primary.light
                    }
                }}
            >
                <NightlifeIcon
                    sx={{
                        fontSize: 40,
                        mb: 1,
                        color: theme.palette.mode === 'light'
                            ? theme.palette.primary.main
                            : theme.palette.primary.light
                    }}
                />
                <Typography
                    variant="h3"
                    component="h1"
                    fontWeight="bold"
                    sx={{
                        mb: 1,
                        background: theme.palette.mode === 'light'
                            ? 'linear-gradient(90deg, #00C2CB 30%, #FF7E45 70%)'
                            : 'linear-gradient(90deg, #00E7F2 30%, #FF9255 70%)',
                        WebkitBackgroundClip: 'text',
                        WebkitTextFillColor: 'transparent',
                        textShadow: theme.palette.mode === 'light'
                            ? '0 0 20px rgba(0, 194, 203, 0.1)'
                            : '0 0 20px rgba(0, 231, 242, 0.2)'
                    }}
                >
                    Discover Special Events & Promotions
                </Typography>
                <Typography
                    variant="h6"
                    color="text.secondary"
                    sx={{
                        maxWidth: 700,
                        mx: 'auto',
                        fontWeight: 400,
                        letterSpacing: '0.01em'
                    }}
                >
                    Find the best deals, happy hours, and events near you
                </Typography>
            </Box>

            <Paper
                elevation={theme.palette.mode === 'light' ? 2 : 3}
                sx={{
                    p: 0,
                    borderRadius: 3,
                    overflow: 'hidden',
                    border: theme.palette.mode === 'dark' ? `1px solid rgba(${theme.palette.primary.main}, 0.1)` : 'none',
                    transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
                    '&:hover': {
                        transform: 'translateY(-4px)',
                        boxShadow: theme.palette.mode === 'light'
                            ? '0 8px 25px rgba(0, 194, 203, 0.15)'
                            : '0 8px 25px rgba(0, 0, 0, 0.25), 0 0 10px rgba(0, 231, 242, 0.15)'
                    }
                }}
            >
                <SearchSpecialsFilter
                    initialFilters={filters}
                    onFilterChange={handleFilterChange}
                    isLoading={isLoading}
                />
                <Box
                    sx={{
                        p: 2.5,
                        display: 'flex',
                        justifyContent: 'flex-end',
                        borderTop: '1px solid',
                        borderColor: theme.palette.mode === 'light'
                            ? 'rgba(0, 194, 203, 0.1)'
                            : 'rgba(0, 231, 242, 0.1)'
                    }}
                >
                    <Button
                        variant="contained"
                        color="primary"
                        startIcon={<SearchIcon />}
                        onClick={handleSearch}
                        disabled={isLoading}
                        size="large"
                        sx={{
                            minWidth: 140,
                            borderRadius: 2,
                            py: 1,
                            px: 3,
                            fontSize: '1rem'
                        }}
                    >
                        {isLoading ? 'Searching...' : 'Search'}
                    </Button>
                </Box>
            </Paper>
        </Box>
    );
};
