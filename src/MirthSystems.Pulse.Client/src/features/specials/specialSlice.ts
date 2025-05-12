import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosInstance } from 'axios';
import { SpecialService } from '@services/specialService';
import { 
  SpecialSearchParams,
  SpecialItem, 
  SpecialItemExtended, 
  CreateSpecialRequest, 
  UpdateSpecialRequest,
  SearchSpecialsResult
} from '@models/special';
import { PagedResult, ApiError } from '@models/common';

interface SpecialState {
  specials: SpecialItem[];
  currentSpecial: SpecialItemExtended | null;
  searchResults: PagedResult<SearchSpecialsResult>;
  loading: boolean;
  error: string | null;
  pagingInfo: {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  };
}

const initialState: SpecialState = {
  specials: [],
  currentSpecial: null,
  searchResults: {
    items: [],
    pagingInfo: {
      currentPage: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 0,
      hasPreviousPage: false,
      hasNextPage: false
    }
  },
  loading: false,
  error: null,
  pagingInfo: {
    currentPage: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 0
  }
};

export const searchSpecials = createAsyncThunk<
  PagedResult<SearchSpecialsResult>,
  SpecialSearchParams,
  { rejectValue: ApiError }
>('specials/searchSpecials', async (params, { rejectWithValue }) => {
  try {
    return await SpecialService.searchSpecials(params);
  } catch (error) {
    return rejectWithValue(error as ApiError);
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
    return rejectWithValue(error as ApiError);
  }
});

export const createSpecial = createAsyncThunk(
  'specials/createSpecial',
  async ({ specialData, apiClient }: { specialData: CreateSpecialRequest; apiClient: AxiosInstance }, { rejectWithValue }) => {
    try {
      return await SpecialService.createSpecial(specialData, apiClient);
    } catch (error) {
      return rejectWithValue(error);
    }
  }
);

export const updateSpecial = createAsyncThunk(
  'specials/updateSpecial',
  async ({ id, specialData, apiClient }: { id: string; specialData: UpdateSpecialRequest; apiClient: AxiosInstance }, { rejectWithValue }) => {
    try {
      return await SpecialService.updateSpecial(id, specialData, apiClient);
    } catch (error) {
      return rejectWithValue(error);
    }
  }
);

export const deleteSpecial = createAsyncThunk(
  'specials/deleteSpecial',
  async ({ id, apiClient }: { id: string; apiClient: AxiosInstance }, { rejectWithValue }) => {
    try {
      return await SpecialService.deleteSpecial(id, apiClient);
    } catch (error) {
      return rejectWithValue(error);
    }
  }
);

const specialsSlice = createSlice({
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
      // searchSpecials
      .addCase(searchSpecials.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(searchSpecials.fulfilled, (state, action) => {
        state.loading = false;
        state.searchResults = action.payload;
        state.pagingInfo = action.payload.pagingInfo;
      })
      .addCase(searchSpecials.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to search specials';
      })
      
      // getSpecialById
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
        state.error = action.payload?.message || 'Failed to fetch special';
      })
      
      // createSpecial
      .addCase(createSpecial.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createSpecial.fulfilled, (state, action) => {
        state.loading = false;
        state.currentSpecial = action.payload;
      })
      .addCase(createSpecial.rejected, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to create special';
      })
      
      // updateSpecial
      .addCase(updateSpecial.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateSpecial.fulfilled, (state, action) => {
        state.loading = false;
        state.currentSpecial = action.payload;
      })
      .addCase(updateSpecial.rejected, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to update special';
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
      .addCase(deleteSpecial.rejected, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to delete special';
      });
  }
});

export const { clearCurrentSpecial, clearSpecialsError } = specialsSlice.actions;

export default specialsSlice.reducer;
