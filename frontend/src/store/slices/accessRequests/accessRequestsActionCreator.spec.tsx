import { waitFor } from '@testing-library/react';
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

const setup = () => {
  const { result } = renderHook(useAccessRequests, { wrapper: getWrapper(getStore()) });
  return result.current;
};

describe('useAccessRequests functionality', () => {
  describe('updateAccessRequest action creator', () => {
    const url = `/keycloak/access/requests`;
    const mockResponse = { data: mockAccessRequest };
    it('calls the api with the expected url', async () => {
      mockAxios.onPut(url).reply(200, mockResponse);
      const { updateAccessRequest } = setup();
      updateAccessRequest(mockAccessRequest);
      await waitFor(async () => {
        expect(mockAxios.history.put[0]).toMatchObject({ url: url });
      });
    });
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onPut(url).reply(200, mockResponse);
      const { updateAccessRequest } = setup();
      updateAccessRequest(mockAccessRequest);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logSuccess' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
        expect(currentStore.getActions()).toContainEqual({
          payload: {
            data: mockAccessRequest,
          },
          type: 'accessRequests/updateAccessRequestsAdmin',
        });
      });
    });

    it('Request failure, dispatches error with correct response', async () => {
      mockAxios.onPut(url).reply(400, MOCK.ERROR);
      const { updateAccessRequest } = setup();
      updateAccessRequest(mockAccessRequest);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
        expect(currentStore.getActions()).not.toContainEqual({
          accessRequest: {
            data: mockAccessRequest,
          },
          type: 'accessRequests/updateAccessRequestsAdmin',
        });
      });
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
    it('calls the api with the expected url', async () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      const { fetchAccessRequests } = setup();
      fetchAccessRequests({} as any);
      await waitFor(async () => {
        expect(mockAxios.history.get[0]).toMatchObject({ url: url });
      });
    });
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      const { fetchAccessRequests } = setup();
      fetchAccessRequests({} as any);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logSuccess' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
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
      });
    });

    it('Request failure, dispatches error with correct response', async () => {
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const { fetchAccessRequests } = setup();
      fetchAccessRequests({} as any);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
        expect(currentStore.getActions()).not.toContainEqual({
          accessRequest: {
            data: mockAccessRequest,
          },
          type: 'accessRequests/storeAccessRequests',
        });
      });
    });
  });

  describe('removeAccessRequest action creator', () => {
    const url = `/admin/access/requests/${mockAccessRequest.id}`;
    const mockResponse = { data: mockAccessRequest };
    it('calls the api with the expected url', async () => {
      mockAxios.onDelete(url).reply(200, mockResponse);
      const { removeAccessRequest } = setup();
      removeAccessRequest(mockAccessRequest.id ?? 0, mockAccessRequest);
      await waitFor(async () => {
        expect(mockAxios.history.delete[0]).toMatchObject({ url: url });
      });
    });
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onDelete(url).reply(200, mockResponse);
      const { removeAccessRequest } = setup();
      removeAccessRequest(mockAccessRequest.id ?? 0, mockAccessRequest);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logSuccess' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
        expect(currentStore.getActions()).toContainEqual({
          payload: 2,
          type: 'accessRequests/deleteAccessRequest',
        });
      });
    });

    it('Request failure, dispatches error with correct response', async () => {
      mockAxios.onDelete(url).reply(400, MOCK.ERROR);
      const { removeAccessRequest } = setup();
      removeAccessRequest(mockAccessRequest.id ?? 0, mockAccessRequest);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
        expect(currentStore.getActions()).not.toContainEqual({
          payload: 2,
          type: 'accessRequests/deleteAccessRequest',
        });
      });
    });
  });
});
