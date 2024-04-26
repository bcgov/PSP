import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import find from 'lodash/find';
import { Provider } from 'react-redux';
import { toast } from 'react-toastify';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as MOCK from '@/mocks/data.mock';
import {
  mockDispositionFilePropertyResponse,
  mockDispositionFileResponse,
} from '@/mocks/dispositionFiles.mock';
import { networkSlice } from '@/store/slices/network/networkSlice';

import { useDispositionProvider } from './useDispositionProvider';

const dispatch = vi.fn();
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

describe('useDispositionProvider hook', () => {
  const setup = (values?: any) => {
    const { result } = renderHook(useDispositionProvider, {
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

  describe('getDispositionFile', () => {
    beforeEach(() => {
      url = /dispositionfiles.*/;
    });

    it('Dispatches success with correct response when request is successful', async () => {
      mockAxios.onGet(url).reply(200, mockDispositionFileResponse(1));
      const { getDispositionFile } = setup();

      await act(async () => {
        const response = await getDispositionFile.execute(1);
        expect(response).toEqual(mockDispositionFileResponse(1));
      });

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
    });

    it('Dispatches error with correct response when request fails with a 500', async () => {
      mockAxios.onGet(url).reply(500, MOCK.ERROR);
      const { getDispositionFile } = setup();

      await act(async () => {
        await getDispositionFile.execute(mockDispositionFileResponse().id || 0);
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });

    it('Dispatches error with correct response when request fails with a 400', async () => {
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const { getDispositionFile } = setup();

      await act(async () => {
        await getDispositionFile.execute(mockDispositionFileResponse().id || 0);
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBe(undefined);
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });

  describe('getDispositionFileProperties', () => {
    beforeEach(() => {
      url = /dispositionfiles.*/;
    });

    it('Dispatches success with correct response when request is successful', async () => {
      mockAxios.onGet(url).reply(200, mockDispositionFilePropertyResponse());
      const { getDispositionProperties } = setup();

      await act(async () => {
        const response = await getDispositionProperties.execute(1);
        expect(response).toEqual(mockDispositionFilePropertyResponse());
      });

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
    });

    it('Dispatches error with correct response when request fails with a 500', async () => {
      mockAxios.onGet(url).reply(500, MOCK.ERROR);
      const { getDispositionProperties } = setup();

      await act(async () => {
        await getDispositionProperties.execute(mockDispositionFilePropertyResponse()[0].fileId);
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });

    it('Dispatches error with correct response when request fails with a 400', async () => {
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const { getDispositionProperties } = setup();

      await act(async () => {
        await getDispositionProperties.execute(mockDispositionFilePropertyResponse()[0].fileId);
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBe(undefined);
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });
});
