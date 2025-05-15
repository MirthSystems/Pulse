import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { SpecialService } from '@services/specialService';
import { 
  SpecialItem, 
  SpecialItemExtended,
  SpecialSearchParams, 
  SearchSpecialsResult,
  CreateSpecialRequest,
  UpdateSpecialRequest
} from '@models/special';
import { PagedResult, ApiError } from '@models/common';
import { AxiosError, AxiosInstance } from 'axios';

// Define the state interface
interface SpecialState {
  items: SpecialItem[];
  searchResults: PagedResult<SearchSpecialsResult>;
  currentSpecial: SpecialItemExtended | null;
  pagingInfo: {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
  };
  loading: boolean;
  error: string | null;
}

const initialState: SpecialState = {
  items: [],
  searchResults: {
    items: [],
    pagingInfo: {
      currentPage: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 0,
      hasPreviousPage: false,
      hasNextPage: false,
    }
  },
  currentSpecial: null,
  pagingInfo: {
    currentPage: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 0,
    hasPreviousPage: false,
    hasNextPage: false,
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
export const searchSpecials = createAsyncThunk<
  PagedResult<SearchSpecialsResult>,
  SpecialSearchParams,
  { rejectValue: ApiError }
>('specials/searchSpecials', async (params, { rejectWithValue }) => {
  try {
    // Using the corrected service method
    return await SpecialService.searchSpecials(params);
  } catch (error) {
    // Handle the error properly to ensure it's serializable
    const apiError = processApiError(error);
    return rejectWithValue(apiError);
  }
});

export const getSpecialById = createAsyncThunk<
  SpecialItemExtended,
  string,
  { rejectValue: ApiError }
>('specials/getSpecialById', async (id, { rejectWithValue }) => {
  try {
    return await SpecialService.getSpecialById(id);
  } catch (error) {
    const apiError = processApiError(error);
    return rejectWithValue(apiError);
  }
});

export const createSpecial = createAsyncThunk(
  'specials/createSpecial',
  async ({ specialData, apiClient }: { specialData: CreateSpecialRequest; apiClient: AxiosInstance }, { rejectWithValue }) => {
    try {
      return await SpecialService.createSpecial(specialData, apiClient);
    } catch (error) {
      return rejectWithValue(processApiError(error));
    }
  }
);

export const updateSpecial = createAsyncThunk(
  'specials/updateSpecial',
  async ({ id, specialData, apiClient }: { id: string; specialData: UpdateSpecialRequest; apiClient: AxiosInstance }, { rejectWithValue }) => {
    try {
      return await SpecialService.updateSpecial(id, specialData, apiClient);
    } catch (error) {
      return rejectWithValue(processApiError(error));
    }
  }
);

export const deleteSpecial = createAsyncThunk(
  'specials/deleteSpecial',
  async ({ id, apiClient }: { id: string; apiClient: any }, { rejectWithValue }) => {
    try {
      return await SpecialService.deleteSpecial(id, apiClient);
    } catch (error) {
      const apiError = processApiError(error);
      return rejectWithValue(apiError);
    }
  }
);

const specialSlice = createSlice({
  name: 'specials',
  initialState,
  reducers: {
    clearCurrentSpecial: (state) => {
      state.currentSpecial = null;
    },
    clearSpecialsError: (state) => {
      state.error = null;
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase(searchSpecials.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(searchSpecials.fulfilled, (state, action) => {
        state.loading = false;
        state.searchResults = action.payload;
        state.pagingInfo = {
          ...action.payload.pagingInfo,
          hasPreviousPage: action.payload.pagingInfo.currentPage > 1,
          hasNextPage: action.payload.pagingInfo.currentPage < action.payload.pagingInfo.totalPages,
        };
      })
      .addCase(searchSpecials.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload ? (typeof action.payload === 'object' && 'message' in action.payload ? action.payload.message : 'Unknown error') : 'Failed to search specials';
      })
      .addCase(getSpecialById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(getSpecialById.fulfilled, (state, action) => {
        state.loading = false;
        state.currentSpecial = action.payload;
      })
      .addCase(getSpecialById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload ? (typeof action.payload === 'object' && 'message' in action.payload ? action.payload.message : 'Unknown error') : 'Failed to fetch special';
      })
      .addCase(createSpecial.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createSpecial.fulfilled, (state, action) => {
        state.loading = false;
        state.currentSpecial = action.payload;
      })
      .addCase(createSpecial.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload ? (typeof action.payload === 'object' && 'message' in action.payload ? action.payload.message : 'Unknown error') : 'Failed to create special';
      })
      .addCase(updateSpecial.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateSpecial.fulfilled, (state, action) => {
        state.loading = false;
        state.currentSpecial = action.payload;
      })
      .addCase(updateSpecial.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload ? (typeof action.payload === 'object' && 'message' in action.payload ? action.payload.message : 'Unknown error') : 'Failed to update special';
      })
      
      // deleteSpecial
      .addCase(deleteSpecial.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteSpecial.fulfilled, (state) => {
        state.loading = false;
        state.currentSpecial = null;
      })
      .addCase(deleteSpecial.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload ? (typeof action.payload === 'object' && 'message' in action.payload ? action.payload.message : 'Unknown error') : 'Failed to delete special';
      });
  },
});

export const { clearCurrentSpecial, clearSpecialsError } = specialSlice.actions;

export const specialReducer = specialSlice.reducer;
