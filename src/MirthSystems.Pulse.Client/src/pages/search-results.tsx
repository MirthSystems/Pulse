import NightlifeIcon from '@mui/icons-material/Nightlife';
import SearchOffIcon from '@mui/icons-material/SearchOff';
import { Box, CircularProgress, Container, Paper, Typography, useTheme } from '@mui/material';
import { useEffect } from 'react';
import { Pager } from '../components/pagination';
import { SearchSpecials } from '../components/specials/search-specials';
import { SpecialMenu } from '../components/specials/special-menu';
import { useSpecialsStore } from '../store';

export const SearchResultsPage = () => {
    const theme = useTheme();
    const {
        searchResults,
        isLoading,
        error,
        pagingInfo,
        searchSpecials: fetchSpecials,
        setPage,
        setPageSize,
        // filters are already in the store, set by SearchSpecials component before navigation
    } = useSpecialsStore();

    useEffect(() => {
        // Fetch specials when the component mounts or when paging/filters change (implicitly via store)
        // The filters are expected to be in the store already from the SearchSpecials component.
        fetchSpecials();
    }, [fetchSpecials, pagingInfo.currentPage, pagingInfo.pageSize]); // Re-fetch if page or pageSize changes

    const handlePageChange = (newPage: number) => {
        setPage(newPage);
        // fetchSpecials will be called by the useEffect above
    };

    const handlePageSizeChange = (newPageSize: number) => {
        setPageSize(newPageSize);
        // fetchSpecials will be called by the useEffect above
    };

    // Calculate totalPages
    const totalPages = Math.ceil(pagingInfo.totalCount / pagingInfo.pageSize) || 1;

    return (
        <Container maxWidth="lg">
            <Box sx={{ my: { xs: 2, md: 4 } }}>
                <SearchSpecials /> {/* Render the search/filter bar again for refinement */}

                <Box sx={{
                    display: 'flex',
                    alignItems: 'center',
                    mt: 4,
                    mb: 2,
                    position: 'relative',
                    '&:after': {
                        content: '""',
                        position: 'absolute',
                        bottom: -8,
                        left: 0,
                        width: 100,
                        height: 3,
                        borderRadius: 1.5,
                        backgroundColor: theme.palette.mode === 'light'
                            ? theme.palette.primary.main
                            : theme.palette.primary.light
                    }
                }}>
                    <NightlifeIcon
                        sx={{
                            mr: 1,
                            color: theme.palette.mode === 'light'
                                ? theme.palette.primary.main
                                : theme.palette.primary.light
                        }}
                    />
                    <Typography
                        variant="h4"
                        component="h1"
                        fontWeight="bold"
                        sx={{
                            background: theme.palette.mode === 'light'
                                ? 'linear-gradient(90deg, #00C2CB 30%, #FF7E45 70%)'
                                : 'linear-gradient(90deg, #00E7F2 30%, #FF9255 70%)',
                            WebkitBackgroundClip: 'text',
                            WebkitTextFillColor: 'transparent',
                        }}
                    >
                        Search Results
                    </Typography>
                </Box>

                {isLoading && !searchResults.length && (
                    <Box
                        display="flex"
                        justifyContent="center"
                        flexDirection="column"
                        alignItems="center"
                        my={8}
                    >
                        <CircularProgress
                            size={60}
                            thickness={4}
                            sx={{
                                color: theme.palette.mode === 'light'
                                    ? theme.palette.primary.main
                                    : theme.palette.primary.light
                            }}
                        />
                        <Typography
                            variant="h6"
                            sx={{ mt: 2 }}
                            color="text.secondary"
                        >
                            Searching for specials...
                        </Typography>
                    </Box>
                )}

                {error && (
                    <Paper
                        elevation={2}
                        sx={{
                            p: 4,
                            my: 4,
                            textAlign: 'center',
                            borderRadius: 3,
                            borderLeft: '4px solid',
                            borderColor: theme.palette.error.main
                        }}
                    >
                        <Typography
                            color="error"
                            variant="h6"
                            gutterBottom
                            fontWeight="bold"
                        >
                            Error Loading Results
                        </Typography>
                        <Typography color="text.secondary">
                            {error}. Please try again.
                        </Typography>
                    </Paper>
                )}

                {!isLoading && !error && searchResults.length === 0 && (
                    <Paper
                        elevation={2}
                        sx={{
                            p: 6,
                            my: 4,
                            textAlign: 'center',
                            borderRadius: 3,
                            backgroundColor: theme.palette.mode === 'light'
                                ? 'rgba(255, 255, 255, 0.8)'
                                : 'rgba(22, 28, 35, 0.8)',
                        }}
                    >
                        <SearchOffIcon
                            sx={{
                                fontSize: 60,
                                mb: 2,
                                opacity: 0.7,
                                color: theme.palette.mode === 'light'
                                    ? 'rgba(0, 194, 203, 0.5)'
                                    : 'rgba(0, 231, 242, 0.5)'
                            }}
                        />
                        <Typography
                            variant="h5"
                            gutterBottom
                            fontWeight="bold"
                        >
                            No specials found matching your criteria
                        </Typography>
                        <Typography
                            variant="body1"
                            color="text.secondary"
                            sx={{ maxWidth: 500, mx: 'auto' }}
                        >
                            Try adjusting your search radius, filters, or search terms to find more results.
                        </Typography>
                    </Paper>
                )}

                {!isLoading && !error && searchResults.length > 0 && (
                    <Box sx={{ mt: 2 }}>
                        {/* Render each search result with the SpecialMenu component */}
                        {searchResults.map(searchResult => (
                            <SpecialMenu
                                key={searchResult.venue.id}
                                searchResult={searchResult}
                            />
                        ))}

                        {pagingInfo.totalCount > pagingInfo.pageSize && (
                            <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
                                <Pager
                                    currentPage={pagingInfo.currentPage}
                                    totalPages={totalPages}
                                    pageSize={pagingInfo.pageSize}
                                    totalCount={pagingInfo.totalCount}
                                    onPageChange={handlePageChange}
                                    onPageSizeChange={handlePageSizeChange}
                                />
                            </Box>
                        )}
                    </Box>
                )}
            </Box>
        </Container>
    );
};
