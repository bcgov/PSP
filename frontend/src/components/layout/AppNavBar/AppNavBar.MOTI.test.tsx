import React from 'react';
import { render, fireEvent, cleanup } from '@testing-library/react';
import { createMemoryHistory } from 'history';
import { Router } from 'react-router-dom';
import { mount } from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';
import Enzyme from 'enzyme';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { Provider } from 'react-redux';
import { useKeycloak } from '@react-keycloak/web';
import { mountToJson } from 'enzyme-to-json';
import AppNavBar from './AppNavBar';
import Claims from 'constants/claims';
import Roles from 'constants/roles';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { TenantProvider } from 'tenants';

jest.mock('@react-keycloak/web');
Enzyme.configure({ adapter: new Adapter() });

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

describe('[ MOTI ] AppNavBar', () => {
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
    it('Home link should render for all users', () => {
      const { container } = renderNavBar();
      const link = container.querySelector('.home-button');

      expect(link).toBeVisible();
      fireEvent.click(link!);
      expect(history.location.pathname).toBe('/mapview');
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

      it('should include Admin Agencies link', () => {
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Administration'));
        const link = getByText('Agencies');

        expect(link).toBeVisible();
        fireEvent.click(link);
        expect(history.location.pathname).toBe('/admin/agencies');
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

    describe('Disposal Projects dropdown', () => {
      beforeEach(() => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.DISPOSE_APPROVE],
            },
          },
        });
      });
      it('should NOT render', () => {
        const { queryByText } = renderNavBar();
        const element = queryByText('Disposal Projects');
        expect(element).not.toBeInTheDocument();
      });
    });

    describe('Reports Dropdown', () => {
      beforeEach(() => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.REPORTS_VIEW, Claims.REPORTS_SPL],
            },
          },
        });
      });

      it('should render', () => {
        const { getByText } = renderNavBar();
        const link = getByText('Reports');
        expect(link).toBeVisible();
      });

      it('SPL Reports link should NOT render', () => {
        const { queryByText, getByText } = renderNavBar();
        fireEvent.click(getByText('Reports'));
        const link = queryByText('SPL Report');
        expect(link).not.toBeInTheDocument();
      });
    });
  });
});
