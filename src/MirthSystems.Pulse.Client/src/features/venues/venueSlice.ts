import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { VenueService } from '@services/venueService';
import { VenueItem, VenueItemExtended, VenueSearchParams } from '@models/venue';
import { PagedResult, ApiError } from '@models/common';
import { OperatingScheduleItem } from '@models/operatingSchedule';
import { SpecialItem } from '@models/special';
import { AxiosInstance } from 'axios';

interface VenueState {
  venues: VenueItem[];
  currentVenue: VenueItemExtended | null;
  venueBusinessHours: OperatingScheduleItem[];
  venueSpecials: SpecialItem[];
  pagingInfo: {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  };
  loading: boolean;
  error: string | null;
}

const initialState: VenueState = {
  venues: [],
  currentVenue: null,
  venueBusinessHours: [],
  venueSpecials: [],
  pagingInfo: {
    currentPage: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 0,
  },
  loading: false,
  error: null,
};

// Helper function to process API errors
const processApiError = (error: any): ApiError => {
  const errorMessage = error.message || 'An unexpected error occurred';
  
  // Extract status and error details if available
  const statusCode = error.status || 500;
  const errorDetails = error.errors || {};
  
  return {
    status: statusCode,
    message: errorMessage,
    errors: errorDetails
  };
};

// Async thunks
export const fetchVenues = createAsyncThunk<
  PagedResult<VenueItem>, 
  VenueSearchParams, 
  { rejectValue: ApiError }
>('venues/fetchVenues', async (params, { rejectWithValue }) => {
  try {
    return await VenueService.getVenues(params);
  } catch (error) {
    return rejectWithValue(processApiError(error));
  }
});

export const fetchVenueById = createAsyncThunk<
  VenueItemExtended, 
  string, 
  { rejectValue: ApiError }
>('venues/fetchVenueById', async (id, { rejectWithValue }) => {
  try {
    return await VenueService.getVenueById(id);
  } catch (error) {
    return rejectWithValue(processApiError(error));
  }
});

export const fetchVenueBusinessHours = createAsyncThunk<
  OperatingScheduleItem[], 
  string, 
  { rejectValue: ApiError }
>('venues/fetchVenueBusinessHours', async (id, { rejectWithValue }) => {
  try {
    return await VenueService.getVenueBusinessHours(id);
  } catch (error) {
    // Return empty array for 404s to prevent errors
    if (error.status === 404) {
      return [];
    }
    return rejectWithValue(processApiError(error));
  }
});

export const fetchVenueSpecials = createAsyncThunk<
  SpecialItem[], 
  string, 
  { rejectValue: ApiError }
>('venues/fetchVenueSpecials', async (id, { rejectWithValue }) => {
  try {
    return await VenueService.getVenueSpecials(id);
  } catch (error) {
    // Return empty array for 404s to prevent errors
    if (error.status === 404) {
      return [];
    }
    return rejectWithValue(processApiError(error));
  }
});

export const createVenue = createAsyncThunk(
  'venues/createVenue',
  async ({ venueData, apiClient }: { venueData: any; apiClient: AxiosInstance }, { rejectWithValue }) => {
    try {
      return await VenueService.createVenue(venueData, apiClient);
    } catch (error) {
      return rejectWithValue(processApiError(error));
    }
  }
);

export const updateVenue = createAsyncThunk(
  'venues/updateVenue',
  async ({ id, venueData, apiClient }: { id: string; venueData: any; apiClient: AxiosInstance }, { rejectWithValue }) => {
    try {
      return await VenueService.updateVenue(id, venueData, apiClient);
    } catch (error) {
      return rejectWithValue(processApiError(error));
    }
  }
);

export const deleteVenue = createAsyncThunk(
  'venues/deleteVenue',
  async ({ id, apiClient }: { id: string; apiClient: AxiosInstance }, { rejectWithValue }) => {
    try {
      return await VenueService.deleteVenue(id, apiClient);
    } catch (error) {
      return rejectWithValue(processApiError(error));
    }
  }
);

const venueSlice = createSlice({
  name: 'venues',
  initialState,
  reducers: {
    clearCurrentVenue: (state) => {
      state.currentVenue = null;
    },
    clearVenueError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      // fetchVenues
      .addCase(fetchVenues.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchVenues.fulfilled, (state, action) => {
        state.loading = false;
        state.venues = action.payload.items;
        state.pagingInfo = action.payload.pagingInfo;
      })
      .addCase(fetchVenues.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to fetch venues';
      })
      
      // fetchVenueById
      .addCase(fetchVenueById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchVenueById.fulfilled, (state, action) => {
        state.loading = false;
        state.currentVenue = action.payload;
      })
      .addCase(fetchVenueById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to fetch venue';
      })
      
      // fetchVenueBusinessHours
      .addCase(fetchVenueBusinessHours.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchVenueBusinessHours.fulfilled, (state, action) => {
        state.loading = false;
        state.venueBusinessHours = action.payload;
      })
      .addCase(fetchVenueBusinessHours.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to fetch business hours';
      })
      
      // fetchVenueSpecials
      .addCase(fetchVenueSpecials.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchVenueSpecials.fulfilled, (state, action) => {
        state.loading = false;
        state.venueSpecials = action.payload;
      })
      .addCase(fetchVenueSpecials.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to fetch specials';
      })
      
      // createVenue
      .addCase(createVenue.pending, (state) => {
        state.loading = true;
      })
      .addCase(createVenue.fulfilled, (state, action) => {
        state.loading = false;
        state.currentVenue = action.payload;
      })
      .addCase(createVenue.rejected, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to create venue';
      })
      
      // updateVenue
      .addCase(updateVenue.pending, (state) => {
        state.loading = true;
      })
      .addCase(updateVenue.fulfilled, (state, action) => {
        state.loading = false;
        state.currentVenue = action.payload;
      })
      .addCase(updateVenue.rejected, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to update venue';
      })
      
      // deleteVenue
      .addCase(deleteVenue.pending, (state) => {
        state.loading = true;
      })
      .addCase(deleteVenue.fulfilled, (state) => {
        state.loading = false;
        state.currentVenue = null;
      })
      .addCase(deleteVenue.rejected, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to delete venue';
      });
  },
});

export const { clearCurrentVenue, clearVenueError } = venueSlice.actions;

export default venueSlice.reducer;
export const venueReducer = venueSlice.reducer;
