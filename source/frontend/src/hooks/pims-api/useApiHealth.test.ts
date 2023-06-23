import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { useApiHealth } from './useApiHealth';

const mockAxios = new MockAdapter(axios);

describe('useApiHealth testing suite', () => {
  beforeEach(() => {
    mockAxios.onGet('health/env').reply(200, defaultVersion);
    mockAxios.onGet('health/live').reply(200, defaultLive);
    mockAxios.onGet('health/ready').reply(200, defaultReady);
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiHealth);
    return result.current;
  };

  it('Get Health Version endpoint', async () => {
    const { getVersion } = setup();
    const response = await getVersion();

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(defaultVersion);
  });

  it('Get Health Live endpoint', async () => {
    const { getLive } = setup();
    const response = await getLive();

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(defaultLive);
  });

  it('Get Health Ready endpoint', async () => {
    const { getReady } = setup();
    const response = await getReady();

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(defaultReady);
  });
});

const defaultVersion = { environment: 'test', version: '1.0.0.0' };
const defaultLive = { status: 'healthy' };
const defaultReady = { status: 'healthy' };
