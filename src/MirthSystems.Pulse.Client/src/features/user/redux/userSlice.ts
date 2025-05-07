import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface UserState {
  isInitialized: boolean;
  // Add additional state properties as needed
}

const initialState: UserState = {
  isInitialized: false,
  // Initialize additional state properties
};

export const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    setInitialized: (state, action: PayloadAction<boolean>) => {
      state.isInitialized = action.payload;
    },
    // Add additional reducers as needed
  },
});

export const { setInitialized } = userSlice.actions;
export default userSlice.reducer;