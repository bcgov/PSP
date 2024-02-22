import { waitFor } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { find, values } from 'lodash';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as MOCK from '@/mocks/data.mock';

import { networkSlice } from '../network/networkSlice';
import { useSystemConstants } from '.';

const requestSpy = jest.spyOn(networkSlice.actions, 'logRequest');
const successSpy = jest.spyOn(networkSlice.actions, 'logSuccess');
const errorSpy = jest.spyOn(networkSlice.actions, 'logError');
const mockAxios = new MockAdapter(axios);

beforeEach(() => {
  mockAxios.reset();
  requestSpy.mockClear();
  successSpy.mockClear();
  errorSpy.mockClear();
});

let currentStore: MockStoreEnhanced<any, object>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    <Provider store={store}>{children}</Provider>;

describe('TODO: SystemConstants slice action creator', () => {
  afterAll(() => {
    jest.restoreAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useSystemConstants, { wrapper: getWrapper(getStore(values)) });
    return result.current;
  };

  const apiUrl = `/systemConstant`;
  const mockResponse = {
    data: [
      {
        name: 'DBVERSION',
        value: '17.00',
      },
    ],
  };
  xit('calls the api with the expected url', async () => {
    mockAxios.onGet(apiUrl).reply(200, mockResponse);
    const { fetchSystemConstants } = setup();
    fetchSystemConstants();
    await waitFor(
      async () =>
        expect(mockAxios.history.get[0]).toMatchObject({
          url: apiUrl,
        }),
      { timeout: 5000 },
    );
  });
  xit('gets all codes when paramaters contains all', async () => {
    mockAxios.onGet(apiUrl).reply(200, mockResponse);
    const { fetchSystemConstants } = setup();
    fetchSystemConstants();
    await waitFor(
      async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logSuccess' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
        expect(currentStore.getActions()).toContainEqual({
          payload: mockResponse,
          type: 'systemConstant/storeSystemConstants',
        });
      },
      { timeout: 5000 },
    );
  });

  xit('Request failure, dispatches error with correct response', async () => {
    mockAxios.onGet(apiUrl).reply(400, MOCK.ERROR);
    const { fetchSystemConstants } = setup();
    fetchSystemConstants();
    await waitFor(
      async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      },
      { timeout: 5000 },
    );
  });
});
