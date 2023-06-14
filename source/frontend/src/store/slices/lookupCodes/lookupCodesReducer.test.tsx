import { ORGANIZATION_TYPES } from '@/constants/API';

import { initialState, lookupCodesSlice } from './lookupCodesSlice';
describe('lookup code slice reducer functionality', () => {
  const lookupCodeReducer = lookupCodesSlice.reducer;
  it('saves the list of lookup codes', () => {
    const result = lookupCodeReducer(undefined, {
      type: lookupCodesSlice.actions.storeLookupCodes,
      payload: [
        {
          code: 'AEST',
          id: '1',
          isDisabled: false,
          name: 'Ministry of Advanced Education',
          type: ORGANIZATION_TYPES,
        },
      ],
    });
    expect(result).toEqual({
      ...initialState,
      lookupCodes: [
        {
          code: 'AEST',
          id: '1',
          isDisabled: false,
          name: 'Ministry of Advanced Education',
          type: ORGANIZATION_TYPES,
        },
      ],
    });
  });
});
