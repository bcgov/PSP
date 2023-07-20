import { createSlice, PayloadAction } from '@reduxjs/toolkit';

import { ERROR, REQUEST, SUCCESS } from '@/constants/actionTypes';

import { IGenericNetworkAction, IGenericNetworkState } from './interfaces';

const initialState: IGenericNetworkState = {};

/**
 * The network reducer stores the status of network transactions tracked by redux, including loading state and errors.
 */
export const networkSlice = createSlice({
  name: 'network',
  initialState: initialState,
  reducers: {
    logRequest(state: IGenericNetworkState, action: PayloadAction<string>) {
      state[action.payload] = {
        name: action.payload,
        isFetching: true,
        status: undefined,
        error: undefined,
        type: REQUEST,
      };
    },
    logSuccess(state: IGenericNetworkState, action: PayloadAction<IGenericNetworkAction>) {
      state[action.payload.name] = {
        name: action.payload.name,
        isFetching: false,
        status: action.payload.status,
        error: undefined,
        type: SUCCESS,
      };
    },
    logError(state: IGenericNetworkState, action: PayloadAction<IGenericNetworkAction>) {
      state[action.payload.name] = {
        name: action.payload.name,
        isFetching: false,
        status: action.payload.status,
        error: action.payload.error,
        type: ERROR,
      };
    },
    logClear(state: IGenericNetworkState, action: PayloadAction<string>) {
      delete state[action.payload];
    },
  },
});

// Destructure and export the plain action creators
export const { logRequest, logSuccess, logError, logClear } = networkSlice.actions;
