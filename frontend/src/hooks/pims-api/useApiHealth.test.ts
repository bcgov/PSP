import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { useApiHealth } from '.';

const mockAxios = new MockAdapter(axios);

describe('useApiHealth testing suite', () => {
  beforeEach(() => {
    mockAxios.onGet('health/env').reply(200, defaultVersion);
    mockAxios.onGet('health/live').reply(200, defaultLive);
    mockAxios.onGet('health/ready').reply(200, defaultReady);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Get Health Version endpoint', () => {
    renderHook(async () => {
      const api = useApiHealth();
      const response = await api.getVersion();

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(defaultVersion);
    });
  });

  it('Get Health Live endpoint', () => {
    renderHook(async () => {
      const api = useApiHealth();
      const response = await api.getLive();

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(defaultLive);
    });
  });

  it('Get Health Ready endpoint', () => {
    renderHook(async () => {
      const api = useApiHealth();
      const response = await api.getReady();

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(defaultReady);
    });
  });
});

const defaultVersion = { environment: 'test', version: '1.0.0.0' };
const defaultLive = { status: 'healthy' };
const defaultReady = { status: 'healthy' };
