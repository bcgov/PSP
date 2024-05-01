import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { useApiTenants } from './useApiTenants';

const mockAxios = new MockAdapter(axios);

describe('useApiTenants testing suite', () => {
  beforeEach(() => {
    mockAxios.onGet('tenants').reply(200, defaultTenant);
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiTenants);
    return result.current;
  };

  it('Get Tenant Configuration Settings', async () => {
    const { getSettings } = setup();
    const response = await getSettings();

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(defaultTenant);
  });
});

const defaultTenant = { code: 'test', name: 'test', settings: { helpDeskEmail: 'test@test.com' } };
