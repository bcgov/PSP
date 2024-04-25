import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import find from 'lodash/find';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as MOCK from '@/mocks/data.mock';
import { defaultApiLease } from '@/models/defaultInitializers';

import { useAddLease } from './useAddLease';
import { act } from '@testing-library/react';

const dispatch = vi.fn();
const mockAxios = new MockAdapter(axios);

beforeEach(() => {
  mockAxios.reset();
  dispatch.mockClear();
});
let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    <Provider store={store}>{children}</Provider>;

const setup = (values?: any) => {
  const { result } = renderHook(useAddLease, { wrapper: getWrapper(getStore(values)) });
  return result.current;
};

describe('useAddLease functions', () => {
  afterAll(() => {
    vi.restoreAllMocks();
  });
  describe('addLease', () => {
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onPost().reply(200, defaultApiLease());

      const { addLease } = setup();
      await act(async () => {
        const leaseResponse = await addLease.execute(defaultApiLease(), []);

        expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
        expect(leaseResponse).toEqual(defaultApiLease());
      });
    });

    it('Request failure, dispatches error with correct response', async () => {
      mockAxios.onPost().reply(400, MOCK.ERROR);

      const { addLease } = setup();
      await act(async () => {
        expect(async () => await addLease.execute(defaultApiLease(), [])).rejects.toThrow();
      });
    });
  });
});
