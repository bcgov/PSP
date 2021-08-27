import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { find } from 'lodash';
import * as MOCK from 'mocks/dataMocks';
import { mockAccessRequest } from 'mocks/filterDataMock';
import React from 'react';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useAccessRequests } from './useAccessRequests';

const mockAxios = new MockAdapter(axios);

afterEach(() => {
  mockAxios.reset();
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
    const url = `/access/requests`;
    const mockResponse = {
      data: mockAccessRequest,
    };
    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .fetchCurrentAccessRequest()
            .then(() => {
              expect(mockAxios.history.get[0]).toMatchObject({ url: url });
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
          useAccessRequests()
            .fetchCurrentAccessRequest()
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
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
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .fetchCurrentAccessRequest()
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
    const newMockAccessRequest = { ...mockAccessRequest, id: 0 };
    const url = `/access/requests`;
    const mockResponse = { data: mockAccessRequest };

    it('calls the api with the expected url', () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .addAccessRequest(newMockAccessRequest)
            .then(() => {
              expect(mockAxios.history.post[0]).toMatchObject({ url: url });
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
          useAccessRequests()
            .addAccessRequest(newMockAccessRequest)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
      mockAxios.onPost(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .addAccessRequest(newMockAccessRequest)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
    const url = `/keycloak/access/requests`;
    const mockResponse = { data: mockAccessRequest };
    it('calls the api with the expected url', () => {
      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .updateAccessRequest(mockAccessRequest)
            .then(() => {
              expect(mockAxios.history.put[0]).toMatchObject({ url: url });
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
          useAccessRequests()
            .updateAccessRequest(mockAccessRequest)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(
                find(currentStore.getActions(), { type: 'network/logSuccess' }),
              ).not.toBeNull();
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
      mockAxios.onPut(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .updateAccessRequest(mockAccessRequest)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
    const url = '/admin/access/requests?';
    const mockResponse = {
      data: {
        items: [mockAccessRequest],
        page: 1,
        pageIndex: 0,
        quantity: 0,
        total: 0,
      },
    };
    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .fetchAccessRequests({} as any)
            .then(() => {
              expect(mockAxios.history.get[0]).toMatchObject({ url: url });
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
          useAccessRequests()
            .fetchAccessRequests({} as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .fetchAccessRequests({} as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
    const url = `/admin/access/requests/${mockAccessRequest.id}`;
    const mockResponse = { data: mockAccessRequest };
    it('calls the api with the expected url', () => {
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAccessRequests()
            .removeAccessRequest(mockAccessRequest.id ?? 0, mockAccessRequest)
            .then(() => {
              expect(mockAxios.history.delete[0]).toMatchObject({ url: url });
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
          useAccessRequests()
            .removeAccessRequest(mockAccessRequest.id ?? 0, mockAccessRequest)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
      mockAxios.onDelete(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAccessRequests()
            .removeAccessRequest(mockAccessRequest.id ?? 0, mockAccessRequest)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
