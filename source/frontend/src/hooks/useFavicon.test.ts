import { renderHook } from '@testing-library/react-hooks';

import { useFavicon } from './useFavicon';

jest.mock('@/tenants', () => ({
  useTenant: () => ({ logo: { favicon: 'test' } }),
}));

const spy = jest
  .spyOn(document, 'getElementById')
  .mockImplementation(() => document.createElement('<link id="favicon" />'));

describe('useFavicon suite', () => {
  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('useFavicon returns undefined link', () => {
    renderHook(() => {
      const link = useFavicon();
      expect(link).toBeUndefined();
    });
  });

  it('useFavicon returns empty link', () => {
    renderHook(() => {
      const link = useFavicon();
      expect(link.href).toBeEmpty(); // TODO: PSP-4409 This test doesn't actually work.
      expect(spy).toBeCalledTimes(1);
    });
  });

  it('useFavicon returns tenant link', () => {
    renderHook(() => {
      const link = useFavicon();
      expect(link.href).toBe('test'); // TODO: PSP-4409 This test doesn't actually work.
    });
  });
});
