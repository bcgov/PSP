import { renderHook } from '@testing-library/react-hooks';
import { AxiosResponse } from 'axios';
import React from 'react';
import { Provider } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { Action } from 'redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import ITenantConfig from '@/hooks/pims-api/interfaces/ITenantConfig';
import { useApiTenants } from '@/hooks/pims-api/useApiTenants';

import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { useTenants } from '.';

const mockApiGetSettings = jest.fn<Promise<AxiosResponse<ITenantConfig>>, any>();
jest.mock('@/hooks/pims-api/useApiTenants');
(useApiTenants as jest.Mock).mockReturnValue({ getSettings: mockApiGetSettings });

jest.mock('react-redux-loading-bar', () => {
  const original = jest.requireActual('react-redux-loading-bar');
  return {
    ...original,
    showLoading: jest.fn((scope?: string): Action<any> => ({ type: 'show' })),
    hideLoading: jest.fn((scope?: string): Action<any> => ({ type: 'hide' })),
  };
});

const mockHideLoading = showLoading as jest.Mock;
const mockShowLoading = hideLoading as jest.Mock;

jest.mock('../network/networkSlice', () => {
  const original = jest.requireActual('../network/networkSlice');
  return {
    ...original,
    logError: jest.fn(() => ({ type: 'error' })),
    logRequest: jest.fn(() => ({ type: 'request' })),
    logSuccess: jest.fn(() => ({ type: 'success' })),
  };
});

const mockLogRequest = logRequest as unknown as jest.Mock;
const mockLogSuccess = logSuccess as unknown as jest.Mock;
const mockLogError = logError as unknown as jest.Mock;

let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    <Provider store={store}>{children}</Provider>;

describe('useTenant slice hook', () => {
  beforeEach(() => {});

  afterEach(() => {
    mockShowLoading.mockClear();
    mockLogRequest.mockClear();
    mockLogSuccess.mockClear();
    mockLogError.mockClear();
    mockHideLoading.mockClear();
    mockApiGetSettings.mockClear();
  });

  afterAll(() => {
    jest.restoreAllMocks();
    jest.unmock('react-redux-loading-bar');
  });

  it('getSettings reducer + api hook', async () => {
    // mock API calls
    mockApiGetSettings.mockResolvedValue({ data: { code: 'test' } } as any);
    const wrapper = getWrapper(getStore({}));
    const { result } = renderHook(() => useTenants(), { wrapper });
    // get results from hook
    const response = await result.current.getSettings();
    // assertions
    expect(response?.data).toStrictEqual({ code: 'test' });
    expect(mockApiGetSettings).toBeCalledTimes(1);
    expect(mockShowLoading).toBeCalledTimes(1);
    expect(mockLogRequest).toBeCalledTimes(1);
    expect(mockLogSuccess).toBeCalledTimes(1);
    expect(mockLogError).toBeCalledTimes(0);
    expect(mockHideLoading).toBeCalledTimes(1);
  });

  it('getSettings reducer + api hook error', async () => {
    // mock API calls
    mockApiGetSettings.mockRejectedValue({ data: { code: 'test' } } as any);
    const wrapper = getWrapper(getStore({}));
    const { result } = renderHook(() => useTenants(), { wrapper });
    // get results from hook
    const response = await result.current.getSettings();
    // assertions
    expect(response?.data).toBeUndefined();
    expect(mockApiGetSettings).toBeCalledTimes(1);
    expect(mockShowLoading).toBeCalledTimes(1);
    expect(mockLogRequest).toBeCalledTimes(1);
    expect(mockLogSuccess).toBeCalledTimes(0);
    expect(mockLogError).toBeCalledTimes(1);
    expect(mockHideLoading).toBeCalledTimes(1);
  });
});
