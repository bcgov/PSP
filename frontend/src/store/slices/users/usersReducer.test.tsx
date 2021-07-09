import { initialState, usersSlice } from './usersSlice';
describe('users slice reducer functionality', () => {
  const userReducer = usersSlice.reducer;
  const mockUser = {
    id: 1,
    key: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
    rowVersion: 1,
    displayName: 'User, Admin',
    firstName: 'Admin',
    lastName: 'User',
    email: 'admin@pims.gov.bc.ca',
    username: 'admin',
    position: '',
    createdOn: '2021-05-04T19:07:09.6920606',
  };
  it('saves the list of users', () => {
    const result = userReducer(undefined, {
      type: usersSlice.actions.storeUsers,
      payload: {
        pageIndex: 0,
        page: 1,
        quantity: 1,
        total: 1,
        items: [mockUser],
      },
    });
    expect(result).toEqual({
      ...initialState,
      pagedUsers: {
        pageIndex: 0,
        page: 1,
        quantity: 1,
        total: 1,
        items: [mockUser],
      },
    });
  });
  it('saves the user detail', () => {
    const result = userReducer(undefined, {
      type: usersSlice.actions.storeUserDetails,
      payload: mockUser,
    });
    expect(result).toEqual({
      ...initialState,
      userDetail: mockUser,
    });
  });

  it('allows an admin to update users status', () => {
    const result = userReducer(
      {
        ...initialState,
        pagedUsers: {
          page: 1,
          pageIndex: 0,
          total: 0,
          quantity: 0,
          items: [mockUser],
        },
      },
      {
        type: usersSlice.actions.updateUser,
        payload: { ...mockUser, firstName: 'George' },
      },
    );
    expect(result).toEqual({
      ...initialState,
      pagedUsers: {
        page: 1,
        pageIndex: 0,
        total: 0,
        quantity: 0,
        items: [{ ...mockUser, firstName: 'George' }],
      },
    });
  });

  it('allows the page size to be updated', () => {
    const result = userReducer(undefined, {
      type: usersSlice.actions.setUsersPageSize,
      payload: 2,
    });
    expect(result).toEqual({
      ...initialState,
      rowsPerPage: 2,
    });
  });

  it('allows the page index to be updated', () => {
    const result = userReducer(undefined, {
      type: usersSlice.actions.setUsersPageIndex,
      payload: 1,
    });
    expect(result).toEqual({
      ...initialState,
      pageIndex: 1,
    });
  });

  it('allows an admin to update the users filter', () => {
    const result = userReducer(undefined, {
      type: usersSlice.actions.setUsersFilter,
      payload: { agency: '1', role: '2', searchText: '3' },
    });
    expect(result).toEqual({
      ...initialState,
      filter: { agency: '1', role: '2', searchText: '3' },
    });
  });
});
