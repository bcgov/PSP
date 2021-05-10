import MockAdapter from 'axios-mock-adapter';
import axios from 'axios';
import * as genericActions from 'actions/genericActions';
import * as API from 'constants/API';
import * as MOCK from 'mocks/dataMocks';
import { ENVIRONMENT } from 'constants/environment';
import { useAccessRequests } from './useAccessRequests';
import { renderHook } from '@testing-library/react-hooks';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { Provider } from 'react-redux';
import React from 'react';
import { mockAccessRequest } from 'mocks/filterDataMock';

const requestSpy = jest.spyOn(genericActions, 'request');
const successSpy = jest.spyOn(genericActions, 'success');
const errorSpy = jest.spyOn(genericActions, 'error');
const mockAxios = new MockAdapter(axios);

afterEach(() => {
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

describe('useAccessRequests functionality', () => {
  describe('fetchCurrentAccessRequest', () => {
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS();
      const mockResponse = {
        data: mockAccessRequest,
      };

      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .fetchCurrentAccessRequest()
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'accessRequests/storeAccessRequest',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS();

      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .fetchCurrentAccessRequest()
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).not.toContainEqual({
                payload: {
                  data: {
                    items: [mockAccessRequest],
                    page: 1,
                    pageIndex: 0,
                    quantity: 0,
                    total: 0,
                  },
                },
                type: 'accessRequests/storeAccessRequest',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('addAccessRequest action creator', () => {
    it('Request successful, dispatches success with correct response', () => {
      const newMockAccessRequest = { ...mockAccessRequest, id: 0 };
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS();
      const mockResponse = { data: mockAccessRequest };

      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .addAccessRequest(newMockAccessRequest)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).toContainEqual({
                payload: {
                  data: mockAccessRequest,
                },
                type: 'accessRequests/storeAccessRequest',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const newMockAccessRequest = { ...mockAccessRequest, id: 0 };
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS();
      mockAxios.onPost(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .addAccessRequest(newMockAccessRequest)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).not.toContainEqual({
                accessRequest: {
                  data: mockAccessRequest,
                },
                type: 'accessRequests/storeAccessRequest',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('updateAccessRequest action creator', () => {
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS_ADMIN();
      const mockResponse = { data: mockAccessRequest };

      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .updateAccessRequest(mockAccessRequest)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).toContainEqual({
                payload: {
                  data: mockAccessRequest,
                },
                type: 'accessRequests/updateAccessRequestsAdmin',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS_ADMIN();
      mockAxios.onPut(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .updateAccessRequest(mockAccessRequest)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).not.toContainEqual({
                accessRequest: {
                  data: mockAccessRequest,
                },
                type: 'accessRequests/updateAccessRequestsAdmin',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('fetchAccessRequests action creator', () => {
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS_LIST({} as any);
      const mockResponse = {
        data: {
          items: [mockAccessRequest],
          page: 1,
          pageIndex: 0,
          quantity: 0,
          total: 0,
        },
      };

      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .fetchAccessRequests({} as any)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).toContainEqual({
                payload: {
                  data: {
                    items: [mockAccessRequest],
                    page: 1,
                    pageIndex: 0,
                    quantity: 0,
                    total: 0,
                  },
                },
                type: 'accessRequests/storeAccessRequests',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS_LIST({} as any);
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .fetchAccessRequests({} as any)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).not.toContainEqual({
                accessRequest: {
                  data: mockAccessRequest,
                },
                type: 'accessRequests/storeAccessRequests',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('removeAccessRequest action creator', () => {
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS_DELETE(mockAccessRequest.id);
      const mockResponse = { data: mockAccessRequest };

      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .removeAccessRequest(mockAccessRequest.id, mockAccessRequest)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(successSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).toContainEqual({
                payload: 2,
                type: 'accessRequests/deleteAccessRequest',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.REQUEST_ACCESS_DELETE(mockAccessRequest.id);
      mockAxios.onDelete(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .removeAccessRequest(mockAccessRequest.id, mockAccessRequest)
            .then(() => {
              expect(requestSpy).toHaveBeenCalledTimes(1);
              expect(errorSpy).toHaveBeenCalledTimes(1);
              expect(currentStore.getActions()).not.toContainEqual({
                payload: 2,
                type: 'accessRequests/deleteAccessRequest',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });
});
