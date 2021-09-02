import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { DEFAULT_PAGE_SIZE } from 'components/Table/constants';
import { TableSort } from 'components/Table/TableSort';
import { IUserRecord } from 'features/admin/users/interfaces/IUserRecord';
import { IPagedItems, IUser, IUsersFilter } from 'interfaces';

import { IUsersState } from '.';

export const initialState: IUsersState = {
  pagedUsers: { page: 1, pageIndex: 0, total: 0, quantity: 0, items: [] },
  rowsPerPage: DEFAULT_PAGE_SIZE,
  filter: {},
  sort: { businessIdentifier: 'asc' },
  pageIndex: 0,
  userDetail: {
    id: 0,
    keycloakUserId: '',
    businessIdentifier: '',
    displayName: '',
    firstName: '',
    surname: '',
    email: '',
    isDisabled: false,
    organizations: [],
    roles: [],
    createdOn: '',
    rowVersion: 1,
  },
};
/**
 * The user reducer stores the complete list of lookup codes used within the application.
 */
export const usersSlice = createSlice({
  name: 'users',
  initialState: initialState,
  reducers: {
    storeUsers(state: IUsersState, action: PayloadAction<IPagedItems<IUser>>) {
      state.pagedUsers = action.payload;
    },
    storeUserDetails(state: IUsersState, action: PayloadAction<IUser>) {
      state.userDetail = action.payload;
    },
    updateUser(state: IUsersState, action: PayloadAction<IUser>) {
      const items = [
        ...state.pagedUsers.items.map((u: IUser) =>
          u.id === action.payload.id ? action.payload : u,
        ),
      ];
      state.pagedUsers.items = items;
      state.userDetail = action.payload;
    },
    setUsersFilter(state: IUsersState, action: PayloadAction<IUsersFilter>) {
      state.filter = action.payload;
    },
    setUsersPageSize(state: IUsersState, action: PayloadAction<number>) {
      state.rowsPerPage = action.payload;
    },
    setUsersPageIndex(state: IUsersState, action: PayloadAction<number>) {
      state.pageIndex = action.payload;
    },
    setUsersPageSort(state: IUsersState, action: PayloadAction<TableSort<IUserRecord>>) {
      state.sort = action.payload;
    },
  },
});

// Destructure and export the plain action creators
export const {
  storeUsers,
  storeUserDetails,
  updateUser,
  setUsersFilter,
  setUsersPageIndex,
  setUsersPageSize,
  setUsersPageSort,
} = usersSlice.actions;
