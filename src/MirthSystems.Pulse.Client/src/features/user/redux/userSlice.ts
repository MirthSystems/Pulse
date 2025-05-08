import { PayloadAction, createSlice } from "@reduxjs/toolkit";

export interface UserState {
  isAuthenticated: boolean;
  token: string | null;
  lastTokenRefresh: number | null;
  userId: string | null;
  userName: string | null;
  userEmail: string | null;
  userRoles: string[];
}

const initialState: UserState = {
  isAuthenticated: false,
  token: null,
  lastTokenRefresh: null,
  userId: null,
  userName: null,
  userEmail: null,
  userRoles: []
};

export interface UserInfo {
  userId: string;
  userName: string | null;
  userEmail: string | null;
  userRoles: string[];
}

export const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    setAuthenticated: (state, action: PayloadAction<boolean>) => {
      state.isAuthenticated = action.payload;
      if (!action.payload) {
        // Clear user state on logout
        state.token = null;
        state.lastTokenRefresh = null;
        state.userId = null;
        state.userName = null;
        state.userEmail = null;
        state.userRoles = [];
      }
    },
    setToken: (state, action: PayloadAction<string>) => {
      state.token = action.payload;
      state.lastTokenRefresh = Date.now();
    },
    setUserInfo: (state, action: PayloadAction<UserInfo>) => {
      state.userId = action.payload.userId;
      state.userName = action.payload.userName;
      state.userEmail = action.payload.userEmail;
      state.userRoles = action.payload.userRoles;
    }
  }
});

export const { setAuthenticated, setToken, setUserInfo } = userSlice.actions;

export default userSlice.reducer;