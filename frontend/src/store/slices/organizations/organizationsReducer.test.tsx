import { initialState, organizationsSlice } from './organizationsSlice';
describe('lookup code slice reducer functionality', () => {
  const organizationReducer = organizationsSlice.reducer;
  it('saves the list of organizations', () => {
    const result = organizationReducer(undefined, {
      type: organizationsSlice.actions.storeOrganizations,
      payload: {
        pageIndex: 0,
        page: 1,
        quantity: 1,
        total: 1,
        items: [
          {
            code: 'AEST',
            id: 1,
            name: 'Ministry of Advanced Education',
          },
        ],
      },
    });
    expect(result).toEqual({
      ...initialState,
      pagedOrganizations: {
        pageIndex: 0,
        page: 1,
        quantity: 1,
        total: 1,
        items: [
          {
            code: 'AEST',
            id: 1,
            name: 'Ministry of Advanced Education',
          },
        ],
      },
    });
  });
  it('saves the organization detail', () => {
    const result = organizationReducer(undefined, {
      type: organizationsSlice.actions.storeOrganizationDetails,
      payload: {
        parentId: 2,
        code: '',
        id: 1,
        name: '',
        description: '',
      },
    });
    expect(result).toEqual({
      ...initialState,
      organizationDetail: {
        parentId: 2,
        code: '',
        id: 1,
        name: '',
        description: '',
      },
    });
  });
});
