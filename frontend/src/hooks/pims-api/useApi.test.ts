import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { ENVIRONMENT } from 'constants/environment';
import { useApi } from '.';

const mockAxios = new MockAdapter(axios);

describe('useApi testing suite', () => {
  beforeEach(() => {
    mockAxios.onGet('success').reply(200, 'success');
    mockAxios.onGet('failure').reply(400, 'failure');
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('useApi uses custom axios with baseURL', () => {
    renderHook(() => {
      const api = useApi();

      expect(api.defaults.baseURL).toBe(ENVIRONMENT.apiUrl);
    });
  });

  it('useApi uses custom axios - success', () => {
    renderHook(async () => {
      const api = useApi();
      const response = await api.get('success');

      expect(response.status).toBe(200);
      expect(response.data).toBe('success');
    });
  });

  it('useApi uses custom axios - failure', () => {
    renderHook(async () => {
      const api = useApi();
      try {
        await api.get('failure');
      } catch (error) {
        expect(error.response.status).toBe(400);
        expect(error.response.data).toBe('failure');
      }
    });
  });
});
