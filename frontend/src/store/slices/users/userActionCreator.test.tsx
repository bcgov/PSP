import { waitFor } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { find } from 'lodash';
import * as MOCK from 'mocks/dataMocks';
import { mockUser } from 'mocks/filterDataMock';
import { Provider } from 'react-redux';
import { MockStoreEnhanced } from 'redux-mock-store';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { networkSlice } from '../network/networkSlice';
import { useUsers } from './useUsers';

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

describe('users action creator', () => {
  afterAll(() => {
    jest.restoreAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useUsers, { wrapper: getWrapper(getStore()) });
    return result.current;
  };
  describe('fetchUsers action creator', () => {
    const url = '/admin/users/my/organization';
    const mockResponse = {
      data: {
        items: [mockUser],
        page: 1,
        pageIndex: 0,
        quantity: 0,
        total: 0,
      },
      pageIndex: null,
    };
    it('calls the api with the expected url', async () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      const { fetchUsers } = setup();
      fetchUsers({} as any);
      await waitFor(async () => {
        expect(mockAxios.history.post[0]).toMatchObject({
          url: '/admin/users/my/organization',
        });
      });
    });
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      const { fetchUsers } = setup();
      fetchUsers({} as any);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logSuccess' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
        expect(currentStore.getActions()).toContainEqual({
          payload: mockResponse,
          type: 'users/storeUsers',
        });
      });
    });

    it('Request failure, dispatches error with correct response', async () => {
      const mockResponse = { data: [mockUser] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const { fetchUsers } = setup();
      fetchUsers({} as any);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
        expect(currentStore.getActions()).not.toContainEqual({
          payload: mockResponse,
          type: 'users/storeUsers',
        });
      });
    });
  });

  describe('fetchUserDetail action creator', () => {
    const url = '/admin/users/14c9a273-6f4a-4859-8d59-9264d3cee53f';
    const mockResponse = {
      data: [mockUser],
    };
    it('calls the api with the expected url', async () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      const { fetchUserDetail } = setup();
      fetchUserDetail('14c9a273-6f4a-4859-8d59-9264d3cee53f');
      await waitFor(async () => {
        expect(mockAxios.history.get[0]).toMatchObject({
          url: '/admin/users/14c9a273-6f4a-4859-8d59-9264d3cee53f',
        });
      });
    });
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      const { fetchUserDetail } = setup();
      fetchUserDetail('14c9a273-6f4a-4859-8d59-9264d3cee53f');
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logSuccess' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
        expect(currentStore.getActions()).toContainEqual({
          payload: mockResponse,
          type: 'users/storeUserDetails',
        });
      });
    });

    it('Request failure, dispatches error with correct response', async () => {
      const mockResponse = { data: [mockUser] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const { fetchUserDetail } = setup();
      fetchUserDetail('14c9a273-6f4a-4859-8d59-9264d3cee53f');
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
        expect(currentStore.getActions()).not.toContainEqual({
          payload: mockResponse,
          type: 'users/storeUserDetails',
        });
      });
    });
  });

  describe('updateUser action creator', () => {
    const url = '/keycloak/users/14c9a273-6f4a-4859-8d59-9264d3cee53f';
    const mockResponse = {
      data: [mockUser],
    };
    it('calls the api with the expected url', async () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      const { updateUser } = setup();
      updateUser(mockUser);
      await waitFor(async () => {
        expect(mockAxios.history.put[0]).toMatchObject({
          url: '/keycloak/users/14c9a273-6f4a-4859-8d59-9264d3cee53f',
        });
      });
    });
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onPut(url).reply(200, mockResponse);
      const { updateUser } = setup();
      updateUser(mockUser);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logSuccess' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
        expect(currentStore.getActions()).toContainEqual({
          payload: mockResponse,
          type: 'users/updateUser',
        });
      });
    });

    it('Request failure, dispatches error with correct response', async () => {
      const mockResponse = { data: [mockUser] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const { updateUser } = setup();
      updateUser(mockUser);
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
        expect(currentStore.getActions()).not.toContainEqual({
          payload: mockResponse,
          type: 'users/updateUser',
        });
      });
    });
  });

  describe('activate action creator', () => {
    const url = '/auth/activate';
    const mockResponse = {
      data: mockUser,
    };
    it('calls the api with the expected url', async () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      const { activateUser } = setup();
      activateUser();
      await waitFor(async () => {
        expect(mockAxios.history.post[0]).toMatchObject({
          url: '/auth/activate',
        });
      });
    });
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      const { activateUser } = setup();
      activateUser();
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logSuccess' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeDefined();
      });
    });

    it('Request failure, dispatches error with correct response', async () => {
      mockAxios.onPost(url).reply(400, MOCK.ERROR);
      const { activateUser } = setup();
      activateUser();
      await waitFor(async () => {
        expect(find(currentStore.getActions(), { type: 'network/logRequest' })).toBeDefined();
        expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      });
    });
  });
});
