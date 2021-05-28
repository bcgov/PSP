import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { find } from 'lodash';
import * as MOCK from 'mocks/dataMocks';
import {
  mockBuilding,
  mockBuildingDetail,
  mockParcel,
  mockParcelDetail,
  mockProperty,
} from 'mocks/filterDataMock';
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
    const url = `/properties/search?`;
    it('Null Params - Request successful, dispatches success with correct response', () => {
      const mockResponse = { data: [mockProperty] };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchParcels(null)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
      const mockResponse = { data: [mockProperty] };
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
    const url = `/properties/parcels/${mockParcel.id}`;
    it('Request successful, dispatches success with correct response', () => {
      const mockResponse = { data: mockParcelDetail };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchParcelDetail(1)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
      const mockResponse = { data: mockBuildingDetail };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .fetchParcelDetail(1)
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
    const url = `/properties/buildings/${mockBuilding.id}`;
    it('Request successful, dispatches success with correct response', () => {
      const mockResponse = { data: { mockBuildingDetail } };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchBuildingDetail(mockBuilding.id as number)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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

    it('Request failure, dispatches error with correct response', () => {
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const mockResponse = { data: { mockBuildingDetail } };
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
                type: 'properties/storeBuildingDetail',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('fetchPropertyDetail action creator', () => {
    it('parcel Request successful, dispatches success with correct response', () => {
      const url = `/properties/parcels/${mockParcel.id}`;
      const mockResponse = { data: mockParcel };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchPropertyDetail(1, 0, undefined)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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

    it('building Request successful, dispatches success with correct response', () => {
      const url = `/properties/buildings/${mockBuilding.id}`;
      const mockResponse = { data: mockBuilding };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchPropertyDetail(mockBuilding.id as number, 1, undefined)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
    const url = `/properties/parcels`;
    it('Request successful, dispatches success with correct response', () => {
      const mockResponse = { data: mockParcelDetail };
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .createParcel(mockParcel)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
      const mockResponse = { data: mockParcelDetail };
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
    const url = `/properties/parcels/${mockParcel.id}`;
    it('Request successful, dispatches success with correct response', () => {
      const mockResponse = { data: mockParcel };
      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .updateParcel(mockParcel)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
      const mockResponse = { data: mockParcel };
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
    const url = `/properties/parcels/${mockParcel.id}`;
    it('Request successful, dispatches success with correct response', () => {
      const mockResponse = { data: { success: true } };
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .removeParcel(mockParcel)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
    const url = `/properties/buildings/${mockBuilding.id}`;
    it('Request successful, dispatches success with correct response', () => {
      const mockResponse = { data: { success: true } };
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .removeBuilding(mockBuilding)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
