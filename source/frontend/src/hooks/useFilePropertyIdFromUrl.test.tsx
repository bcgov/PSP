import { renderHook } from '@testing-library/react-hooks';
import { MemoryRouter, Route } from 'react-router-dom';
import React from 'react';
import { useFilePropertyIdFromUrl } from './useFilePropertyIdFromUrl';
import { createMemoryHistory } from 'history';
import TestRouterWrapper from '@/utils/TestRouterWrapper';

describe('useFilePropertyIdFromUrl', () => {
  const history = createMemoryHistory();
  const wrapper =
    () =>
    ({ children }: { children: React.ReactNode }) =>
      (
        <TestRouterWrapper history={history}>
          <Route path="*">{children}</Route>
        </TestRouterWrapper>
      );

  it('returns correct fileId and filePropertyId for default pattern', () => {
    history.location.pathname = '/mapview/sidebar/lease/55/property/123/';
    const { result } = renderHook(() => useFilePropertyIdFromUrl(), { wrapper: wrapper() });
    expect(result.current).toEqual({ fileId: 55, filePropertyId: 123 });
  });

  it('returns nulls if filePropertyId is missing', () => {
    history.location.pathname = '/mapview/sidebar/lease/55/property/';
    const { result } = renderHook(() => useFilePropertyIdFromUrl(), { wrapper: wrapper() });
    expect(result.current).toEqual({ fileId: null, filePropertyId: null });
  });

  it('returns nulls if fileId is missing', () => {
    history.location.pathname = '/mapview/sidebar/lease//property/123/';
    const { result } = renderHook(() => useFilePropertyIdFromUrl(), { wrapper: wrapper() });
    expect(result.current).toEqual({ fileId: null, filePropertyId: null });
  });

  it('returns nulls if filePropertyId is not a number', () => {
    history.location.pathname = '/mapview/sidebar/lease/55/property/abc/';
    const { result } = renderHook(() => useFilePropertyIdFromUrl(), { wrapper: wrapper() });
    expect(result.current).toEqual({ fileId: 55, filePropertyId: null });
  });

  it('returns nulls if fileId is not a number', () => {
    history.location.pathname = '/mapview/sidebar/lease/abc/property/123/';
    const { result } = renderHook(() => useFilePropertyIdFromUrl(), { wrapper: wrapper() });
    expect(result.current).toEqual({ fileId: null, filePropertyId: 123 });
  });

  it('returns nulls if pattern does not match', () => {
    history.location.pathname = '/some/other/path';
    const { result } = renderHook(() => useFilePropertyIdFromUrl(), { wrapper: wrapper() });
    expect(result.current).toEqual({ fileId: null, filePropertyId: null });
  });
});
