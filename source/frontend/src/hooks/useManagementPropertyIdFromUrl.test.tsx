import { renderHook, act } from '@testing-library/react-hooks';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Route } from 'react-router-dom';
import TestRouterWrapper from '@/utils/TestRouterWrapper';
import { useManagementPropertyIdFromUrl } from './useManagementPropertyIdFromUrl';

// Mock the repository hook
const mockGetManagementFileProperties = vi.fn();
vi.mock('@/hooks/repositories/useManagementFileRepository', () => ({
  useManagementFileRepository: () => ({
    getManagementProperties: {
      execute: mockGetManagementFileProperties,
      loading: false,
    },
  }),
}));

describe('useManagementPropertyIdFromUrl', () => {
  const history = createMemoryHistory();
  const wrapper =
    () =>
    ({ children }: { children: React.ReactNode }) =>
      (
        <TestRouterWrapper history={history}>
          <Route path="*">{children}</Route>
        </TestRouterWrapper>
      );

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('returns propertyId when URL matches and property is found', async () => {
    history.location.pathname = '/mapview/sidebar/management/99/property/123/';
    mockGetManagementFileProperties.mockResolvedValue([
      { id: 123, property: { id: 456 } },
      { id: 124, property: { id: 789 } },
    ]);
    const { result, waitForNextUpdate } = renderHook(() => useManagementPropertyIdFromUrl(), {
      wrapper: wrapper(),
    });

    // Wait for useEffect to finish
    await act(async () => {
      await waitForNextUpdate();
    });

    expect(result.current.propertyId).toBe(456);
    expect(result.current.loading).toBe(false);
    expect(mockGetManagementFileProperties).toHaveBeenCalledWith(99);
  });

  it('returns undefined if property is not found', async () => {
    history.location.pathname = '/mapview/sidebar/management/99/property/999/';
    mockGetManagementFileProperties.mockResolvedValue([{ id: 123, property: { id: 456 } }]);
    const { result } = renderHook(() => useManagementPropertyIdFromUrl(), {
      wrapper: wrapper(),
    });

    expect(result.current.propertyId).toBeUndefined();
    expect(result.current.loading).toBe(false);
    expect(mockGetManagementFileProperties).toHaveBeenCalledWith(99);
  });

  it('returns undefined if managementFileId is invalid', async () => {
    history.location.pathname = '/mapview/sidebar/management/abc/property/123/';
    const { result } = renderHook(() => useManagementPropertyIdFromUrl(), {
      wrapper: wrapper(),
    });
    expect(result.current.propertyId).toBeUndefined();
    expect(result.current.loading).toBe(false);
    expect(mockGetManagementFileProperties).not.toHaveBeenCalled();
  });

  it('returns undefined if propertyFileId is invalid', async () => {
    history.location.pathname = '/mapview/sidebar/management/99/property/abc/';
    const { result } = renderHook(() => useManagementPropertyIdFromUrl(), {
      wrapper: wrapper(),
    });
    expect(result.current.propertyId).toBeUndefined();
    expect(result.current.loading).toBe(false);
    expect(mockGetManagementFileProperties).not.toHaveBeenCalled();
  });

  it('returns undefined if pattern does not match', async () => {
    history.location.pathname = '/some/other/path';
    const { result } = renderHook(() => useManagementPropertyIdFromUrl(), {
      wrapper: wrapper(),
    });
    expect(result.current.propertyId).toBeUndefined();
    expect(result.current.loading).toBe(false);
    expect(mockGetManagementFileProperties).not.toHaveBeenCalled();
  });
});
