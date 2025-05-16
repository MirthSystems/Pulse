import EventIcon from '@mui/icons-material/Event';
import NightlifeIcon from '@mui/icons-material/Nightlife'; // Food and drink icon
import NotificationsActiveIcon from '@mui/icons-material/NotificationsActive';
import { Box, Container, Grid, Paper, Typography, useTheme } from '@mui/material';
import { SearchSpecials } from '../specials/search-specials';

export const HomeContent = () => {
    const theme = useTheme();

    return (
        <Container maxWidth="lg">
            <Box sx={{ my: { xs: 2, md: 4 } }}>
                {/* Main search section */}
                <SearchSpecials />

                {/* Why use Pulse section */}
                <Box sx={{ width: '100%', mt: 6, mb: 4 }}>
                    <Typography
                        variant="h4"
                        component="h2"
                        align="center"
                        fontWeight="bold"
                        sx={{
                            mb: 1,
                            position: 'relative',
                            '&:after': {
                                content: '""',
                                position: 'absolute',
                                bottom: -10,
                                left: '50%',
                                transform: 'translateX(-50%)',
                                width: 80,
                                height: 4,
                                borderRadius: 2,
                                backgroundColor: theme.palette.mode === 'light'
                                    ? theme.palette.secondary.main
                                    : theme.palette.secondary.light
                            }
                        }}
                    >
                        Why use Pulse?
                    </Typography>

                    <Typography
                        variant="subtitle1"
                        align="center"
                        color="text.secondary"
                        sx={{
                            maxWidth: 700,
                            mx: 'auto',
                            mt: 3,
                            mb: 5,
                            px: 2
                        }}
                    >
                        Pulse is designed to help you make the most of your time out, with features
                        that enhance your experience finding and enjoying the best specials and events.
                    </Typography>

                    <Grid container spacing={4}>
                        <Grid sx={{ gridColumn: { xs: 'span 12', sm: 'span 12', md: 'span 4' } }}>
                            <Paper
                                elevation={theme.palette.mode === 'light' ? 1 : 2}
                                sx={{
                                    p: 3,
                                    height: '100%',
                                    borderRadius: 3,
                                    transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
                                    '&:hover': {
                                        transform: 'translateY(-8px)',
                                        boxShadow: theme.palette.mode === 'light'
                                            ? '0 10px 25px rgba(0, 194, 203, 0.15)'
                                            : '0 10px 25px rgba(0, 0, 0, 0.3), 0 0 10px rgba(0, 231, 242, 0.1)'
                                    }
                                }}
                            >
                                <Box sx={{
                                    display: 'flex',
                                    justifyContent: 'center',
                                    mb: 2
                                }}>
                                    <Box
                                        sx={{
                                            backgroundColor: theme.palette.mode === 'light'
                                                ? 'rgba(0, 194, 203, 0.1)'
                                                : 'rgba(0, 231, 242, 0.1)',
                                            borderRadius: '50%',
                                            p: 1.5,
                                            display: 'flex',
                                            alignItems: 'center',
                                            justifyContent: 'center',
                                        }}
                                    >
                                        <NightlifeIcon
                                            sx={{
                                                fontSize: '2.5rem',
                                                color: theme.palette.mode === 'light'
                                                    ? theme.palette.primary.main
                                                    : theme.palette.primary.light
                                            }}
                                        />
                                    </Box>
                                </Box>
                                <Typography
                                    variant="h5"
                                    align="center"
                                    gutterBottom
                                    fontWeight="bold"
                                >
                                    Discover New Places
                                </Typography>
                                <Typography variant="body1" align="center" color="text.secondary">
                                    Find hidden gems and popular spots in your area with our comprehensive venue listings and detailed information.
                                </Typography>
                            </Paper>
                        </Grid>

                        <Grid sx={{ gridColumn: { xs: 'span 12', sm: 'span 12', md: 'span 4' } }}>
                            <Paper
                                elevation={theme.palette.mode === 'light' ? 1 : 2}
                                sx={{
                                    p: 3,
                                    height: '100%',
                                    borderRadius: 3,
                                    transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
                                    '&:hover': {
                                        transform: 'translateY(-8px)',
                                        boxShadow: theme.palette.mode === 'light'
                                            ? '0 10px 25px rgba(255, 126, 69, 0.15)'
                                            : '0 10px 25px rgba(0, 0, 0, 0.3), 0 0 10px rgba(255, 146, 85, 0.1)'
                                    }
                                }}
                            >
                                <Box sx={{
                                    display: 'flex',
                                    justifyContent: 'center',
                                    mb: 2
                                }}>
                                    <Box
                                        sx={{
                                            backgroundColor: theme.palette.mode === 'light'
                                                ? 'rgba(255, 126, 69, 0.1)'
                                                : 'rgba(255, 146, 85, 0.1)',
                                            borderRadius: '50%',
                                            p: 1.5,
                                            display: 'flex',
                                            alignItems: 'center',
                                            justifyContent: 'center',
                                        }}
                                    >
                                        <NotificationsActiveIcon
                                            sx={{
                                                fontSize: '2.5rem',
                                                color: theme.palette.mode === 'light'
                                                    ? theme.palette.secondary.main
                                                    : theme.palette.secondary.light
                                            }}
                                        />
                                    </Box>
                                </Box>
                                <Typography
                                    variant="h5"
                                    align="center"
                                    gutterBottom
                                    fontWeight="bold"
                                >
                                    Never Miss a Deal
                                </Typography>
                                <Typography variant="body1" align="center" color="text.secondary">
                                    Get real-time updates on promotions, happy hours, and limited-time offers from your favorite venues and discover new places.
                                </Typography>
                            </Paper>
                        </Grid>

                        <Grid sx={{ gridColumn: { xs: 'span 12', sm: 'span 12', md: 'span 4' } }}>
                            <Paper
                                elevation={theme.palette.mode === 'light' ? 1 : 2}
                                sx={{
                                    p: 3,
                                    height: '100%',
                                    borderRadius: 3,
                                    transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
                                    '&:hover': {
                                        transform: 'translateY(-8px)',
                                        boxShadow: theme.palette.mode === 'light'
                                            ? '0 10px 25px rgba(0, 194, 203, 0.15)'
                                            : '0 10px 25px rgba(0, 0, 0, 0.3), 0 0 10px rgba(0, 231, 242, 0.1)'
                                    }
                                }}
                            >
                                <Box sx={{
                                    display: 'flex',
                                    justifyContent: 'center',
                                    mb: 2
                                }}>
                                    <Box
                                        sx={{
                                            backgroundColor: theme.palette.mode === 'light'
                                                ? 'rgba(0, 194, 203, 0.1)'
                                                : 'rgba(0, 231, 242, 0.1)',
                                            borderRadius: '50%',
                                            p: 1.5,
                                            display: 'flex',
                                            alignItems: 'center',
                                            justifyContent: 'center',
                                        }}
                                    >
                                        <EventIcon
                                            sx={{
                                                fontSize: '2.5rem',
                                                color: theme.palette.mode === 'light'
                                                    ? theme.palette.primary.main
                                                    : theme.palette.primary.light
                                            }}
                                        />
                                    </Box>
                                </Box>
                                <Typography
                                    variant="h5"
                                    align="center"
                                    gutterBottom
                                    fontWeight="bold"
                                >
                                    Plan Your Night
                                </Typography>
                                <Typography variant="body1" align="center" color="text.secondary">
                                    Check operating hours and available specials to plan your perfect night out with friends or a relaxing solo adventure.
                                </Typography>
                            </Paper>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
        </Container>
    );
};
