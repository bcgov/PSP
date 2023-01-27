import { initialState, tenantsSlice } from '.';

describe('tenants slice reducer functionality', () => {
  const tenantReducer = tenantsSlice.reducer;
  const mockTenant = {
    code: 'test',
    name: 'test',
    settings: {
      helpDeskEmail: 'test@test.com',
    },
  };

  it('store tenant configuration settings', () => {
    const result = tenantReducer(undefined, {
      type: tenantsSlice.actions.storeSettings,
      payload: mockTenant,
    });
    expect(result).toEqual({
      ...initialState,
      config: mockTenant,
    });
  });
});
