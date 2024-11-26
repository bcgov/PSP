import IApiVersion from '@/hooks/pims-api/interfaces/IApiVersion';

import { ApiVersionInfo } from './ApiVersionInfo';
import { act, render, waitForEffects, RenderOptions } from '@/utils/test-utils';
import { useApiHealth } from '@/hooks/pims-api/useApiHealth';

const defaultVersion: IApiVersion = {
  environment: 'test',
  version: '11.1.1.1',
  fileVersion: '11.1.1.1',
  informationalVersion: '11.1.1-93.999',
  dbVersion: '93.00',
};

const mockGetVersionApi = vi.fn();
const mockGetLiveApi = vi.fn();
const mockGetReady = vi.fn();

vi.mock('@/hooks/pims-api/useApiHealth');
vi.mocked(useApiHealth).mockReturnValue({
  getVersion: mockGetVersionApi,
  getLive: mockGetLiveApi,
  getReady: mockGetReady,
});

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
    import.meta.env.VITE_PACKAGE_VERSION = '11.1.1-93.999';
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
    expect(element).toHaveTextContent('v11.1.1-93.999');
    expect(mockGetVersionApi).toHaveBeenCalledTimes(1);
  });

  it('Does not display version warning', async () => {
    const { queryByTestId } = setup();
    await waitForEffects();

    const element = queryByTestId(`version-mismatch-warning`);
    expect(element).not.toBeInTheDocument();
    expect(mockGetVersionApi).toHaveBeenCalledTimes(1);
  });

  it('Does display version warning when API missmatch', async () => {
    mockGetVersionApi.mockResolvedValue({
      data: { ...defaultVersion, informationalVersion: 'xx' },
    } as any);

    const { queryByTestId } = setup();
    await waitForEffects();

    const element = queryByTestId(`version-mismatch-warning`);
    expect(element).toBeInTheDocument();
    expect(mockGetVersionApi).toHaveBeenCalledTimes(1);
  });

  it('Does display version warning when DB missmatch', async () => {
    mockGetVersionApi.mockResolvedValue({
      data: { ...defaultVersion, dbVersion: '00' },
    } as any);

    const { queryByTestId } = setup();
    await waitForEffects();

    const element = queryByTestId(`version-mismatch-warning`);
    expect(element).toBeInTheDocument();
    expect(mockGetVersionApi).toHaveBeenCalledTimes(1);
  });
});
