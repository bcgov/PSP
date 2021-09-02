import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { enableFetchMocks } from 'jest-fetch-mock';
import React from 'react';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { store as realStore } from 'store/store';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { useApi } from './useApi';

enableFetchMocks();
const mockAxios = new MockAdapter(axios);
const mockStore = configureMockStore([thunk]);

const getUseApiHook = (store?: any) => {
  const { result } = renderHook(() => useApi(), {
    wrapper: ({ children }) => <TestCommonWrapper store={store}>{children}</TestCommonWrapper>,
  });
  return result;
};

describe('useApi hook', () => {
  beforeEach(() => {
    fetchMock.mockResponse(JSON.stringify({ status: 200, body: {} }));
  });
  describe('useApi axios functionality', () => {
    it('attaches jwt header', async () => {
      const store = mockStore({});
      mockAxios.onGet().reply(200, {});

      const api = getUseApiHook({ store });
      await api.current.isPidAvailable(undefined, '123-456-789');
      expect(mockAxios.history.get[0]).toMatchObject({
        headers: {
          Accept: 'application/json, text/plain, */*',
          'Access-Control-Allow-Origin': '*',
          Authorization: 'Bearer ',
        },
      });
    });
    it('dispatches hide loading bar action on response', async () => {
      const store = mockStore({});
      mockAxios.onGet().reply(200, {});

      const api = getUseApiHook({ store });
      await api.current.isPidAvailable(undefined, '123-456-789');
      expect(realStore.getState().loadingBar).toStrictEqual({});
    });
    it('dispatches hide loading bar action on error response', async () => {
      const store = mockStore({});
      mockAxios.onGet().reply(400, {});

      const api = getUseApiHook({ store });
      try {
        await api.current.isPidAvailable(undefined, '123-456-789');
      } catch (e) {}
      expect(realStore.getState().loadingBar).toStrictEqual({});
    });
  });
  describe('useApi common PSP network calls', () => {
    afterEach(() => {
      jest.clearAllMocks();
      mockAxios.resetHistory();
    });

    it('calls isPidAvailable with expected parameters', async () => {
      mockAxios.onGet().reply(200, {});
      const api = getUseApiHook();
      await api.current.isPidAvailable(undefined, '123-456-789');
      expect(mockAxios.history.get[0]).toMatchObject({
        url: '/api/properties/parcels/check/pid-available?pid=123456789',
      });
    });

    it('calls isPinAvailable with expected parameters', async () => {
      mockAxios.onGet().reply(200, {});
      const api = getUseApiHook();
      await api.current.isPinAvailable(undefined, 123456789);
      expect(mockAxios.history.get[0]).toMatchObject({
        url: '/api/properties/parcels/check/pin-available?pin=123456789',
      });
    });

    it('calls searchAddress with expected parameters', async () => {
      mockAxios.onGet().reply(200, {});
      const api = getUseApiHook();
      await api.current.searchAddress('1234 Fake st.');
      expect(mockAxios.history.get[0]).toMatchObject({
        url: '/api/tools/geocoder/addresses?address=1234 Fake st.+BC',
      });
    });

    it('calls getSitePids with expected parameters', async () => {
      mockAxios.onGet().reply(200, {});
      const api = getUseApiHook();
      await api.current.getSitePids('siteid');
      expect(mockAxios.history.get[0]).toMatchObject({
        url: '/api/tools/geocoder/parcels/pids/siteid',
      });
    });

    it('calls getNearestAddress with expected parameters', async () => {
      mockAxios.onGet().reply(200, {});
      const api = getUseApiHook();
      await api.current.getNearestAddress({ lat: 1, lng: 2 });
      expect(mockAxios.history.get[0]).toMatchObject({
        url: '/api/tools/geocoder/nearest?point=2,1',
      });
    });

    it('calls getNearAddresses with expected parameters', async () => {
      mockAxios.onGet().reply(200, {});
      const api = getUseApiHook();
      await api.current.getNearAddresses({ lat: 1, lng: 2 });
      expect(mockAxios.history.get[0]).toMatchObject({
        url: '/api/tools/geocoder/near?point=2,1',
      });
    });
  });
});
