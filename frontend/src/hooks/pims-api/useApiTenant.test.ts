import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { useApiTenants } from '.';

const mockAxios = new MockAdapter(axios);

describe('useApiTenants testing suite', () => {
  beforeEach(() => {
    mockAxios.onGet('tenants').reply(200, defaultTenant);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Get Tenant Configuration Settings', () => {
    renderHook(async () => {
      const api = useApiTenants();
      const response = await api.getSettings();

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(defaultTenant);
    });
  });
});

const defaultTenant = { code: 'test', name: 'test', settings: { helpDeskEmail: 'test@test.com' } };
