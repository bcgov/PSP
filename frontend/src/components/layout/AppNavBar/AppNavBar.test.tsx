import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render } from '@testing-library/react';
import Claims from 'constants/claims';
import Roles from 'constants/roles';
import { mount } from 'enzyme';
import { mountToJson } from 'enzyme-to-json';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { TenantProvider } from 'tenants';

import AppNavBar from './AppNavBar';

jest.mock('@react-keycloak/web');

(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      agencies: ['1'],
    },
    subject: 'test',
  },
});

const mockStore = configureMockStore([thunk]);

const history = createMemoryHistory();

const store = mockStore({
  [lookupCodesSlice.name]: { lookupCodes: [] },
});

const renderNavBar = () =>
  render(
    <TenantProvider>
      <Provider store={store}>
        <Router history={history}>
          <AppNavBar />
        </Router>
      </Provider>
    </TenantProvider>,
  );

describe('AppNavBar', () => {
  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'MOTI';
  });
  afterEach(() => {
    delete process.env.REACT_APP_TENANT;
    cleanup();
  });

  it('matches snapshot.', () => {
    const tree = mount(
      <TenantProvider>
        <Provider store={store}>
          <Router history={history}>
            <AppNavBar />
          </Router>
        </Provider>
      </TenantProvider>,
    );
    expect(mountToJson(tree.find(AppNavBar))).toMatchSnapshot();
  });

  describe('Renders navigation links based on security', () => {
    describe('Home Button', () => {
      it('should render for existing users', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.PROPERTY_VIEW],
              groups: [Roles.REAL_ESTATE_MANAGER],
            },
          },
        });
        const { getByLabelText } = renderNavBar();
        const link = getByLabelText(/Home/i);

        expect(link).toBeVisible();
        fireEvent.click(link!);
        expect(history.location.pathname).toBe('/mapview');
      });

      it('should NOT render for new users requesting access', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [],
              groups: [],
            },
          },
        });
        const { queryByLabelText } = renderNavBar();
        const link = queryByLabelText(/Home/i);
        expect(link).not.toBeInTheDocument();
      });
    });

    describe('Administration Dropdown', () => {
      beforeEach(() => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              groups: [Roles.SYSTEM_ADMINISTRATOR],
            },
          },
        });
      });

      it('should render', () => {
        const { getByText } = renderNavBar();
        const dropdown = getByText('Administration');
        expect(dropdown).toBeVisible();
      });

      it('should include Admin Users Link', () => {
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Administration'));
        const link = getByText('Users');

        expect(link).toBeVisible();
        fireEvent.click(link);
        expect(history.location.pathname).toBe('/admin/users');
      });

      it('should include Admin Access Requests Link', () => {
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Administration'));
        const link = getByText('Access Requests');

        expect(link).toBeVisible();
        fireEvent.click(link);
        expect(history.location.pathname).toBe('/admin/access/requests');
      });
    });

    it('Submit Property Link should NOT render', () => {
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          subject: 'test',
          userInfo: {
            roles: [Claims.PROPERTY_ADD, Claims.PROPERTY_VIEW],
          },
        },
      });
      const { queryByText } = renderNavBar();
      const link = queryByText('Submit Property');
      expect(link).not.toBeInTheDocument();
    });

    it('View Inventory Link should render', () => {
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          subject: 'test',
          userInfo: {
            roles: [Claims.PROPERTY_ADD, Claims.PROPERTY_VIEW],
          },
        },
      });
      const { getByText } = renderNavBar();
      const link = getByText('View Property Inventory');

      expect(link).toBeVisible();
      fireEvent.click(link);
      expect(history.location.pathname).toBe('/properties/list');
    });
  });
});
