import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { PointFeature } from 'components/maps/types';

import { IPropertyState } from './interfaces';

export const initialState: IPropertyState = {
  draftProperties: [],
};

/**
 * The properties reducer provides actions to manage the displayed properties used within the application.
 */
export const propertiesSlice = createSlice({
  name: 'properties',
  initialState: initialState,
  reducers: {
    storeDraftProperties(state: IPropertyState, action: PayloadAction<PointFeature[]>) {
      state.draftProperties = action.payload;
    },
  },
});

// Destructor and export the plain action creators
export const { storeDraftProperties } = propertiesSlice.actions;
