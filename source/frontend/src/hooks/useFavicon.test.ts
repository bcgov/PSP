import { renderHook } from '@testing-library/react-hooks';

import { useTenant } from '@/tenants';

import { ITenantConfig2 } from './pims-api/interfaces/ITenantConfig';
import { useFavicon } from './useFavicon';

jest.mock('@/tenants', () => ({
  useTenant: jest.fn(),
}));

const mockUseTenant = useTenant as jest.Mock;
const baseUrl = 'http://localhost/';

describe('useFavicon hook', () => {
  beforeAll(() => {
    var favicon = document.createElement('link');
    favicon.id = 'favicon';
    favicon.rel = 'icon';
    document.head.appendChild(favicon);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('returns empty link when tenant data is unavailable', () => {
    mockUseTenant.mockReturnValue({} as ITenantConfig2);
    const { result } = renderHook(useFavicon);
    expect(result.current.href).toBe(baseUrl);
  });

  it('returns valid icon link from tenant', () => {
    mockUseTenant.mockReturnValue({ logo: { favicon: 'test.ico' } });
    const { result } = renderHook(useFavicon);
    expect(result.current.href).toBe(`${baseUrl}test.ico`);
  });
});
