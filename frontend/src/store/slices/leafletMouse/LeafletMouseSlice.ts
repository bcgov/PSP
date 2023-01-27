import { createAction, createSlice } from '@reduxjs/toolkit';

export const saveClickLatLng = createAction<ISerializableLeafletMouseEvent>('saveClickLatLng');
export const clearClickLatLng = createAction('clearLatLng');

export interface ISerializableLeafletMouseEvent {
  latlng: { lat: number; lng: number };
  originalEvent: {
    timeStamp: number | undefined;
  };
}

export interface ILeafletMouseSlice {
  mapClickEvent: ISerializableLeafletMouseEvent | null;
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
