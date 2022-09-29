import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { defaultLease } from 'interfaces';
import find from 'lodash/find';
import * as MOCK from 'mocks/dataMocks';
import { Provider } from 'react-redux';
import { toast } from 'react-toastify';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { networkSlice } from 'store/slices/network/networkSlice';

import { useAddLease } from './useAddLease';

const dispatch = jest.fn();
const toastSuccessSpy = jest.spyOn(toast, 'success');
const toastErrorSpy = jest.spyOn(toast, 'error');
const requestSpy = jest.spyOn(networkSlice.actions, 'logRequest');
const successSpy = jest.spyOn(networkSlice.actions, 'logSuccess');
const errorSpy = jest.spyOn(networkSlice.actions, 'logError');
const mockAxios = new MockAdapter(axios);

beforeEach(() => {
  mockAxios.reset();
  dispatch.mockClear();
  requestSpy.mockClear();
  successSpy.mockClear();
  errorSpy.mockClear();
});
let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper = (store: any) => ({ children }: any) => (
  <Provider store={store}>{children}</Provider>
);

const setup = (values?: any) => {
  const { result } = renderHook(useAddLease, { wrapper: getWrapper(getStore(values)) });
  return result.current;
};

describe('useAddLease functions', () => {
  afterAll(() => {
    jest.restoreAllMocks();
  });
  describe('addLease', () => {
    const url = `/leases?userOverride=false`;
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onPost(url).reply(200, defaultLease);

      const { addLease } = setup();
      const leaseResponse = await addLease(defaultLease);

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
      expect(leaseResponse).toEqual(defaultLease);
      expect(toastSuccessSpy).toHaveBeenCalled();
    });

    it('Request failure, dispatches error with correct response', async () => {
      mockAxios.onPost(url).reply(400, MOCK.ERROR);

      const { addLease } = setup();
      await addLease(defaultLease);

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });
});
