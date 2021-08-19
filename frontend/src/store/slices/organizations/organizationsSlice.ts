import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { DEFAULT_PAGE_SIZE } from 'components/Table/constants';
import { IOrganization, IOrganizationDetail, IPagedItems } from 'interfaces';

import { IOrganizationsState } from '.';

export const initialState: IOrganizationsState = {
  pagedOrganizations: { page: 1, pageIndex: 0, total: 0, quantity: 0, items: [] },
  rowsPerPage: DEFAULT_PAGE_SIZE,
  filter: {},
  sort: { name: 'asc' },
  pageIndex: 0,
  organizationDetail: {
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
 * The organization code reducer stores the complete list of organization codes used within the application.
 */
export const organizationsSlice = createSlice({
  name: 'organizations',
  initialState: initialState,
  reducers: {
    storeOrganizations(
      state: IOrganizationsState,
      action: PayloadAction<IPagedItems<IOrganization>>,
    ) {
      state.pagedOrganizations = action.payload;
    },
    storeOrganizationDetails(
      state: IOrganizationsState,
      action: PayloadAction<IOrganizationDetail>,
    ) {
      state.organizationDetail = action.payload;
    },
  },
});

// Destructure and export the plain action creators
export const { storeOrganizations, storeOrganizationDetails } = organizationsSlice.actions;
