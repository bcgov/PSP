import { createAction, createSlice } from '@reduxjs/toolkit';

export const saveFilter = createAction<{ [key: string]: any }>('saveFilter');
export const clearFilter = createAction('clearFilter');
/**
 * Save and clear a filter used with useRouterFilter on the mapview and list view.
 */
const filterSlice = createSlice({
  name: 'filter',
  initialState: {} as { [key: string]: any },
  reducers: {},
  extraReducers: (builder: any) => {
    // note that redux-toolkit uses immer to prevent state from being mutated.
    builder.addCase(saveFilter, (state: any, action: any) => {
      return action.payload;
    });
    builder.addCase(clearFilter, (state: any) => {
      return '';
    });
  },
});

export default filterSlice;
