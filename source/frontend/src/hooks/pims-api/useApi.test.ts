import { waitFor } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import axios, { AxiosError } from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { ENVIRONMENT } from '@/constants/environment';

import useAxiosApi from './useApi';

const mockAxios = new MockAdapter(axios);

describe('useApi testing suite', () => {
  afterEach(() => {
    jest.clearAllMocks();
    mockAxios.resetHistory();
  });

  it('useApi uses custom axios with baseURL', () => {
    renderHook(() => {
      const api = useAxiosApi();

      expect(api.defaults.baseURL).toBe(ENVIRONMENT.apiUrl);
    });
  });

  it('useApi uses custom axios - success', async () => {
    mockAxios.onGet('success').reply(200, 'success');

    const {
      result: { current: api },
    } = renderHook(() => {
      return useAxiosApi();
    });

    await waitFor(async () => {
      const response = await api.get('success');

      expect(response.status).toBe(200);
      expect(response.data).toBe('success');
      expect(mockAxios.history.get).toHaveLength(1);
    });
  });

  it('useApi uses custom axios - failure', async () => {
    mockAxios.onGet('failure').reply(400, 'failure');
    const {
      result: { current: api },
    } = renderHook(() => {
      return useAxiosApi();
    });

    try {
      await api.get('failure');
    } catch (e) {
      const error = e as AxiosError;
      expect(error.response?.status).toBe(400);
      expect(error.response?.data).toBe('failure');
      expect(mockAxios.history.get).toHaveLength(1);
    }
  });
});
