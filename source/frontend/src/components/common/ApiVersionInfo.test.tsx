import IApiVersion from '@/hooks/pims-api/interfaces/IApiVersion';
import { useApiHealth } from '@/hooks/pims-api/useApiHealth';
import defaultTenant from '@/tenants/config/defaultTenant';
import { useTenant } from '@/tenants/useTenant';
import { render, RenderOptions, waitForEffects } from '@/utils/test-utils';

import { ApiVersionInfo } from './ApiVersionInfo';
import ISystemCheck from '@/hooks/pims-api/interfaces/ISystemCheck';
import { AxiosResponse } from 'axios';

const defaultVersion: IApiVersion = {
  environment: 'test',
  version: '11.1.1.1',
  fileVersion: '11.1.1.1',
  informationalVersion: '11.1.1.999',
  dbVersion: '93.00',
};

const mockGetVersionApi = vi.fn();
const mockGetLiveApi = vi.fn();
const mockGetReady = vi.fn();
const mockGetSystemCheckApi = vi.fn();
vi.mock('@/hooks/pims-api/useApiHealth');
vi.mock('@/tenants/useTenant');
vi.mocked(useApiHealth).mockReturnValue({
  getVersion: mockGetVersionApi,
  getLive: mockGetLiveApi,
  getReady: mockGetReady,
  getSystemCheck: mockGetSystemCheckApi,
});
const mockUseTenant = vi.mocked(useTenant);

describe('ApiVersionInfo suite', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(<ApiVersionInfo />, {
      ...renderOptions,
    });

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  beforeEach(() => {
    import.meta.env.VITE_PACKAGE_VERSION = '11.1.1.999';
    mockUseTenant.mockReturnValue(defaultTenant);
    mockGetVersionApi.mockResolvedValue({ data: defaultVersion } as any);
  });

  it('Displays version component', async () => {
    const { asFragment } = setup();
    await waitForEffects();

    expect(asFragment()).toMatchSnapshot();
  });

  it('Displays version information', async () => {
    const { getByTestId } = setup();
    await waitForEffects();

    const element = getByTestId(`version-tag`);
    expect(element).toHaveTextContent('v11.1.1');
    expect(mockGetVersionApi).toHaveBeenCalledTimes(1);
  });
});
