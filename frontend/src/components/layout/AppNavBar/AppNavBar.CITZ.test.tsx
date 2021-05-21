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

describe('[ CITZ ] AppNavBar', () => {
  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'CITZ';
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

  describe('AppNavBar Links Based on Security', () => {
    describe('AppNavBar Administration Dropdown', () => {
      it('AppNavBar include Administration Dropdown', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              groups: [Roles.SYSTEM_ADMINISTRATOR],
            },
          },
        });
        const { getByText } = renderNavBar();
        const element = getByText('Administration');

        expect(element).toBeVisible();
      });

      it('AppNavBar include Admin Users Link', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              groups: [Roles.SYSTEM_ADMINISTRATOR],
            },
          },
        });
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Administration'));
        const element = getByText('Users');

        expect(element).toBeVisible();
      });
      it('AppNavBar include Admin Access Requests Link', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              groups: [Roles.SYSTEM_ADMINISTRATOR],
            },
          },
        });
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Administration'));
        const element = getByText('Access Requests');

        expect(element).toBeVisible();
      });

      it('AppNavBar include Admin Agencies link', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              groups: [Roles.SYSTEM_ADMINISTRATOR],
            },
          },
        });
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Administration'));
        const element = getByText('Agencies');

        expect(element).toBeVisible();
      });
    });

    it('AppNavBar include Submit Property Link', () => {
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          subject: 'test',
          userInfo: {
            roles: [Claims.PROPERTY_ADD, Claims.PROPERTY_VIEW],
          },
        },
      });
      const { getByText } = renderNavBar();
      const link = getByText('Submit Property');

      expect(link).toBeTruthy();
    });

    it('AppNavBar include View Inventory Link', () => {
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

      expect(link).toBeTruthy();
    });

    describe('AppNavBar Disposal Projects dropdown', () => {
      it('AppNavBar include Disposal Projects dropdown for admin', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.ADMIN_PROPERTIES, Claims.ADMIN_PROJECTS],
            },
          },
        });
        const { getByText } = renderNavBar();
        const element = getByText('Disposal Projects');

        expect(element).toBeVisible();
      });
      it('AppNavBar include Disposal Projects dropdown for Approval requests only', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.DISPOSE_APPROVE],
            },
          },
        });
        const { getByText } = renderNavBar();
        const element = getByText('Disposal Projects');

        expect(element).toBeVisible();
      });
      it('AppNavBar include Create Disposal Project Link', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.ADMIN_PROPERTIES, Claims.ADMIN_PROJECTS],
            },
          },
        });
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Disposal Projects'));
        const link = getByText('Create Disposal Project');

        expect(link).toBeVisible();
      });

      it('AppNavBar include View Projects Link', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.ADMIN_PROPERTIES, Claims.ADMIN_PROJECTS],
            },
          },
        });
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Disposal Projects'));
        const link = getByText('View Projects');

        expect(link).toBeVisible();
      });

      it('AppNavBar include Approval Requests Link', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.DISPOSE_APPROVE],
            },
          },
        });
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Disposal Projects'));
        const link = getByText('Approval Requests');

        expect(link).toBeVisible();
      });
    });

    describe('AppNavBar Reports Dropdown', () => {
      it('AppNavBar include Reports Dropdown', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.REPORTS_VIEW, Claims.REPORTS_SPL],
            },
          },
        });
        const { getByText } = renderNavBar();
        const link = getByText('Reports');
        expect(link).toBeVisible();
      });

      it('AppNavBar include SPL Reports link', () => {
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            subject: 'test',
            userInfo: {
              roles: [Claims.REPORTS_VIEW, Claims.REPORTS_SPL],
            },
          },
        });
        const { getByText } = renderNavBar();
        fireEvent.click(getByText('Reports'));
        const link = getByText('SPL Report');

        expect(link).toBeVisible();
      });
    });
  });
});
