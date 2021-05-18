import { LeafletMouseEvent } from 'leaflet';
import { createSlice, createAction } from '@reduxjs/toolkit';

export const saveClickLatLng = createAction<LeafletMouseEvent>('saveClickLatLng');
export const clearClickLatLng = createAction('clearLatLng');

export interface ILeafletMouseSlice {
  mapClickEvent: LeafletMouseEvent | null;
}

const leafletMouseSlice = createSlice({
  name: 'leafletMouseEvent',
  initialState: { mapClickEvent: null } as ILeafletMouseSlice,
  reducers: {},
  extraReducers: (builder: any) => {
    // note that redux-toolkit uses immer to prevent state from being mutated.
    builder.addCase(saveClickLatLng, (state: any, action: any) => {
      state.mapClickEvent = action.payload;
    });
    builder.addCase(clearClickLatLng, (state: any) => {
      state.mapClickEvent = null;
    });
  },
});

export default leafletMouseSlice;
