import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { STORE_LOOKUP_CODE_RESULTS } from 'constants/actionTypes';
import { AGENCY_CODE_SET_NAME } from 'constants/API';
import { find } from 'lodash';
import * as MOCK from 'mocks/dataMocks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { useLookupCodes } from 'store/slices/lookupCodes/useLookupCodes';

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

let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper = (store: any) => ({ children }: any) => (
  <Provider store={store}>{children}</Provider>
);

describe('getFetchLookupCodeAction action creator', () => {
  afterAll(() => {
    jest.restoreAllMocks();
  });
  const url = `/lookup/all`;
  const mockResponse = {
    data: [
      {
        code: 'AEST',
        id: '1',
        isDisabled: false,
        name: 'Ministry of Advanced Education',
        type: AGENCY_CODE_SET_NAME,
      },
    ],
  };
  it('calls the api with the expected url', () => {
    mockAxios.onGet(url).reply(200, mockResponse);
    renderHook(
      () =>
        useLookupCodes()
          .fetchLookupCodes()
          .then(() => {
            expect(mockAxios.history.get[0]).toMatchObject({
              url: '/lookup/all',
            });
          }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
  });
  it('gets all codes when paramaters contains all', () => {
    mockAxios.onGet(url).reply(200, mockResponse);
    renderHook(
      () =>
        useLookupCodes()
          .fetchLookupCodes()
          .then(() => {
            expect(find(currentStore.getActions(), { type: 'network/logRequest' })).not.toBeNull();
            expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
            expect(currentStore.getActions()).toContainEqual({
              payload: mockResponse,
              type: 'lookupCode/storeLookupCodes',
            });
          }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
  });

  it('Request failure, dispatches error with correct response', () => {
    mockAxios.onGet(url).reply(400, MOCK.ERROR);
    const mockResponse = {
      data: [
        {
          code: 'AEST',
          id: '1',
          isDisabled: false,
          name: 'Ministry of Advanced Education',
          type: AGENCY_CODE_SET_NAME,
        },
      ],
    };
    renderHook(
      () =>
        useLookupCodes()
          .fetchLookupCodes()
          .then(() => {
            expect(find(currentStore.getActions(), { type: 'network/logRequest' })).not.toBeNull();
            expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
            expect(currentStore.getActions()).not.toContainEqual({
              payload: mockResponse,
              type: STORE_LOOKUP_CODE_RESULTS,
            });
          }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
  });
});
