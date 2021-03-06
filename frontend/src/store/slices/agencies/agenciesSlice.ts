import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { DEFAULT_PAGE_SIZE } from 'components/Table/constants';
import { IAgency, IAgencyDetail, IPagedItems } from 'interfaces';

import { IAgenciesState } from '.';

export const initialState: IAgenciesState = {
  pagedAgencies: { page: 1, pageIndex: 0, total: 0, quantity: 0, items: [] },
  rowsPerPage: DEFAULT_PAGE_SIZE,
  filter: {},
  sort: { name: 'asc' },
  pageIndex: 0,
  agencyDetail: {
    parentId: -1,
    code: '',
    id: -1,
    name: '',
    description: '',
    email: '',
    addressTo: '',
    isDisabled: false,
    sendEmail: false,
    rowVersion: 1,
  },
};
/**
 * The agency code reducer stores the complete list of agency codes used within the application.
 */
export const agenciesSlice = createSlice({
  name: 'agencies',
  initialState: initialState,
  reducers: {
    storeAgencies(state: IAgenciesState, action: PayloadAction<IPagedItems<IAgency>>) {
      state.pagedAgencies = action.payload;
    },
    storeAgencyDetails(state: IAgenciesState, action: PayloadAction<IAgencyDetail>) {
      state.agencyDetail = action.payload;
    },
  },
});

// Destructure and export the plain action creators
export const { storeAgencies, storeAgencyDetails } = agenciesSlice.actions;
