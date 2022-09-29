import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { useApiLookupCodes } from './useApiLookupCodes';

const mockAxios = new MockAdapter(axios);

describe('useApiLookupCodes.test.ts api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiLookupCodes);
    return result.current;
  };

  it('Gets paged organizations', async () => {
    mockAxios.onGet(`/lookup/all`).reply(200, []);

    const { getLookupCodes } = setup();
    const response = await getLookupCodes();

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual([]);
  });
});
