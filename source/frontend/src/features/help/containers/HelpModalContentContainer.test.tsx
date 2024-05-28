import { act, render, screen, waitFor } from '@/utils/test-utils';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import HelpModalContentContainer from './HelpModalContentContainer';

const mockStore = configureMockStore([thunk]);
const store = mockStore({});

describe('HelpModalContentContainer component', () => {
  const setup = () =>
    render(<HelpModalContentContainer setMailto={vi.fn()} />, {
      useMockAuthentication: true,
      store,
    });

  beforeEach(() => {
    import.meta.env.VITE_TENANT = 'MOTI';
  });

  it('renders correctly', async () => {
    const { asFragment } = setup();
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('populates email from keycloak correctly', async () => {
    setup();
    await act(async () => {});
    const email = await waitFor(() => screen.getByDisplayValue('test@test.com'));
    expect(email).toBeInTheDocument();
  });

  it('populates name from keycloak correctly', async () => {
    setup();
    await act(async () => {});
    const name = await waitFor(() => screen.getByDisplayValue('Chester Tester'));
    expect(name).toBeInTheDocument();
  });
});
