import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render } from '@testing-library/react';
import * as API from 'constants/API';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import renderer from 'react-test-renderer';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { config, TenantProvider } from 'tenants';

import Header from './Header';

jest.mock('@react-keycloak/web');
afterEach(() => {
  cleanup();
});

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();

const lCodes = {
  lookupCodes: [
    { name: 'agencyVal', id: '1', isDisabled: false, type: API.AGENCY_CODE_SET_NAME },
    { name: 'roleVal', id: '1', isDisabled: false, type: API.ROLE_CODE_SET_NAME },
  ] as ILookupCode[],
};

const store = mockStore({
  [lookupCodesSlice.name]: lCodes,
});

describe('Header tests', () => {
  const OLD_ENV = process.env;

  beforeEach(() => {
    jest.resetModules();
    process.env = {
      ...OLD_ENV,
      REACT_APP_TENANT: 'TEST',
    };
  });

  afterAll(() => {
    process.env = OLD_ENV;
  });

  it('Header renders correctly', () => {
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    const tree = renderer
      .create(
        <TenantProvider>
          <Provider store={store}>
            <Router history={history}>
              <Header />
            </Router>
          </Provider>
        </TenantProvider>,
      )
      .toJSON();
    expect(tree).toMatchSnapshot();
  });

  it('Header renders for MOTI tenant', async () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    const header = render(
      <TenantProvider>
        <Provider store={store}>
          <Router history={history}>
            <Header />
          </Router>
        </Provider>
      </TenantProvider>,
    );
    const result = await header.findByText(config['MOTI'].title);
    expect(result.innerHTML).toBe(config['MOTI'].title);
  });

  it('Header renders for CITZ tenant', async () => {
    process.env.REACT_APP_TENANT = 'CITZ';
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    const header = render(
      <TenantProvider>
        <Provider store={store}>
          <Router history={history}>
            <Header />
          </Router>
        </Provider>
      </TenantProvider>,
    );
    const result = await header.findByText(config['CITZ'].title);
    expect(result.innerHTML).toBe(config['CITZ'].title);
  });

  it('User displays default if no user name information found', () => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        subject: 'test',
        authenticated: true,
        userInfo: {
          roles: [],
        },
      },
    });

    const { getByText } = render(
      <TenantProvider>
        <Provider store={store}>
          <Router history={history}>
            <Header />
          </Router>
        </Provider>
      </TenantProvider>,
    );
    const name = getByText('default');
    expect(name).toBeVisible();
  });

  describe('UserProfile user name display', () => {
    it('Displays keycloak display name if available', () => {
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          subject: 'test',
          authenticated: true,
          userInfo: {
            name: 'display name',
            firstName: 'name',
            roles: [],
          },
        },
      });

      const { getByText } = render(
        <TenantProvider>
          <Provider store={store}>
            <Router history={history}>
              <Header />
            </Router>
          </Provider>
        </TenantProvider>,
      );
      const name = getByText('display name');
      expect(name).toBeVisible();
    });

    it('Displays first last name if no display name', () => {
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          subject: 'test',
          authenticated: true,
          userInfo: {
            roles: [],
            firstName: 'firstName',
            lastName: 'lastName',
          },
        },
      });

      const { getByText } = render(
        <TenantProvider>
          <Provider store={store}>
            <Router history={history}>
              <Header />
            </Router>
          </Provider>
        </TenantProvider>,
      );
      const name = getByText('firstName lastName');
      expect(name).toBeVisible();
    });

    it('displays appropriate agency', () => {
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          subject: 'test',
          authenticated: true,
          userInfo: {
            agencies: ['1'],
            firstName: 'test',
            lastName: 'user',
          },
        },
      });
      const { getByText } = render(
        <TenantProvider>
          <Provider store={store}>
            <Router history={history}>
              <Header />
            </Router>
          </Provider>
        </TenantProvider>,
      );
      fireEvent.click(getByText(/test user/i));
      expect(getByText(/agencyVal/i)).toBeInTheDocument();
    });
  });
});
