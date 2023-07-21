import { createSlice, PayloadAction } from '@reduxjs/toolkit';

import { ITenantConfig } from '@/hooks/pims-api/interfaces/ITenantConfig';

import { ITenantsState } from '.';

export const initialState: ITenantsState = {
  config: undefined,
};

/**
 * The tenant reducer stores tenant configuration settings.
 */
export const tenantsSlice = createSlice({
  name: 'tenants',
  initialState: initialState,
  reducers: {
    storeSettings(state: ITenantsState, action: PayloadAction<ITenantConfig>) {
      state.config = action.payload;
    },
  },
});

// Deconstruct and export the plain action creators
export const { storeSettings } = tenantsSlice.actions;
