import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosInstance } from 'axios';
import { OperatingScheduleService } from '@services/operatingScheduleService';
import { 
  OperatingScheduleItemExtended, 
  CreateOperatingScheduleRequest,
  UpdateOperatingScheduleRequest 
} from '@models/operatingSchedule';
import { ApiError } from '@models/common';

interface ScheduleState {
  currentSchedule: OperatingScheduleItemExtended | null;
  loading: boolean;
  error: string | null;
}

const initialState: ScheduleState = {
  currentSchedule: null,
  loading: false,
  error: null
};

export const getScheduleById = createAsyncThunk<
  OperatingScheduleItemExtended,
  { id: string, apiClient: AxiosInstance },
  { rejectValue: ApiError }
>('schedules/getScheduleById', async ({ id, apiClient }, { rejectWithValue }) => {
  try {
    return await OperatingScheduleService.getOperatingScheduleById(id, apiClient);
  } catch (error) {
    return rejectWithValue(error as ApiError);
  }
});

export const createSchedule = createAsyncThunk(
  'schedules/createSchedule',
  async (
    { scheduleData, apiClient }: { scheduleData: CreateOperatingScheduleRequest; apiClient: AxiosInstance }, 
    { rejectWithValue }
  ) => {
    try {
      return await OperatingScheduleService.createOperatingSchedule(scheduleData, apiClient);
    } catch (error) {
      return rejectWithValue(error);
    }
  }
);

export const updateSchedule = createAsyncThunk(
  'schedules/updateSchedule',
  async (
    { id, scheduleData, apiClient }: { id: string; scheduleData: UpdateOperatingScheduleRequest; apiClient: AxiosInstance }, 
    { rejectWithValue }
  ) => {
    try {
      return await OperatingScheduleService.updateOperatingSchedule(id, scheduleData, apiClient);
    } catch (error) {
      return rejectWithValue(error);
    }
  }
);

export const deleteSchedule = createAsyncThunk(
  'schedules/deleteSchedule',
  async ({ id, apiClient }: { id: string; apiClient: AxiosInstance }, { rejectWithValue }) => {
    try {
      return await OperatingScheduleService.deleteOperatingSchedule(id, apiClient);
    } catch (error) {
      return rejectWithValue(error);
    }
  }
);

const scheduleSlice = createSlice({
  name: 'schedules',
  initialState,
  reducers: {
    clearCurrentSchedule: (state) => {
      state.currentSchedule = null;
    },
    clearScheduleError: (state) => {
      state.error = null;
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase(getScheduleById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(getScheduleById.fulfilled, (state, action) => {
        state.loading = false;
        state.currentSchedule = action.payload;
      })
      .addCase(getScheduleById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to fetch schedule';
      })
      .addCase(createSchedule.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createSchedule.fulfilled, (state, action) => {
        state.loading = false;
        state.currentSchedule = action.payload;
      })
      .addCase(createSchedule.rejected, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to create schedule';
      })
      .addCase(updateSchedule.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateSchedule.fulfilled, (state, action) => {
        state.loading = false;
        state.currentSchedule = action.payload;
      })
      .addCase(updateSchedule.rejected, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to update schedule';
      })
      .addCase(deleteSchedule.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteSchedule.fulfilled, (state) => {
        state.loading = false;
        state.currentSchedule = null;
      })
      .addCase(deleteSchedule.rejected, (state, action: PayloadAction<any>) => {
        state.loading = false;
        state.error = action.payload?.message || 'Failed to delete schedule';
      });
  }
});

export const { clearCurrentSchedule, clearScheduleError } = scheduleSlice.actions;

export default scheduleSlice.reducer;
export const scheduleReducer = scheduleSlice.reducer;
