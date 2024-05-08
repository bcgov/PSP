import { act, cleanup, render, screen, userEvent, waitFor } from '@/utils/test-utils';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { mockLookups } from '@/mocks/index.mock';

import HelpContainer from './HelpContainer';

const mockStore = configureMockStore([thunk]);
const store = mockStore({});
const mockData = {
  config: { settings: { helpDeskEmail: 'test@test.com' } },
};

vi.mock('@/store/hooks', () => ({
  useAppSelector: () => mockData,
  useAppDispatch: () => vi.fn(),
}));

vi.mock('@/hooks/useLookupCodeHelpers');
vi.mocked(useLookupCodeHelpers).mockReturnValue({
  getByType: () => mockLookups,
} as unknown as ReturnType<typeof useLookupCodeHelpers>);

describe('HelpContainer component', () => {
  beforeEach(() => {
    import.meta.env.VITE_TENANT = 'MOTI';
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
    const { getByText, queryByText } = render(<HelpContainer />, {
      useMockAuthentication: true,
      store,
    });

    const link = getByText('Help');
    expect(link).toBeInTheDocument();
    await act(async () => userEvent.click(link));
    expect(getByText('Get started with PIMS')).toBeVisible();

    const cancelBtn = await screen.getByTitle('cancel-modal');
    expect(cancelBtn).toBeVisible();
    await act(async () => userEvent.click(cancelBtn));
    await waitFor(() => expect(queryByText('Get started with PIMS')).toBeNull());
  });
});
