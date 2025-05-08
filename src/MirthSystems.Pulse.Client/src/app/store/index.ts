import { configureStore } from '@reduxjs/toolkit';
import { counterReducer } from '../../features/counter';
import { userReducer } from '../../features/user';
import { authMiddleware } from '../../features/api/redux/middleware';

export const store = configureStore({
  reducer: {
    counter: counterReducer,
    user: userReducer,
  },
  middleware: (getDefaultMiddleware) => 
    getDefaultMiddleware().concat(authMiddleware),
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;