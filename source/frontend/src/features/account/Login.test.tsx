import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render } from '@testing-library/react';
import { createMemoryHistory, MemoryHistory } from 'history';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom/cjs/react-router-dom';
import renderer from 'react-test-renderer';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ThemeProvider } from 'styled-components';

import { Roles } from '@/constants';
import { ADD_ACTIVATE_USER } from '@/constants/actionTypes';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { IGenericNetworkAction } from '@/store/slices/network/interfaces';
import { networkSlice } from '@/store/slices/network/networkSlice';
import { TenantConsumer, TenantProvider } from '@/tenants';

import Login from './Login';
import { IKeycloak } from '@/hooks/useKeycloakWrapper';
import Keycloak from 'keycloak-js';

vi.mock('axios');
vi.mock('@react-keycloak/web');

const mockStore = configureMockStore([thunk]);

const defaultStore = mockStore({
  [networkSlice.name]: {
    [ADD_ACTIVATE_USER]: {},
  },
  [lookupCodesSlice.name]: {
    lookupCodes: mockLookups,
  },
});

const TestLogin = ({
  history,
  store,
}: {
  history: MemoryHistory<unknown>;
  store?: MockStoreEnhanced<unknown, {}>;
}) => {
  return (
    <TenantProvider>
      <TenantConsumer>
        {({ tenant }) => (
          <ThemeProvider theme={{ tenant, css: {} }}>
            <Provider store={store ?? defaultStore}>
              <Router history={history}>
                <Login />
              </Router>
            </Provider>
          </ThemeProvider>
        )}
      </TenantConsumer>
    </TenantProvider>
  );
};

//boilerplate function used by most tests to wrap the Login component with a router.
const renderLogin = () => {
  import.meta.env.VITE_TENANT = 'MOTI';
  const history = createMemoryHistory();
  return render(<TestLogin history={history} />);
};

describe('login', () => {
  afterEach(() => {
    cleanup();
  });
  it('login renders correctly', () => {
    vi.mocked(useKeycloak).mockReturnValue({
      keycloak: { authenticated: false } as unknown as Keycloak.KeycloakInstance,
      initialized: true,
    });
    import.meta.env.VITE_TENANT = 'MOTI';
    const history = createMemoryHistory();
    const tree = renderer.create(<TestLogin history={history} />).toJSON();
    expect(tree).toMatchSnapshot();
  });

  it('authenticated users are redirected to the mapview', () => {
    import.meta.env.VITE_TENANT = 'MOTI';
    vi.mocked(useKeycloak).mockReturnValue({
      keycloak: {
        authenticated: true,
        userInfo: { client_roles: [Roles.SYSTEM_ADMINISTRATOR] },
      } as unknown as Keycloak.KeycloakInstance,
      initialized: true,
    });
    const history = createMemoryHistory();
    render(<TestLogin history={history} />);
    expect(history.location.pathname).toBe('/mapview');
  });

  it('authenticated lease functional users are redirected to the lease list', () => {
    import.meta.env.VITE_TENANT = 'MOTI';
    vi.mocked(useKeycloak).mockReturnValue({
      keycloak: {
        authenticated: true,
        userInfo: { client_roles: [Roles.LEASE_FUNCTIONAL] },
      } as unknown as Keycloak.KeycloakInstance,
      initialized: true,
    });
    const history = createMemoryHistory();
    render(<TestLogin history={history} />);
    expect(history.location.pathname).toBe('/lease/list');
  });

  it('new users are sent to the guest page', () => {
    import.meta.env.VITE_TENANT = 'MOTI';
    vi.mocked(useKeycloak).mockReturnValue({
      keycloak: {
        authenticated: true,
        realmAccess: { client_roles: [] },
      } as unknown as Keycloak.KeycloakInstance,
      initialized: true,
    });
    const history = createMemoryHistory();
    const activatedAction: IGenericNetworkAction = {
      isFetching: false,
      name: 'test',
      type: 'POST',
      status: 201,
    };
    const store = mockStore({
      [networkSlice.name]: {
        activateUser: activatedAction,
      },
    });

    render(<TestLogin history={history} store={store} />);
    expect(history.location.pathname).toBe('/access/request');
  });

  it('unAuthenticated users are shown the login screen', () => {
    vi.mocked(useKeycloak).mockReturnValue({
      keycloak: { authenticated: false } as unknown as Keycloak.KeycloakInstance,
      initialized: true,
    });
    const { getAllByRole } = renderLogin();
    expect(getAllByRole('heading')[0]).toHaveTextContent(
      'MOTI Property Information Management System (PIMS)',
    );
    expect(getAllByRole('heading')[1]).toHaveTextContent(
      'PIMS enables you to view highways and properties owned by the Ministry of Transportation and Infrastructure',
    );
  });

  it('a spinner is displayed if keycloak has not yet been initialized', () => {
    vi.mocked(useKeycloak).mockReturnValue({ keycloak: undefined, initialized: true });
    const { container } = renderLogin();
    expect(container.firstChild).toHaveClass('spinner-border');
  });

  it('the login button calls keycloaks login() method', () => {
    const login = vi.fn();
    vi.mocked(useKeycloak).mockReturnValue({
      keycloak: { login: login, authenticated: false } as unknown as Keycloak.KeycloakInstance,
      initialized: true,
    });

    const { getByText } = renderLogin();
    fireEvent.click(getByText(/Sign In/));

    expect(login.mock.calls.length).toBe(1);
  });
});
