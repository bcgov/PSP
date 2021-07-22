import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { GET_AGENCIES } from 'constants/actionTypes';
import * as API from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import { IAgencyDetail } from 'interfaces';
import { find } from 'lodash';
import * as MOCK from 'mocks/dataMocks';
import { AGENCIES } from 'mocks/filterDataMock';
import { Provider } from 'react-redux';
import { MockStoreEnhanced } from 'redux-mock-store';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { networkSlice } from '../network/networkSlice';
import { useAgencies } from './useAgencies';

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

describe('agencies async actions', () => {
  describe('fetchAgencies action creator', () => {
    const url = `/admin/agencies/filter`;
    const mockResponse = {
      data: {
        items: AGENCIES,
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
          useAgencies()
            .fetchAgencies({} as any)
            .then(() => {
              expect(mockAxios.history.post[0]).toMatchObject({
                url: '/admin/agencies/filter',
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
          useAgencies()
            .fetchAgencies({} as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'agencies/storeAgencies',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const url = ENVIRONMENT.apiUrl + API.POST_AGENCIES();
      const mockResponse = { data: [AGENCIES] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAgencies()
            .fetchAgencies({} as any)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: GET_AGENCIES,
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('getAgencyDetail action creator', () => {
    const url = `/admin/agencies/${AGENCIES[0].id}`;
    const mockResponse = {
      data: [AGENCIES[0]],
    };
    it('calls the api with the expected url', () => {
      mockAxios.onGet(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAgencies()
            .fetchAgencyDetail(+AGENCIES[0].id)
            .then(() => {
              expect(mockAxios.history.get[0]).toMatchObject({ url: '/admin/agencies/1' });
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
          useAgencies()
            .fetchAgencyDetail(+AGENCIES[0].id)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).toContainEqual({
                payload: mockResponse,
                type: 'agencies/storeAgencyDetails',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });

    it('Request failure, dispatches error with correct response', () => {
      const mockResponse = { data: [AGENCIES] };
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      renderHook(
        () =>
          useAgencies()
            .fetchAgencyDetail(+AGENCIES[0].id)
            .then(() => {
              expect(
                find(currentStore.getActions(), { type: 'network/logRequest' }),
              ).not.toBeNull();
              expect(find(currentStore.getActions(), { type: 'network/logError' })).not.toBeNull();
              expect(currentStore.getActions()).not.toContainEqual({
                payload: mockResponse,
                type: 'agencies/storeAgencyDetails',
              });
            }),
        {
          wrapper: getWrapper(getStore()),
        },
      );
    });
  });

  describe('addAgency action creator', () => {
    const url = `/admin/agencies`;
    const mockResponse = {
      data: [AGENCIES[0]],
    };
    const mockAgency = { ...AGENCIES[0], email: '', sendEmail: true, addressTo: '' };
    it('calls the api with the expected url', () => {
      mockAxios.onPost(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAgencies()
            .addAgency(mockAgency)
            .then(() => {
              expect(mockAxios.history.post[0]).toMatchObject({ url: '/admin/agencies' });
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
          useAgencies()
            .addAgency(mockAgency)
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
          useAgencies()
            .addAgency(mockAgency)
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

  describe('updateAgency action creator', () => {
    const url = `/admin/agencies/${AGENCIES[0].id}`;
    const mockResponse = {
      data: [AGENCIES[0]],
    };
    const agencyDetail: IAgencyDetail = {
      ...AGENCIES[0],
      id: +AGENCIES[0].id,
      email: 'mail',
      sendEmail: true,
      addressTo: '',
      rowVersion: 1,
    };
    it('calls the api with the expected url', () => {
      mockAxios.onPut(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAgencies()
            .updateAgency(agencyDetail)
            .then(() => {
              expect(mockAxios.history.put[0]).toMatchObject({ url: '/admin/agencies/1' });
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
          useAgencies()
            .updateAgency(agencyDetail)
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
          useAgencies()
            .updateAgency(agencyDetail)
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

  describe('deleteAgency action creator', () => {
    const url = `/admin/agencies/${AGENCIES[0].id}`;
    const mockResponse = {
      data: [AGENCIES[0]],
    };
    it('calls the api with the expected url', () => {
      mockAxios.onDelete(url).reply(200, mockResponse);
      renderHook(
        () =>
          useAgencies()
            .removeAgency(AGENCIES[0])
            .then(() => {
              expect(mockAxios.history.delete[0]).toMatchObject({ url: '/admin/agencies/1' });
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
          useAgencies()
            .removeAgency(AGENCIES[0])
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
          useAgencies()
            .removeAgency(AGENCIES[0])
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
