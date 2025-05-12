import { configureStore } from '@reduxjs/toolkit';
import venueReducer from '@features/venues/venueSlice';
import specialReducer from '@features/specials/specialSlice';
import schedulesReducer from '@features/schedules/scheduleSlice';
import authReducer from '@features/auth/authSlice';

export const store = configureStore({
  reducer: {
    venues: venueReducer,
    specials: specialReducer,
    schedules: schedulesReducer,
    auth: authReducer
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
