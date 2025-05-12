import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface AuthState {
  token: string | null;
  isAuthenticated: boolean;
  userId: string | null;
  userName: string | null;
}

const initialState: AuthState = {
  token: null,
  isAuthenticated: false,
  userId: null,
  userName: null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setCredentials: (
      state,
      action: PayloadAction<{
        token: string;
        userId: string;
        userName: string;
      }>
    ) => {
      const { token, userId, userName } = action.payload;
      state.token = token;
      state.userId = userId;
      state.userName = userName;
      state.isAuthenticated = true;
    },
    clearCredentials: (state) => {
      state.token = null;
      state.isAuthenticated = false;
      state.userId = null;
      state.userName = null;
    },
  },
});

export const { setCredentials, clearCredentials } = authSlice.actions;

export default authSlice.reducer;
