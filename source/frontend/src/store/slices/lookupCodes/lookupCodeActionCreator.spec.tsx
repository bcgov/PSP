import { waitFor } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { find, values } from 'lodash';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { ORGANIZATION_TYPES } from '@/constants/API';
import * as MOCK from '@/mocks/data.mock';
import { useLookupCodes } from '@/store/slices/lookupCodes/useLookupCodes';

import { networkSlice } from '../network/networkSlice';

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

describe('getFetchLookupCodeAction action creator', () => {
  afterAll(() => {
    jest.restoreAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useLookupCodes, { wrapper: getWrapper(getStore(values)) });
    return result.current;
  };

  const url = `/lookup/all`;
  const mockResponse = {
    data: [
      {
        code: 'AEST',
        id: '1',
        isDisabled: false,
        name: 'Ministry of Advanced Education',
        type: ORGANIZATION_TYPES,
      },
    ],
  };
  it('calls the api with the expected url', async () => {
    mockAxios.onGet(url).reply(200, mockResponse);
    const { fetchLookupCodes } = setup();
    fetchLookupCodes();
    await waitFor(async () =>
      expect(mockAxios.history.get[0]).toMatchObject({
        url: '/lookup/all',
      }),
    );
  });
  it('gets all codes when paramaters contains all', async () => {
    mockAxios.onGet(url).reply(200, mockResponse);
    const { fetchLookupCodes } = setup();
    fetchLookupCodes();
    await waitFor(async () => {
      expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logSuccess' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
      expect(currentStore.getActions()).toContainEqual({
        payload: mockResponse,
        type: 'lookupCode/storeLookupCodes',
      });
    });
  });

  it('Request failure, dispatches error with correct response', async () => {
    mockAxios.onGet(url).reply(400, MOCK.ERROR);
    const { fetchLookupCodes } = setup();
    fetchLookupCodes();
    await waitFor(async () => {
      expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
    });
  });
});
