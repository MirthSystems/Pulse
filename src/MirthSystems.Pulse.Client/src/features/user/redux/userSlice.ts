import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface UserState {
  isInitialized: boolean;
  isAuthenticated: boolean;
  token: string | null;
  userId: string | null;
  userName: string | null;
  userEmail: string | null;
  userRoles: string[];
  lastTokenRefresh: number | null;
}

const initialState: UserState = {
  isInitialized: false,
  isAuthenticated: false,
  token: null,
  userId: null,
  userName: null,
  userEmail: null,
  userRoles: [],
  lastTokenRefresh: null,
};

export const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    setInitialized: (state, action: PayloadAction<boolean>) => {
      state.isInitialized = action.payload;
    },
    
    setAuthenticated: (state, action: PayloadAction<boolean>) => {
      state.isAuthenticated = action.payload;
      // If logging out, clear user info
      if (!action.payload) {
        state.token = null;
        state.userId = null;
        state.userName = null;
        state.userEmail = null;
        state.userRoles = [];
        state.lastTokenRefresh = null;
      }
    },
    
    setToken: (state, action: PayloadAction<string | null>) => {
      state.token = action.payload;
      state.lastTokenRefresh = action.payload ? Date.now() : null;
    },
    
    setUserInfo: (state, action: PayloadAction<{ 
      userId: string; 
      userName: string | null; 
      userEmail: string | null;
      userRoles?: string[];
    }>) => {
      state.userId = action.payload.userId;
      state.userName = action.payload.userName;
      state.userEmail = action.payload.userEmail;
      if (action.payload.userRoles) {
        state.userRoles = action.payload.userRoles;
      }
    },
  },
});

export const { setInitialized, setAuthenticated, setToken, setUserInfo } = userSlice.actions;
export default userSlice.reducer;