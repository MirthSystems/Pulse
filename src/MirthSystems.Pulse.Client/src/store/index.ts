import { configureStore } from '@reduxjs/toolkit';
import { venueReducer } from '@store/venueSlice';
import { specialReducer } from '@store/specialSlice';
import { scheduleReducer } from '@store/scheduleSlice';

export const store = configureStore({
  reducer: {
    venues: venueReducer,
    specials: specialReducer,
    schedules: scheduleReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        // Ignore non-serializable values in these action types
        ignoredActions: [
          'venues/fetchVenues/rejected',
          'venues/fetchVenueById/rejected',
          'venues/fetchVenueBusinessHours/rejected',
          'venues/fetchVenueSpecials/rejected',
          'specials/searchSpecials/rejected',
          'specials/getSpecialById/rejected'
        ],
      },
    }),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
