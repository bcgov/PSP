import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import find from 'lodash/find';
import * as MOCK from 'mocks/dataMocks';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { Provider } from 'react-redux';
import { toast } from 'react-toastify';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { networkSlice } from 'store/slices/network/networkSlice';

import { useAcquisitionProvider } from './useAcquisitionProvider';

const dispatch = jest.fn();
const toastSuccessSpy = jest.spyOn(toast, 'success');
const toastErrorSpy = jest.spyOn(toast, 'error');
const requestSpy = jest.spyOn(networkSlice.actions, 'logRequest');
const successSpy = jest.spyOn(networkSlice.actions, 'logSuccess');
const errorSpy = jest.spyOn(networkSlice.actions, 'logError');
const mockAxios = new MockAdapter(axios);

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

describe('useAcquisitionProvider hook', () => {
  const setup = (values?: any) => {
    const { result } = renderHook(useAcquisitionProvider, {
      wrapper: getWrapper(getStore(values)),
    });
    return result.current;
  };

  beforeEach(() => {
    mockAxios.reset();
    dispatch.mockClear();
    requestSpy.mockClear();
    successSpy.mockClear();
    errorSpy.mockClear();
  });

  afterAll(() => {
    jest.restoreAllMocks();
  });

  let url: string;

  describe('addAcquisitionFile', () => {
    beforeEach(() => {
      url = `/acquisitionfiles`;
    });

    it('Dispatches success with correct response when request is successful', async () => {
      mockAxios.onPost(url).reply(200, mockAcquisitionFileResponse(1));
      const { addAcquisitionFile } = setup();

      await act(async () => {
        const response = await addAcquisitionFile.execute(mockAcquisitionFileResponse(1));
        expect(response).toEqual(mockAcquisitionFileResponse(1));
      });

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
      expect(toastSuccessSpy).toHaveBeenCalledWith('Acquisition File saved');
    });

    it('Dispatches error with correct response when request fails', async () => {
      mockAxios.onPost(url).reply(400, MOCK.ERROR);
      const { addAcquisitionFile } = setup();

      await act(async () => {
        await addAcquisitionFile.execute(mockAcquisitionFileResponse(1));
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });

  describe('getAcquisitionFile', () => {
    beforeEach(() => {
      url = `/acquisitionfiles/1`;
    });

    it('Dispatches success with correct response when request is successful', async () => {
      mockAxios.onGet(url).reply(200, mockAcquisitionFileResponse(1));
      const { getAcquisitionFile } = setup();

      await act(async () => {
        const response = await getAcquisitionFile.execute(1);
        expect(response).toEqual(mockAcquisitionFileResponse(1));
      });

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
    });

    it('Dispatches error with correct response when request fails', async () => {
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const { getAcquisitionFile } = setup();

      await act(async () => {
        await getAcquisitionFile.execute(mockAcquisitionFileResponse());
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });
});
