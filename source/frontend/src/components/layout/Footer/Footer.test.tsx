import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import IApiVersion from '@/hooks/pims-api/interfaces/IApiVersion';
import { createMemoryHistory } from 'history';
import { RenderOptions, render } from '@/utils/test-utils';
import Footer from './Footer';

const defaultVersion: IApiVersion = {
  environment: 'test',
  version: '11.1.1.1',
  fileVersion: '11.1.1.1',
  informationalVersion: '11.1.1-1.999',
};

const mockGetVersion = vi.fn(async () => {
  return Promise.resolve({ data: defaultVersion });
});

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
  it('renders correctly', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });
});
