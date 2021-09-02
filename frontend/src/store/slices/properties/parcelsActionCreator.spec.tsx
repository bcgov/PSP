import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { find } from 'lodash';
import * as MOCK from 'mocks/dataMocks';
import { mockParcel, mockParcelDetail } from 'mocks/filterDataMock';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { networkSlice } from '../network/networkSlice';
import { useProperties } from './useProperties';

const dispatch = jest.fn();
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

describe('useProperties functions', () => {
  afterAll(() => {
    jest.restoreAllMocks();
  });
  describe('getProperties action creator', () => {
    const url = '/properties/search?';
    const mockResponse = { data: [mockParcel] };
    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .getProperties(null)
            .then(() => {
              expect(mockAxios.history.get[0]).toMatchObject({
                url: '/properties/search?',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
    it('Null Params - Request successful, dispatches success with correct response', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .getProperties(null)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'properties/storeProperties',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request successful, dispatches error with correct response', () => {
      mockAxios.onGet(url).reply(400, mockResponse);
      renderHook(
        () =>
          useProperties()
            .getProperties(null)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'properties/storeProperties',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });
  describe('getProperty action creator', () => {
    const url = '/properties/1';
    const mockResponse = { data: mockParcel };
    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .getProperty(mockParcel.id as number)
            .then(() => {
              expect(mockAxios.history.get[0]).toMatchObject({
                url: '/properties/1',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
    it('Request successful, dispatches success with correct response', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .getProperty(mockParcel.id as number)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: { position: undefined, property: mockResponse },
                type: 'properties/storeProperty',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .getProperty(mockParcel.id as number)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: { position: undefined, property: mockResponse },
                type: 'properties/storePropertyDetail',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('createProperty action creator', () => {
    const url = '/properties';
    const mockResponse = { data: mockParcelDetail };
    it('calls the api with the expected url', () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .createProperty(mockParcel)
            .then(() => {
              expect(mockAxios.history.post[0]).toMatchObject({
                url: '/properties',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
    it('Request successful, dispatches success with correct response', () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .createProperty(mockParcel)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'properties/storeProperty',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request successful, dispatches error with correct response', () => {
      mockAxios.onPost(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .createProperty(mockParcel)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'STORE_BUILDING_DETAILS',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('updateProperty action creator', () => {
    const url = '/properties/1';
    const mockResponse = { data: mockParcelDetail };
    it('calls the api with the expected url', () => {
      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .updateProperty(mockParcel)
            .then(() => {
              expect(mockAxios.history.put[0]).toMatchObject({
                url: '/properties/1',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
    it('Request successful, dispatches success with correct response', () => {
      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .updateProperty(mockParcel)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'properties/storeProperty',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request successful, dispatches error with correct response', () => {
      mockAxios.onPut(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .updateProperty(mockParcel)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'properties/storeProperty',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('deleteParcel action creator', () => {
    const url = '/properties/1';
    const mockResponse = { data: { success: true } };
    it('calls the api with the expected url', () => {
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .deleteProperty(mockParcel)
            .then(() => {
              expect(mockAxios.history.delete[0]).toMatchObject({
                url: '/properties/1',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
    it('Request successful, dispatches success with correct response', () => {
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .deleteProperty(mockParcel)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: null,
                type: 'properties/storeProperty',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request successful, dispatches error with correct response', () => {
      mockAxios.onDelete(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .deleteProperty(mockParcel)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: null,
                type: 'properties/storeProperty',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });
});
