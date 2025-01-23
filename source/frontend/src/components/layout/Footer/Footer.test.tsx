import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import IApiVersion from '@/hooks/pims-api/interfaces/IApiVersion';
import { RenderOptions, render, waitForEffects } from '@/utils/test-utils';

import Footer from './Footer';

const defaultVersion: IApiVersion = {
  environment: 'test',
  version: '11.1.1.1',
  fileVersion: '11.1.1.1',
  informationalVersion: '11.1.1-1.999',
  dbVersion: '93.00',
};

const mockGetVersion = vi.fn();

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();

const store = mockStore({});

const setup = (renderOptions: RenderOptions = {}) => {
  const utils = render(<Footer />, {
    store,
    history,
    useMockAuthentication: true,
    keycloakMock: renderOptions.keycloakMock,
  });
  return { ...utils };
};

vi.mock('@/hooks/pims-api/useApiHealth', () => ({
  useApiHealth: () => ({
    getVersion: mockGetVersion,
  }),
}));

describe('Footer', () => {
  afterEach(() => {
    vi.clearAllMocks();
  });

  beforeEach(() => {
    import.meta.env.VITE_PACKAGE_VERSION = '11.1.1-93.999';
    mockGetVersion.mockResolvedValue({ data: defaultVersion } as any);
  });

  it('renders correctly', async () => {
    const { asFragment } = setup({});
    await waitForEffects();
    expect(asFragment()).toMatchSnapshot();
  });
});
