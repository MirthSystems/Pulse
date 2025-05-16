import CategoryIcon from '@mui/icons-material/Category';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import SearchIcon from '@mui/icons-material/Search';
import StraightenIcon from '@mui/icons-material/Straighten';
import { Box, FormControl, FormControlLabel, Grid, InputAdornment, InputLabel, MenuItem, Paper, Select, Slider, Switch, TextField, Typography, useTheme } from '@mui/material';
import { DateTimePicker, LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterLuxon } from '@mui/x-date-pickers/AdapterLuxon';
import { DateTime } from 'luxon';
import { useEffect, useState } from 'react';
import type { GetSpecialsRequest } from '../../models';
import { SpecialTypes } from '../../models';

interface SearchSpecialsFilterProps {
    initialFilters: Partial<GetSpecialsRequest>;
    onFilterChange: (filters: Partial<GetSpecialsRequest>) => void;
    isLoading?: boolean;
}

const radiusMarks = [
    { value: 1, label: '1 mi' },
    { value: 5, label: '5 mi' },
    { value: 10, label: '10 mi' },
    { value: 25, label: '25 mi' },
    { value: 50, label: '50 mi' },
];

export const SearchSpecialsFilter = ({ initialFilters, onFilterChange, isLoading = false }: SearchSpecialsFilterProps) => {
    const theme = useTheme();
    const [internalFilters, setInternalFilters] = useState<Partial<GetSpecialsRequest>>(initialFilters);
    const [searchDateTime, setSearchDateTime] = useState<DateTime | null>(
        initialFilters.searchDateTime ? DateTime.fromISO(initialFilters.searchDateTime) : null
    );

    useEffect(() => {
        setInternalFilters(initialFilters);
        setSearchDateTime(initialFilters.searchDateTime ? DateTime.fromISO(initialFilters.searchDateTime) : null);
    }, [initialFilters]);

    const handleChange = <T extends keyof GetSpecialsRequest>(field: T, value: GetSpecialsRequest[T] | undefined) => {
        const newFilters = { ...internalFilters, [field]: value };
        setInternalFilters(newFilters);
        onFilterChange(newFilters);
    };

    const handleDateTimeChange = (newValue: DateTime | null) => {
        setSearchDateTime(newValue);
        handleChange('searchDateTime', newValue?.toISO() || undefined);
    };

    const handleSliderChange = (_event: Event, newValue: number | number[]) => {
        handleChange('radius', newValue as number);
    };

    const inputStyles = {
        '& .MuiOutlinedInput-root': {
            borderRadius: 2,
            transition: 'box-shadow 0.2s ease-in-out',
            '&.Mui-focused': {
                boxShadow: theme.palette.mode === 'light'
                    ? '0 0 0 2px rgba(0, 194, 203, 0.2)'
                    : '0 0 0 2px rgba(0, 231, 242, 0.2)'
            },
            '&:hover': {
                boxShadow: theme.palette.mode === 'light'
                    ? '0 0 0 2px rgba(0, 194, 203, 0.1)'
                    : '0 0 0 2px rgba(0, 231, 242, 0.1)'
            }
        }
    };

    return (
        <LocalizationProvider dateAdapter={AdapterLuxon}>
            <Paper
                elevation={0}
                sx={{
                    p: { xs: 2, sm: 3 },
                    mb: 0,
                    borderRadius: 2,
                    backgroundColor: theme.palette.mode === 'light'
                        ? 'rgba(255, 255, 255, 0.8)'
                        : 'rgba(22, 28, 35, 0.8)'
                }}
            >
                {/* Main search row */}
                <Grid container spacing={2} sx={{ mb: 3 }}>
                    <Grid sx={{ gridColumn: { xs: 'span 12', md: 'span 4' } }}>
                        <TextField
                            placeholder="Address, City, or Zip Code"
                            fullWidth
                            value={internalFilters.address || ''}
                            onChange={(e) => handleChange('address', e.target.value)}
                            disabled={isLoading}
                            sx={inputStyles}
                            InputProps={{
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <LocationOnIcon sx={{
                                            color: theme.palette.mode === 'light'
                                                ? 'primary.main'
                                                : 'primary.light'
                                        }} />
                                    </InputAdornment>
                                ),
                            }}
                        />
                    </Grid>
                    <Grid sx={{ gridColumn: { xs: 'span 12', md: 'span 4' } }}>
                        <TextField
                            placeholder="Search for specials..."
                            fullWidth
                            value={internalFilters.searchTerm || ''}
                            onChange={(e) => handleChange('searchTerm', e.target.value)}
                            disabled={isLoading}
                            sx={inputStyles}
                            InputProps={{
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <SearchIcon sx={{
                                            color: theme.palette.mode === 'light'
                                                ? 'primary.main'
                                                : 'primary.light'
                                        }} />
                                    </InputAdornment>
                                ),
                            }}
                        />
                    </Grid>
                    <Grid sx={{ gridColumn: { xs: 'span 12', md: 'span 4' } }}>
                        <FormControl fullWidth disabled={isLoading} sx={inputStyles}>
                            <InputLabel id="special-type-label">
                                Special Type
                            </InputLabel>
                            <Select
                                labelId="special-type-label"
                                label="Special Type"
                                value={internalFilters.specialTypeId === undefined ? '' : internalFilters.specialTypeId.toString()}
                                onChange={(e) => handleChange('specialTypeId', e.target.value === '' ? undefined : Number(e.target.value))}
                                startAdornment={
                                    <InputAdornment position="start">
                                        <CategoryIcon sx={{
                                            color: theme.palette.mode === 'light'
                                                ? 'primary.main'
                                                : 'primary.light',
                                            ml: 1
                                        }} />
                                    </InputAdornment>
                                }
                            >
                                <MenuItem value=""><em>Any Type</em></MenuItem>
                                {Object.entries(SpecialTypes).map(([key, value]) => (
                                    typeof value === 'number' && <MenuItem key={key} value={value.toString()}>{key}</MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </Grid>
                </Grid>

                {/* Radius slider */}
                <Box sx={{ px: 2, mb: 3 }}>
                    <Typography
                        variant="subtitle2"
                        gutterBottom
                        sx={{
                            display: 'flex',
                            alignItems: 'center',
                            mb: 1,
                            color: theme.palette.mode === 'light'
                                ? 'text.secondary'
                                : 'text.primary',
                            fontWeight: 500
                        }}
                    >
                        <StraightenIcon
                            sx={{
                                mr: 1,
                                fontSize: 18,
                                color: theme.palette.mode === 'light'
                                    ? 'primary.main'
                                    : 'primary.light'
                            }}
                        />
                        Search Radius: <Box component="span" sx={{ ml: 0.5, fontWeight: 600 }}>
                            {internalFilters.radius || 5} miles
                        </Box>
                    </Typography>
                    <Slider
                        value={internalFilters.radius || 5}
                        onChange={handleSliderChange}
                        aria-labelledby="radius-slider"
                        valueLabelDisplay="auto"
                        step={null}
                        marks={radiusMarks}
                        min={1}
                        max={50}
                        disabled={isLoading}
                        valueLabelFormat={(value) => `${value} mi`}
                    />
                </Box>

                {/* Additional filters */}
                <Grid container spacing={2}>
                    <Grid sx={{ gridColumn: { xs: 'span 12', sm: 'span 6' } }}>
                        <FormControlLabel
                            control={
                                <Switch
                                    checked={internalFilters.isCurrentlyRunning || false}
                                    onChange={(e) => handleChange('isCurrentlyRunning', e.target.checked)}
                                    disabled={isLoading}
                                    color="primary"
                                />
                            }
                            label="Only show active specials"
                            sx={{
                                '& .MuiFormControlLabel-label': {
                                    fontWeight: 500
                                }
                            }}
                        />
                    </Grid>
                    <Grid sx={{ gridColumn: { xs: 'span 12', sm: 'span 6' } }}>
                        <DateTimePicker
                            label="Search Date & Time (Optional)"
                            value={searchDateTime}
                            onChange={handleDateTimeChange}
                            ampm={false}
                            slotProps={{
                                textField: {
                                    fullWidth: true,
                                    size: "small",
                                    disabled: isLoading,
                                    sx: inputStyles
                                }
                            }}
                        />
                    </Grid>
                </Grid>
            </Paper>
        </LocalizationProvider>
    );
};
