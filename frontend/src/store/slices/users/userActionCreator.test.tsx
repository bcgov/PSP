import MockAdapter from 'axios-mock-adapter';
import axios from 'axios';
import * as API from 'constants/API';
import * as MOCK from 'mocks/dataMocks';
import { ENVIRONMENT } from 'constants/environment';
import { Provider } from 'react-redux';
import { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import configureMockStore from 'redux-mock-store';
import { renderHook } from '@testing-library/react-hooks';
import { useUsers } from './useUsers';
import { AGENCIES } from 'mocks/filterDataMock';
import { IUserDetails } from 'interfaces';
import { networkSlice } from '../network/networkSlice';
import { find } from 'lodash';

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
const mockUser: IUserDetails = {
  id: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  displayName: 'User, Admin',
  firstName: 'Admin',
  lastName: 'User',
  email: 'admin@pims.gov.bc.ca',
  username: 'admin',
  position: '',
  createdOn: '2021-05-04T19:07:09.6920606',
  agencies: [],
  roles: [],
};

describe('users action creator', () => {
  describe('fetchUsers action creator', () => {
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.POST_USERS();
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
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useUsers()
            .fetchUsers({} as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'users/storeUsers',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.POST_USERS();
      const mockResponse = { data: [mockUser] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useUsers()
            .fetchUsers({} as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'users/storeUsers',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('fetchUserDetail action creator', () => {
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.USER_DETAIL({ id: mockUser.id } as any);
      const mockResponse = {
        data: [mockUser],
      };
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useUsers()
            .fetchUserDetail({ id: mockUser.id } as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'users/storeUserDetails',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.USER_DETAIL({ id: mockUser.id } as any);
      const mockResponse = { data: [AGENCIES] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useUsers()
            .fetchUserDetail({ id: mockUser.id } as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'users/storeUserDetails',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('updateUser action creator', () => {
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.KEYCLOAK_USER_UPDATE({ id: mockUser.id } as any);
      const mockResponse = {
        data: [mockUser],
      };
      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useUsers()
            .updateUser({ id: mockUser.id } as any, mockUser)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'users/updateUser',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.KEYCLOAK_USER_UPDATE({ id: mockUser.id } as any);
      const mockResponse = { data: [mockUser] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useUsers()
            .updateUser({ id: mockUser.id } as any, mockUser)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'users/updateUser',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('activate action creator', () => {
    it('Request successful, dispatches success with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.ACTIVATE_USER();
      const mockResponse = {
        data: mockUser,
      };
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useUsers()
            .activateUser()
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
      const url = ENVIRONMENT.apiUrl + API.ACTIVATE_USER();
      mockAxios.onPost(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useUsers()
            .activateUser()
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
  });
});
