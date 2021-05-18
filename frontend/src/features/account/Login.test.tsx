import React from 'react';
import { createMemoryHistory } from 'history';
import { render, fireEvent, cleanup } from '@testing-library/react';
import { Router } from 'react-router-dom';
import renderer from 'react-test-renderer';
import { useKeycloak } from '@react-keycloak/web';
import thunk from 'redux-thunk';
import configureMockStore from 'redux-mock-store';
import Login from './Login';
import { Provider } from 'react-redux';
import { ADD_ACTIVATE_USER } from 'constants/actionTypes';
import { TenantProvider } from 'tenants';
import { networkSlice } from 'store/slices/network/networkSlice';
import { IGenericNetworkAction } from 'store/slices/network/interfaces';

jest.mock('axios');
jest.mock('@react-keycloak/web');
const mockStore = configureMockStore([thunk]);

const store = mockStore({
  [networkSlice.name]: {
    [ADD_ACTIVATE_USER]: {},
  },
});

//boilerplate function used by most tests to wrap the Login component with a router.
const renderLogin = () => {
  process.env.REACT_APP_TENANT = 'CITZ';
  const history = createMemoryHistory();
  return render(
    <TenantProvider>
      <Provider store={store}>
        <Router history={history}>
          <Login />
        </Router>
      </Provider>
    </TenantProvider>,
  );
};

describe('login', () => {
  afterEach(() => {
    cleanup();
  });
  it('login renders correctly', () => {
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    process.env.REACT_APP_TENANT = 'MOTI';
    const history = createMemoryHistory();
    const tree = renderer
      .create(
        <TenantProvider>
          <Provider store={store}>
            <Router history={history}>
              <Login></Login>
            </Router>
          </Provider>
        </TenantProvider>,
      )
      .toJSON();
    expect(tree).toMatchSnapshot();
  });

  it('authenticated users are redirected to the mapview', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: { authenticated: true, userInfo: { groups: ['System Administrator'] } },
    });
    const history = createMemoryHistory();

    render(
      <TenantProvider>
        <Provider store={store}>
          <Router history={history}>
            <Login />
          </Router>
        </Provider>
      </TenantProvider>,
    );
    expect(history.location.pathname).toBe('/mapview');
  });

  it('new users are sent to the guest page', () => {
    process.env.REACT_APP_TENANT = 'CITZ';
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

    render(
      <TenantProvider>
        <Provider store={store}>
          <Router history={history}>
            <Login />
          </Router>
        </Provider>
      </TenantProvider>,
    );
    expect(history.location.pathname).toBe('/access/request');
  });

  it('unAuthenticated users are shown the login screen', () => {
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    const { getAllByRole } = renderLogin();
    expect(getAllByRole('heading')[0]).toHaveTextContent(
      'Search and visualize government property information',
    );
    expect(getAllByRole('heading')[1]).toHaveTextContent(
      'PIMS enables you to search properties owned by the Government of British Columbia',
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
