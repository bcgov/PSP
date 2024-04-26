import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { Action } from 'redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useApiTenants } from '@/hooks/pims-api/useApiTenants';

import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { useTenants } from '.';
import { vi } from 'vitest';

const mockApiGetSettings = vi.fn();
vi.mock('@/hooks/pims-api/useApiTenants');
vi.mocked(useApiTenants).mockReturnValue({ getSettings: mockApiGetSettings });

vi.mock('react-redux-loading-bar', async importOriginal => {
  const actual = (await importOriginal()) as any;
  return {
    ...actual,
    showLoading: vi.fn((scope?: string): Action<any> => ({ type: 'show' })),
    hideLoading: vi.fn((scope?: string): Action<any> => ({ type: 'hide' })),
  };
});

vi.mock('../network/networkSlice', async importOriginal => {
  const actual = (await importOriginal()) as any;
  return {
    ...actual,
    logError: vi.fn(() => ({ type: 'error' })),
    logRequest: vi.fn(() => ({ type: 'request' })),
    logSuccess: vi.fn(() => ({ type: 'success' })),
  };
});

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
    vi.clearAllMocks();
  });

  afterAll(() => {
    vi.restoreAllMocks();
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
    expect(showLoading).toBeCalledTimes(1);
    expect(logRequest).toBeCalledTimes(1);
    expect(logSuccess).toBeCalledTimes(1);
    expect(logError).toBeCalledTimes(0);
    expect(hideLoading).toBeCalledTimes(1);
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
    expect(showLoading).toBeCalledTimes(1);
    expect(logRequest).toBeCalledTimes(1);
    expect(logSuccess).toBeCalledTimes(0);
    expect(logError).toBeCalledTimes(1);
    expect(hideLoading).toBeCalledTimes(1);
  });
});
