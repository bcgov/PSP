import { act, cleanup, render, userEvent } from '@/utils/test-utils';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { mockLookups } from '@/mocks/index.mock';

import HelpContainer from './HelpContainer';

jest.mock('@react-keycloak/web');

const mockStore = configureMockStore([thunk]);
const store = mockStore({});
const mockData = {
  config: { settings: { helpDeskEmail: 'test@test.com' } },
};

jest.mock('@/store/hooks', () => ({
  useAppSelector: () => mockData,
  useAppDispatch: () => jest.fn(),
}));

jest.mock('@/hooks/useLookupCodeHelpers');
(useLookupCodeHelpers as jest.Mock).mockReturnValue({ getByType: () => mockLookups });

describe('HelpContainer component', () => {
  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'MOTI';
  });
  afterEach(() => {
    cleanup();
  });

  it('renders correctly', () => {
    const { asFragment } = render(<HelpContainer />, { useMockAuthentication: true, store });
    expect(asFragment()).toMatchSnapshot();
  });

  it(`displays help modal dialog when "Help" link is clicked`, async () => {
    const { getByText } = render(<HelpContainer />, { useMockAuthentication: true, store });
    const link = getByText('Help');
    expect(link).toBeInTheDocument();
    await act(async () => userEvent.click(link));

    expect(getByText('Get started with PIMS')).toBeVisible();
  });

  it(`dismisses the help modal dialog when "Cancel" is clicked`, async () => {
    const { getByText, getByTitle, queryByText } = render(<HelpContainer />, {
      useMockAuthentication: true,
      store,
    });
    const link = getByText('Help');
    expect(link).toBeInTheDocument();
    await act(async () => userEvent.click(link));

    expect(getByText('Get started with PIMS')).toBeVisible();

    await act(async () => userEvent.click(getByTitle('cancel-modal')));
    expect(queryByText('Get started with PIMS')).toBeNull();
  });
});
