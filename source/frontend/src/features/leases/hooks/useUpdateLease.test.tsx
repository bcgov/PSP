import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import find from 'lodash/find';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as MOCK from '@/mocks/data.mock';
import { defaultApiLease } from '@/models/api/Lease';

import { useUpdateLease } from './useUpdateLease';

const dispatch = jest.fn();
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
  const { result } = renderHook(useUpdateLease, { wrapper: getWrapper(getStore(values)) });
  return result.current;
};
const defaultLeaseWithId = { ...defaultApiLease, id: 1 };

describe('useUpdateLease functions', () => {
  afterAll(() => {
    jest.restoreAllMocks();
  });
  describe('updateLease', () => {
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onPut().reply(200, defaultLeaseWithId);

      const { updateApiLease } = setup();
      const leaseResponse = await updateApiLease.execute(defaultLeaseWithId, []);

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
      expect(leaseResponse).toEqual(defaultLeaseWithId);
    });

    it('400 Request failure, dispatches error with correct response', async () => {
      mockAxios.onPut().reply(400, MOCK.ERROR);

      const { updateApiLease } = setup();
      expect(() => updateApiLease.execute(defaultLeaseWithId, [])).rejects.toThrow();
    });
  });
});
