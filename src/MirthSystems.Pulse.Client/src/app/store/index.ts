import { configureStore } from '@reduxjs/toolkit';
import { counterReducer } from '../../features/counter';
import { userReducer } from '../../features/user';
import { themeReducer } from '../../features/ui/redux';
import { authMiddleware } from '../../features/api/redux/middleware';

export const store = configureStore({
  reducer: {
    counter: counterReducer,
    user: userReducer,
    theme: themeReducer,
  },
  middleware: (getDefaultMiddleware) => 
    getDefaultMiddleware().concat(authMiddleware),
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;