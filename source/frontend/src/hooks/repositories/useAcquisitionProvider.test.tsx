import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import find from 'lodash/find';
import { Provider } from 'react-redux';
import { toast } from 'react-toastify';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import * as MOCK from '@/mocks/data.mock';
import { networkSlice } from '@/store/slices/network/networkSlice';

import { useAcquisitionProvider } from './useAcquisitionProvider';

const dispatch = vi.fn();
const toastSuccessSpy = vi.spyOn(toast, 'success');
const toastErrorSpy = vi.spyOn(toast, 'error');
const requestSpy = vi.spyOn(networkSlice.actions, 'logRequest');
const successSpy = vi.spyOn(networkSlice.actions, 'logSuccess');
const errorSpy = vi.spyOn(networkSlice.actions, 'logError');
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
    vi.restoreAllMocks();
  });

  let url: RegExp;

  describe('addAcquisitionFile', () => {
    beforeEach(() => {
      url = /acquisitionfiles.*/;
    });

    it('Dispatches success with correct response when request is successful', async () => {
      mockAxios.onPost(url).reply(200, mockAcquisitionFileResponse(1));
      const { addAcquisitionFile } = setup();

      await act(async () => {
        const response = await addAcquisitionFile.execute(mockAcquisitionFileResponse(1), []);
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
        expect(async () => {
          await addAcquisitionFile.execute(mockAcquisitionFileResponse(1), []);
        }).rejects.toThrow();
      });
    });
  });

  describe('getAcquisitionFile', () => {
    beforeEach(() => {
      url = /acquisitionfiles.*/;
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

    it('Dispatches error with correct response when request fails with 500', async () => {
      mockAxios.onGet(url).reply(500, MOCK.ERROR);
      const { getAcquisitionFile } = setup();

      await act(async () => {
        await getAcquisitionFile.execute(mockAcquisitionFileResponse().id || 0);
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });

    it('Dispatches error with correct response when request fails with 400', async () => {
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const { getAcquisitionFile } = setup();

      await act(async () => {
        await getAcquisitionFile.execute(mockAcquisitionFileResponse().id || 0);
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBe(undefined);
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });
});
