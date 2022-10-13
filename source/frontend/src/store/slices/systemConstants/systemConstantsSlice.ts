import { createSlice, PayloadAction } from '@reduxjs/toolkit';

import { ISystemConstant, ISystemConstantsState } from '.';

export const initialState: ISystemConstantsState = {
  systemConstants: [],
};
/**
 * The system constants reducer stores the complete system constants used within the application.
 */
export const systemConstantsSlice = createSlice({
  name: 'systemConstant',
  initialState: initialState,
  reducers: {
    storeSystemConstants(state: ISystemConstantsState, action: PayloadAction<ISystemConstant[]>) {
      state.systemConstants = action.payload;
    },
  },
});

// Destructure and export the plain action creators
export const { storeSystemConstants } = systemConstantsSlice.actions;
