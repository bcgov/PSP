import { renderHook } from '@testing-library/react-hooks';

import { useTenant } from '@/tenants';

import { ITenantConfig2 } from './pims-api/interfaces/ITenantConfig';
import { useFavicon } from './useFavicon';

vi.mock('@/tenants', () => ({
  useTenant: vi.fn(),
}));

const mockUseTenant = vi.mocked(useTenant);
const baseUrl = 'http://localhost:3000/';

describe('useFavicon hook', () => {
  beforeAll(() => {
    var favicon = document.createElement('link');
    favicon.id = 'favicon';
    favicon.rel = 'icon';
    document.head.appendChild(favicon);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('returns empty link when tenant data is unavailable', () => {
    mockUseTenant.mockReturnValue({} as ITenantConfig2);
    const { result } = renderHook(useFavicon);
    expect(result.current.href).toBe(baseUrl);
  });

  it('returns valid icon link from tenant', () => {
    mockUseTenant.mockReturnValue({ logo: { favicon: 'test.ico' } } as ITenantConfig2);
    const { result } = renderHook(useFavicon);
    expect(result.current.href).toBe(`${baseUrl}test.ico`);
  });
});
