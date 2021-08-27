import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { GET_ORGANIZATIONS } from 'constants/actionTypes';
import * as API from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import { IOrganization } from 'interfaces';
import { find } from 'lodash';
import * as MOCK from 'mocks/dataMocks';
import { mockOrganization } from 'mocks/filterDataMock';
import { Provider } from 'react-redux';
import { MockStoreEnhanced } from 'redux-mock-store';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { networkSlice } from '../network/networkSlice';
import { useOrganizations } from './useOrganizations';

const dispatch = jest.fn();
const requestSpy = jest.spyOn(networkSlice.actions, 'logRequest');
const successSpy = jest.spyOn(networkSlice.actions, 'logSuccess');
const errorSpy = jest.spyOn(networkSlice.actions, 'logError');
const mockAxios = new MockAdapter(axios);

afterEach(() => {
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

describe('organizations async actions', () => {
  describe('fetchOrganizations action creator', () => {
    const url = `/admin/organizations/filter`;
    const mockResponse = {
      data: {
        items: [mockOrganization],
        page: 1,
        pageIndex: 0,
        quantity: 0,
        total: 0,
      },
      pageIndex: null,
    };
    it('calls the api with the expected url', () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useOrganizations()
            .fetchOrganizations({} as any)
            .then(() => {
              expect(mockAxios.history.post[0]).toMatchObject({
                url: '/admin/organizations/filter',
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
          useOrganizations()
            .fetchOrganizations({} as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'organizations/storeOrganizations',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.POST_ORGANIZATIONS();
      const mockResponse = { data: [mockOrganization] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useOrganizations()
            .fetchOrganizations({} as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: GET_ORGANIZATIONS,
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('getOrganizationDetail action creator', () => {
    const url = `/admin/organizations/${mockOrganization.id}`;
    const mockResponse = {
      data: [mockOrganization],
    };
    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useOrganizations()
            .fetchOrganizationDetail(mockOrganization.id ?? 0)
            .then(() => {
              expect(mockAxios.history.get[0]).toMatchObject({ url: '/admin/organizations/1' });
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
          useOrganizations()
            .fetchOrganizationDetail(mockOrganization.id ?? 0)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'organizations/storeOrganizationDetails',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const mockResponse = { data: [mockOrganization] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useOrganizations()
            .fetchOrganizationDetail(mockOrganization.id ?? 0)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'organizations/storeOrganizationDetails',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('addOrganization action creator', () => {
    const url = `/admin/organizations`;
    const mockResponse = {
      data: [mockOrganization],
    };
    const mockOrganization1 = { ...mockOrganization, email: '', sendEmail: true, addressTo: '' };
    it('calls the api with the expected url', () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useOrganizations()
            .addOrganization(mockOrganization)
            .then(() => {
              expect(mockAxios.history.post[0]).toMatchObject({ url: '/admin/organizations' });
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
          useOrganizations()
            .addOrganization(mockOrganization1)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
          useOrganizations()
            .addOrganization(mockOrganization)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('updateOrganization action creator', () => {
    const url = `/admin/organizations/${mockOrganization.id}`;
    const mockResponse = {
      data: [mockOrganization],
    };
    const organizationDetail: IOrganization = {
      ...mockOrganization,
      rowVersion: 1,
    };
    it('calls the api with the expected url', () => {
      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useOrganizations()
            .updateOrganization(organizationDetail)
            .then(() => {
              expect(mockAxios.history.put[0]).toMatchObject({ url: '/admin/organizations/1' });
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
          useOrganizations()
            .updateOrganization(organizationDetail)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
          useOrganizations()
            .updateOrganization(organizationDetail)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('deleteOrganization action creator', () => {
    const url = `/admin/organizations/${mockOrganization.id}`;
    const mockResponse = {
      data: [mockOrganization],
    };
    it('calls the api with the expected url', () => {
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useOrganizations()
            .removeOrganization(mockOrganization)
            .then(() => {
              expect(mockAxios.history.delete[0]).toMatchObject({ url: '/admin/organizations/1' });
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
          useOrganizations()
            .removeOrganization(mockOrganization)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
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
          useOrganizations()
            .removeOrganization(mockOrganization)
            .catch(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });
});
