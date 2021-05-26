import { renderHook } from '@testing-library/react-hooks';
import { useTenants } from '.';
import { useApiTenants, ITenantConfig } from 'hooks/pims-api';
import { AxiosResponse } from 'axios';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import React from 'react';
import { Provider } from 'react-redux';
import { networkSlice } from '../network/networkSlice';
import { Action } from 'redux';
import * as loadingBar from 'react-redux-loading-bar';

jest.mock('react-redux-loading-bar', () => {
  const original = jest.requireActual('react-redux-loading-bar');
  return {
    ...original,
    showLoading: jest.fn((scope?: string): Action<any> => ({ type: 'show' })),
    hideLoading: jest.fn((scope?: string): Action<any> => ({ type: 'hide' })),
  };
});
const showSpy = jest.spyOn(loadingBar, 'showLoading');
const hideSpy = jest.spyOn(loadingBar, 'hideLoading');

const requestSpy = jest.spyOn(networkSlice.actions, 'logRequest');
const successSpy = jest.spyOn(networkSlice.actions, 'logSuccess');
const errorSpy = jest.spyOn(networkSlice.actions, 'logError');

let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper = (store: any) => ({ children }: any) => (
  <Provider store={store}>{children}</Provider>
);

describe('useTenant slice hook', () => {
  beforeEach(() => {});

  afterEach(() => {
    showSpy.mockClear();
    requestSpy.mockClear();
    successSpy.mockClear();
    errorSpy.mockClear();
    hideSpy.mockClear();
  });

  afterAll(() => {
    jest.restoreAllMocks();
  });

  it('getSettings reducer + api hook', () => {
    renderHook(
      async () => {
        const getSettingsSpy = jest.spyOn(useApiTenants(), 'getSettings');
        getSettingsSpy.mockImplementation(() =>
          Promise.resolve(({ code: 'test' } as unknown) as AxiosResponse<ITenantConfig>),
        );
        const { getSettings } = useTenants();

        const response = await getSettings();
        expect(response?.data).toStrictEqual({ code: 'test' });
        expect(getSettingsSpy).toBeCalledTimes(1);
        expect(showSpy).toBeCalledTimes(1);
        expect(requestSpy).toBeCalledTimes(1);
        expect(successSpy).toBeCalledTimes(1);
        expect(errorSpy).toBeCalledTimes(0);
        expect(hideSpy).toBeCalledTimes(1);
      },
      {
        wrapper: getWrapper(getStore()),
      },
    );
  });

  it('getSettings reducer + api hook error', () => {
    renderHook(
      async () => {
        const getSettingsSpy = jest.spyOn(useApiTenants(), 'getSettings');
        getSettingsSpy.mockImplementation(() =>
          Promise.reject(({ code: 'test' } as unknown) as AxiosResponse<ITenantConfig>),
        );
        const { getSettings } = useTenants();

        const response = await getSettings();
        expect(response?.data).toStrictEqual({ code: 'test' });
        expect(getSettingsSpy).toBeCalledTimes(1);
        expect(showSpy).toBeCalledTimes(1);
        expect(requestSpy).toBeCalledTimes(1);
        expect(successSpy).toBeCalledTimes(0);
        expect(errorSpy).toBeCalledTimes(1);
        expect(hideSpy).toBeCalledTimes(1);
      },
      {
        wrapper: getWrapper(getStore()),
      },
    );
  });
});
