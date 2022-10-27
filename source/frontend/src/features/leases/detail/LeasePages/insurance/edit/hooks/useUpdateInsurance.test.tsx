import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { IInsurance } from 'interfaces';
import { IBatchUpdateReply, IBatchUpdateRequest } from 'interfaces/batchUpdate';
import find from 'lodash/find';
import * as MOCK from 'mocks/dataMocks';
import { Provider } from 'react-redux';
import { toast } from 'react-toastify';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { networkSlice } from 'store/slices/network/networkSlice';

import { useUpdateInsurance } from './useUpdateInsurance';

const dispatch = jest.fn();
const toastSuccessSpy = jest.spyOn(toast, 'success');
const toastErrorSpy = jest.spyOn(toast, 'error');
const requestSpy = jest.spyOn(networkSlice.actions, 'logRequest');
const successSpy = jest.spyOn(networkSlice.actions, 'logSuccess');
const errorSpy = jest.spyOn(networkSlice.actions, 'logError');
const mockAxios = new MockAdapter(axios);

const defaultLeaseId = 1;
const defaultUpdateRequest: IBatchUpdateRequest<IInsurance> = { payload: [] };
const defaultUpdateReply: IBatchUpdateReply<IInsurance> = { payload: [], errorMessages: [] };

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
const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    <Provider store={store}>{children}</Provider>;

const setup = (values?: any) => {
  const { result } = renderHook(useUpdateInsurance, { wrapper: getWrapper(getStore(values)) });
  return result.current;
};

describe('useUpdateInsurance functions', () => {
  afterAll(() => {
    jest.restoreAllMocks();
  });
  describe('Update Insurance', () => {
    const url = `/leases/1/insurances?batch`;
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onPost(url).reply(200, defaultUpdateReply);

      const { batchUpdateInsurances } = setup();
      const updateReply = await batchUpdateInsurances(defaultLeaseId, defaultUpdateRequest);

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
      expect(updateReply).toEqual(defaultUpdateReply);
      expect(toastSuccessSpy).toHaveBeenCalled();
    });

    it('Request failure, dispatches error with correct response', async () => {
      mockAxios.onPost(url).reply(400, MOCK.ERROR);

      const { batchUpdateInsurances } = setup();
      await batchUpdateInsurances(defaultLeaseId, defaultUpdateRequest);

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });
});
