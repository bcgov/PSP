import { createAction, createSlice, PayloadAction } from '@reduxjs/toolkit';

export const savePropertyNames = createAction<string[]>('savePropertyNames');
export const clearPropertyNames = createAction('clearPropertyNames');
/**
 * Slice to handle storage of property names
 */
const propertyNameSlice = createSlice({
  name: 'propertyNames',
  initialState: [] as string[],
  reducers: {},
  extraReducers: (builder: any) => {
    builder.addCase(savePropertyNames, (state: any, action: PayloadAction<string[]>) => {
      return action.payload;
    });
    builder.addCase(clearPropertyNames, (state: any) => {
      return '';
    });
  },
});

export default propertyNameSlice;
