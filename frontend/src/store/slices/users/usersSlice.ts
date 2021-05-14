import { IUsersState } from '.';
import { USERS } from '../../../constants/reducerTypes';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { IPagedItems, IUserDetails, IUser, IUsersFilter } from 'interfaces';
import { DEFAULT_PAGE_SIZE } from 'components/Table/constants';
import { TableSort } from 'components/Table/TableSort';
import { IUserRecord } from 'features/admin/users/interfaces/IUserRecord';

export const initialState: IUsersState = {
  pagedUsers: { page: 1, pageIndex: 0, total: 0, quantity: 0, items: [] },
  rowsPerPage: DEFAULT_PAGE_SIZE,
  filter: {},
  sort: { username: 'asc' },
  pageIndex: 0,
  userDetail: {
    id: '',
    username: '',
    displayName: '',
    firstName: '',
    lastName: '',
    email: '',
    isDisabled: false,
    agencies: [],
    roles: [],
    createdOn: '',
    rowVersion: '',
  },
};
/**
 * The user reducer stores the complete list of lookup codes used within the application.
 */
export const usersSlice = createSlice({
  name: USERS,
  initialState: initialState,
  reducers: {
    storeUsers(state: IUsersState, action: PayloadAction<IPagedItems<IUser>>) {
      state.pagedUsers = action.payload;
    },
    storeUserDetails(state: IUsersState, action: PayloadAction<IUserDetails>) {
      state.userDetail = action.payload;
    },
    updateUser(state: IUsersState, action: PayloadAction<IUser>) {
      const items = [
        ...state.pagedUsers.items.map((u: IUser) =>
          u.id === action.payload.id ? action.payload : u,
        ),
      ];
      state.pagedUsers.items = items;
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
