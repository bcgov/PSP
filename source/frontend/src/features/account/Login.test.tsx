import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render } from '@testing-library/react';
import { ADD_ACTIVATE_USER } from 'constants/actionTypes';
import { createMemoryHistory, MemoryHistory } from 'history';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import renderer from 'react-test-renderer';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { IGenericNetworkAction } from 'store/slices/network/interfaces';
import { networkSlice } from 'store/slices/network/networkSlice';
import { ThemeProvider } from 'styled-components';
import { TenantConsumer, TenantProvider } from 'tenants';

import Login from './Login';

jest.mock('axios');
jest.mock('@react-keycloak/web');
const mockStore = configureMockStore([thunk]);

const defaultStore = mockStore({
  [networkSlice.name]: {
    [ADD_ACTIVATE_USER]: {},
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
  process.env.REACT_APP_TENANT = 'MOTI';
  const history = createMemoryHistory();
  return render(<TestLogin history={history} />);
};

describe('login', () => {
  afterEach(() => {
    cleanup();
  });
  it('login renders correctly', () => {
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    process.env.REACT_APP_TENANT = 'MOTI';
    const history = createMemoryHistory();
    const tree = renderer.create(<TestLogin history={history} />).toJSON();
    expect(tree).toMatchSnapshot();
  });

  it('authenticated users are redirected to the mapview', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: { authenticated: true, userInfo: { groups: ['System Administrator'] } },
    });
    const history = createMemoryHistory();
    render(<TestLogin history={history} />);
    expect(history.location.pathname).toBe('/mapview');
  });

  it('authenticated finance users are redirected to the lease list', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: { authenticated: true, userInfo: { groups: ['Finance'] } },
    });
    const history = createMemoryHistory();
    render(<TestLogin history={history} />);
    expect(history.location.pathname).toBe('/lease/list');
  });

  it('new users are sent to the guest page', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: { authenticated: true, realmAccess: { roles: [{}] } },
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
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    const { getAllByRole } = renderLogin();
    expect(getAllByRole('heading')[0]).toHaveTextContent(
      'TRAN Property Information Management System (PIMS)',
    );
    expect(getAllByRole('heading')[1]).toHaveTextContent(
      'PIMS enables you to view highways and properties owned by the Ministry of Transportation and Infrastructure',
    );
  });

  it('a spinner is displayed if keycloak has not yet been initialized', () => {
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: undefined });
    const { container } = renderLogin();
    expect(container.firstChild).toHaveClass('spinner-border');
  });

  it('the login button calls keycloaks login() method', () => {
    const login = jest.fn();
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: { login: login, authenticated: false },
    });

    const { getByText } = renderLogin();
    fireEvent.click(getByText(/Sign In/));

    expect(login.mock.calls.length).toBe(1);
  });
});
