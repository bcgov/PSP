import { useApiLookupCodes } from './useApiLookupCodes';
import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

const mockAxios = new MockAdapter(axios);

describe('useApiLookupCodes.test.ts api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Gets paged agencies', () => {
    renderHook(async () => {
      mockAxios.onGet(`/lookup/all`).reply(200, []);

      const api = useApiLookupCodes();
      const response = await api.getLookupCodes();

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual([]);
    });
  });
});
