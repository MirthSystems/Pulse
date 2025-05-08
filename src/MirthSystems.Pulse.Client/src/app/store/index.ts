import { configureStore } from '@reduxjs/toolkit';
import { counterReducer } from '../../features/counter';
import { userReducer } from '../../features/user';

export const store = configureStore({
  reducer: {
    counter: counterReducer,
    user: userReducer,
  },
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;