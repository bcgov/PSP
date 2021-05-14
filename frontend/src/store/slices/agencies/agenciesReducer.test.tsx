import { agenciesSlice, initialState } from './agenciesSlice';
describe('lookup code slice reducer functionality', () => {
  const agencyReducer = agenciesSlice.reducer;
  it('saves the list of agencies', () => {
    const result = agencyReducer(undefined, {
      type: agenciesSlice.actions.storeAgencies,
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
      pagedAgencies: {
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
  it('saves the agency detail', () => {
    const result = agencyReducer(undefined, {
      type: agenciesSlice.actions.storeAgencyDetails,
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
      agencyDetail: {
        parentId: 2,
        code: '',
        id: 1,
        name: '',
        description: '',
      },
    });
  });
});
