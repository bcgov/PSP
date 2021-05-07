import { IAccessRequestsFilterData, IAccessRequestsState } from './interfaces';
import { IAccessRequest, IPagedItems } from 'interfaces';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { ACCESS_REQUEST } from 'constants/reducerTypes';

export const MAX_ACCESS_RESULTS_PER_PAGE = 100;
// First, define the reducer and action creators via `createSlice`

/**
 * convert an unstructured object into a properly formatted access request.
 * @param values
 */
export const toAccessRequest = (values: any): IAccessRequest => {
  return {
    id: values.id,
    userId: values.userId,
    user: {
      id: values.userId,
      username: values.user.username,
      email: values.user.email,
      position: values.user.position,
    },
    agencies: isNaN(values.agency) ? [] : [{ id: parseInt(values.agency) }],
    roles: [{ id: values.role }],
    status: values.status,
    note: values.note,
    rowVersion: values.rowVersion,
  };
};

export const initialState: IAccessRequestsState = {
  pagedAccessRequests: { page: 1, pageIndex: 0, total: 0, quantity: 0, items: [] },
  filter: { agency: '', role: '', searchText: '' },
  sorting: { column: 'username', direction: 'desc' },
  selections: [],
  accessRequest: null,
  pageSize: MAX_ACCESS_RESULTS_PER_PAGE,
  pageIndex: 0,
};
/**
 * The access request slice manages the current access request, a list of all active access requests, and methods to sort and filter this list.
 */
export const accessRequestsSlice = createSlice({
  name: ACCESS_REQUEST,
  initialState: initialState,
  reducers: {
    storeAccessRequests(
      state: IAccessRequestsState,
      action: PayloadAction<IPagedItems<IAccessRequest>>,
    ) {
      state.pagedAccessRequests = action.payload;
    },
    storeAccessRequest(state: IAccessRequestsState, action: PayloadAction<IAccessRequest>) {
      state.accessRequest = action.payload;
    },
    deleteAccessRequest(state: IAccessRequestsState, action: PayloadAction<number>) {
      state.pagedAccessRequests.items = state.pagedAccessRequests.items.filter(
        (accessRequest: { id: number }) => accessRequest.id !== action.payload,
      );
    },
    updateAccessRequestsAdmin(state: IAccessRequestsState, action: PayloadAction<IAccessRequest>) {
      state.pagedAccessRequests.items = state.pagedAccessRequests.items.filter(
        (accessRequest: { id: number }) => accessRequest.id !== action.payload.id,
      );
    },
    updateAccessRequestPageSize(state: IAccessRequestsState, action: PayloadAction<number>) {
      state.pageSize = action.payload;
    },
    updateAccessRequestPageIndex(state: IAccessRequestsState, action: PayloadAction<number>) {
      state.pageIndex = action.payload;
    },
    filterAccessRequestsAdmin(
      state: IAccessRequestsState,
      action: PayloadAction<IAccessRequestsFilterData>,
    ) {
      state.filter = action.payload;
    },
  },
});

// Destructure and export the plain action creators
export const {
  storeAccessRequests,
  storeAccessRequest,
  deleteAccessRequest,
  updateAccessRequestsAdmin,
  updateAccessRequestPageSize,
  updateAccessRequestPageIndex,
  filterAccessRequestsAdmin,
} = accessRequestsSlice.actions;
