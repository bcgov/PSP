import { useProperties } from './useProperties';
import MockAdapter from 'axios-mock-adapter';
import axios from 'axios';
import * as genericActions from 'actions/genericActions';
import * as API from 'constants/API';
import { IParcelDetailParams } from 'constants/API';
import * as MOCK from 'mocks/dataMocks';
import { ENVIRONMENT } from 'constants/environment';
import { renderHook } from '@testing-library/react-hooks';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { Provider } from 'react-redux';
import {
  mockBuilding,
  mockBuildingDetail,
  mockParcel,
  mockParcelDetail,
  mockProperty,
} from 'mocks/filterDataMock';

const dispatch = jest.fn();
const requestSpy = jest.spyOn(genericActions, 'request');
const successSpy = jest.spyOn(genericActions, 'success');
const errorSpy = jest.spyOn(genericActions, 'error');
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
  describe('fetchParcels action creator', () => {
    it('Null Params - Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.PROPERTIES(null);
      const mockResponse = { data: [mockProperty] };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchParcels(null)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
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
      const url = ENVIRONMENT.apiUrl + API.PROPERTIES(null);
      const mockResponse = { data: [mockProperty] };
      mockAxios.onGet(url).reply(400, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchParcels(null)
            .catch(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
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
    it('Request successful, dispatches success with correct response', () => {
      const params: IParcelDetailParams = { id: 1 };
      const url = ENVIRONMENT.apiUrl + API.PARCEL_DETAIL(params);
      const mockResponse = { data: mockParcelDetail };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchParcelDetail(params)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
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
      const params: IParcelDetailParams = { id: 1 };
      const url = ENVIRONMENT.apiUrl + API.PARCEL_DETAIL(params);
      const mockResponse = { data: mockBuildingDetail };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .fetchParcelDetail(params)
            .catch(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
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
    it('Request successful, dispatches success with correct response', () => {
      const params: API.IBuildingDetailParams = { id: 1 };
      const url = ENVIRONMENT.apiUrl + API.BUILDING_DETAIL(params);
      const mockResponse = { data: { mockBuildingDetail } };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchBuildingDetail(params)
            .catch(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
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
      const params: IParcelDetailParams = { id: 1 };
      const url = ENVIRONMENT.apiUrl + API.BUILDING_DETAIL(params);
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const mockResponse = { data: { mockBuildingDetail } };
      renderHook(
        () =>
          useProperties()
            .fetchBuildingDetail(params)
            .catch(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
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
    it('parcel Request successful, dispatches success with correct response', () => {
      const params: IParcelDetailParams = { id: 1 };
      const url = ENVIRONMENT.apiUrl + API.PARCEL_DETAIL(params);
      const mockResponse = { data: mockParcelDetail };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchParcelDetail(params)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
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
      const params: any = { id: 1, propertyTypeId: 1, position: [0, 0] };
      const url = ENVIRONMENT.apiUrl + API.BUILDING_DETAIL(params);
      const mockResponse = { data: mockBuildingDetail };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .fetchPropertyDetail(1, 1, undefined)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
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
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.PARCEL_ROOT;
      const mockResponse = { data: mockParcelDetail };
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .createParcel(mockParcel)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
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
      const url = ENVIRONMENT.apiUrl + API.PARCEL_ROOT;
      const mockResponse = { data: mockParcelDetail };
      mockAxios.onPost(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .createParcel(mockParcel)
            .catch(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
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
    it('Request successful, dispatches success with correct response', () => {
      const parcel = { id: 1 } as any;
      const url = ENVIRONMENT.apiUrl + API.PARCEL_ROOT + `/${parcel.id}`;
      const mockResponse = { data: mockParcelDetail };
      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .updateParcel(mockParcel)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
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
      const parcel = { id: 1 } as any;
      const url = ENVIRONMENT.apiUrl + API.PARCEL_ROOT + `/${parcel.id}`;
      const mockResponse = { data: mockParcelDetail };
      mockAxios.onPut(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .updateParcel(mockParcel)
            .catch(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
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
    it('Request successful, dispatches success with correct response', () => {
      const parcel = { id: 1 } as any;
      const url = ENVIRONMENT.apiUrl + API.PARCEL_ROOT + `/${parcel.id}`;
      const mockResponse = { data: { success: true } };
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .deleteParcel(mockParcel)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
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
      const parcel = { id: 1 } as any;
      const url = ENVIRONMENT.apiUrl + API.PARCEL_ROOT + `/${parcel.id}`;
      mockAxios.onDelete(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .deleteParcel(mockParcel)
            .catch(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
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
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.BUILDING_ROOT + `/${mockBuilding.id}`;
      const mockResponse = { data: { success: true } };
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useProperties()
            .deleteBuilding(mockBuilding)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
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
      const url = ENVIRONMENT.apiUrl + API.BUILDING_ROOT + `/${mockBuilding.id}`;
      mockAxios.onDelete(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useProperties()
            .deleteBuilding(mockBuilding)
            .catch(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
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
