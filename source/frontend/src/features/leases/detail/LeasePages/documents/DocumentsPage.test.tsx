import { useKeycloak } from '@react-keycloak/web';
import { createMemoryHistory } from 'history';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, RenderResult } from 'utils/test-utils';
import DocumentsPage from './DocumentsPage';

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    subject: 'test',
    authenticated: true,
    userInfo: {
      roles: [],
    },
  },
});

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};
const history = createMemoryHistory();

describe('Lease Documents Page', () => {
  const setup = (renderOptions: RenderOptions = {}): RenderResult => {
    // render component under test
    const result = render(<DocumentsPage />, {
      ...renderOptions,
      store: storeState,
      history,
    });
    return result;
  };

  it('renders as expected', () => {
    const result = setup();
    expect(result.asFragment()).toMatchSnapshot();
  });
});
