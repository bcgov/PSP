import { mockAccessRequest } from 'mocks/filterDataMock';
import { accessRequestsSlice, initialState } from 'store/slices/accessRequests/accessRequestsSlice';

const accessRequestReducer = accessRequestsSlice.reducer;
describe('access request reducer functionality', () => {
  it('stores access requests', () => {
    const result = accessRequestReducer(undefined, {
      type: accessRequestsSlice.actions.storeAccessRequests,
      payload: {
        page: 1,
        pageIndex: 0,
        total: 0,
        quantity: 0,
        items: [mockAccessRequest],
      },
    });
    expect(result).toEqual({
      ...initialState,
      pagedAccessRequests: {
        page: 1,
        pageIndex: 0,
        total: 0,
        quantity: 0,
        items: [mockAccessRequest],
      },
    });
  });
  it('stores an access request', () => {
    const result = accessRequestReducer(undefined, {
      type: accessRequestsSlice.actions.storeAccessRequest,
      payload: mockAccessRequest,
    });
    expect(result).toEqual({
      ...initialState,
      accessRequest: mockAccessRequest,
    });
  });
  it('allows an admin to delete an access request', () => {
    const result = accessRequestReducer(
      {
        ...initialState,
        pagedAccessRequests: {
          page: 1,
          pageIndex: 0,
          total: 0,
          quantity: 0,
          items: [mockAccessRequest],
        },
      },
      {
        type: accessRequestsSlice.actions.deleteAccessRequest,
        payload: 2,
      },
    );
    expect(result).toEqual({
      ...initialState,
      pagedAccessRequests: {
        page: 1,
        pageIndex: 0,
        total: 0,
        quantity: 0,
        items: [],
      },
    });
  });
  it('allows an admin to update request access status', () => {
    const result = accessRequestReducer(
      {
        ...initialState,
        pagedAccessRequests: {
          page: 1,
          pageIndex: 0,
          total: 0,
          quantity: 0,
          items: [mockAccessRequest],
        },
      },
      {
        type: accessRequestsSlice.actions.updateAccessRequestsAdmin,
        payload: mockAccessRequest,
      },
    );
    expect(result).toEqual({
      ...initialState,
      pagedAccessRequests: {
        page: 1,
        pageIndex: 0,
        total: 0,
        quantity: 0,
        items: [],
      },
    });
  });

  it('allows the page size to be updated', () => {
    const result = accessRequestReducer(undefined, {
      type: accessRequestsSlice.actions.updateAccessRequestPageSize,
      payload: 2,
    });
    expect(result).toEqual({
      ...initialState,
      pageSize: 2,
    });
  });

  it('allows the page index to be updated', () => {
    const result = accessRequestReducer(undefined, {
      type: accessRequestsSlice.actions.updateAccessRequestPageIndex,
      payload: 1,
    });
    expect(result).toEqual({
      ...initialState,
      pageSize: 100,
      pageIndex: 1,
    });
  });

  it('allows an admin to update the access request filter', () => {
    const result = accessRequestReducer(undefined, {
      type: accessRequestsSlice.actions.filterAccessRequestsAdmin,
      payload: { agency: '1', role: '2', searchText: '3' },
    });
    expect(result).toEqual({
      ...initialState,
      filter: { agency: '1', role: '2', searchText: '3' },
    });
  });
});
