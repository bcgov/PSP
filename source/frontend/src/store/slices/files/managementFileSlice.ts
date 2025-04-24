import { createSlice, PayloadAction } from '@reduxjs/toolkit';

import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';

export interface ManagementFileState {
  [key: number]: { ApiGen_Concepts_ManagementFile };
}

export const managementSlice = createSlice({
  name: 'managementFiles',
  initialState: {} as ManagementFileState,
  reducers: {
    setManagementFile(state: any, action: PayloadAction<ApiGen_Concepts_ManagementFile>) {
      state[action.payload?.id] = action.payload;
    },
    clearManagementFile(state: any, action: PayloadAction<number>) {
      delete state[action.payload];
    },
  },
});

export const { setManagementFile, clearManagementFile } = managementSlice.actions;

export default managementSlice;
