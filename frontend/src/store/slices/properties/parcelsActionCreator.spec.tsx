import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { PropertyTypes } from 'constants/propertyTypes';
import { find } from 'lodash';
import * as MOCK from 'mocks/dataMocks';
import { mockBuilding, mockParcel, mockParcelDetail } from 'mocks/filterDataMock';
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
  describe('fetchParcels action creator', () => {
    const url = '/properties/search?';
    const mockResponse = { data: [mockParcel] };
    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchParcels(null)
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
            .fetchParcels(null)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'properties/storeParcels',
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
            .fetchParcels(null)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'properties/storeParcels',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });
  describe('fetchParcelDetail action creator', () => {
    const url = '/properties/parcels/1';
    const mockResponse = { data: mockParcel };
    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchParcelDetail(mockParcel.id as number)
            .then(() => {
              expect(mockAxios.history.get[0]).toMatchObject({
                url: '/properties/parcels/1',
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
            .fetchParcelDetail(mockParcel.id as number)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: { position: undefined, property: mockResponse },
                type: 'properties/storeParcelDetail',
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
            .fetchParcelDetail(mockParcel.id as number)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: { position: undefined, property: mockResponse },
                type: 'properties/storeParcelDetail',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('fetchBuildingDetail action creator', () => {
    const url = '/properties/buildings/100';
    const mockResponse = { data: { mockBuilding } };

    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchBuildingDetail(mockBuilding.id as number)
            .then(() => {
              expect(mockAxios.history.get[0]).toMatchObject({
                url: '/properties/buildings/100',
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
            .fetchBuildingDetail(mockBuilding.id as number)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: { position: undefined, property: mockResponse },
                type: 'STORE_BUILDING_DETAILS',
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
            .fetchBuildingDetail(mockBuilding.id as number)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: { position: undefined, property: mockResponse },
                type: 'STORE_BUILDING_DETAILS',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('fetchPropertyDetail action creator', () => {
    const url = '/properties/buildings/1';
    const mockResponse = { data: mockParcelDetail };
    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchPropertyDetail(1, PropertyTypes.Building, undefined)
            .then(() => {
              expect(mockAxios.history.get[0]).toMatchObject({
                url: '/properties/buildings/1',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
    it('parcel Request successful, dispatches success with correct response', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchPropertyDetail(1, PropertyTypes.Building, undefined)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: { position: undefined, property: mockResponse },
                type: 'properties/storeBuildingDetail',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('building Request successful, dispatches success with correct response', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchPropertyDetail(1, PropertyTypes.Building, undefined)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: { position: undefined, property: mockResponse },
                type: 'properties/storeBuildingDetail',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('createParcel action creator', () => {
    const url = '/properties/parcels';
    const mockResponse = { data: mockParcelDetail };
    it('calls the api with the expected url', () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .createParcel(mockParcel)
            .then(() => {
              expect(mockAxios.history.post[0]).toMatchObject({
                url: '/properties/parcels',
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
            .createParcel(mockParcel)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'properties/storeParcelDetail',
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
            .createParcel(mockParcel)
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

  describe('updateParcel action creator', () => {
    const url = '/properties/parcels/1';
    const mockResponse = { data: mockParcelDetail };
    it('calls the api with the expected url', () => {
      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .updateParcel(mockParcel)
            .then(() => {
              expect(mockAxios.history.put[0]).toMatchObject({
                url: '/properties/parcels/1',
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
            .updateParcel(mockParcel)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'properties/storeParcelDetail',
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
            .updateParcel(mockParcel)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'properties/storeParcelDetail',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('deleteParcel action creator', () => {
    const url = '/properties/parcels/1';
    const mockResponse = { data: { success: true } };
    it('calls the api with the expected url', () => {
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .removeParcel(mockParcel)
            .then(() => {
              expect(mockAxios.history.delete[0]).toMatchObject({
                url: '/properties/parcels/1',
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
            .removeParcel(mockParcel)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: null,
                type: 'properties/storeParcelDetail',
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
            .removeParcel(mockParcel)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: null,
                type: 'properties/storeParcelDetail',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });
  describe('deleteBuilding action creator', () => {
    const url = '/properties/buildings/100';
    const mockResponse = { data: { success: true } };
    it('calls the api with the expected url', () => {
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .removeBuilding(mockBuilding)
            .then(() => {
              expect(mockAxios.history.delete[0]).toMatchObject({
                url: '/properties/buildings/100',
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
            .removeBuilding(mockBuilding)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: null,
                type: 'properties/storeParcelDetail',
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
            .removeBuilding(mockBuilding)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: null,
                type: 'properties/storeParcelDetail',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });
});
