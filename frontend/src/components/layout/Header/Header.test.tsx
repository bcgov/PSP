import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render, waitFor } from '@testing-library/react';
import * as API from 'constants/API';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { tenantsSlice, useTenants } from 'store/slices/tenants';
import { config } from 'tenants';
import { defaultTenant } from 'tenants';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import Header from './Header';

jest.mock('@react-keycloak/web');
afterEach(() => {
  cleanup();
});

jest.mock('store/slices/tenants/useTenants');
(useTenants as any).mockImplementation(() => ({
  getSettings: jest.fn(),
}));

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();

const lCodes = {
  lookupCodes: [
    { name: 'organizationVal', id: 1, isDisabled: false, type: API.ORGANIZATION_CODE_SET_NAME },
    { name: 'roleVal', id: 2, isDisabled: false, type: API.ROLE_CODE_SET_NAME },
  ] as ILookupCode[],
};

const store = mockStore({
  [lookupCodesSlice.name]: lCodes,
  [tenantsSlice.name]: { defaultTenant },
});

const getHeader = () => (
  <TestCommonWrapper store={store} history={history}>
    <Header />
  </TestCommonWrapper>
);

describe('Header tests', () => {
  const OLD_ENV = process.env;

  beforeEach(() => {
    process.env = {
      ...OLD_ENV,
      REACT_APP_TENANT: 'TEST',
    };
  });

  afterAll(() => {
    process.env = OLD_ENV;
  });

  it('Header renders correctly', async () => {
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    const { container } = render(getHeader());
    await waitFor(async () => {
      expect(container).toMatchSnapshot();
    });
  });

  it('Header renders for MOTI tenant', async () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    const header = render(getHeader());
    await waitFor(async () => {
      const result = await header.findByText(config['MOTI'].title);
      expect(result.innerHTML).toBe(config['MOTI'].title);
    });
  });

  it('Header renders for CITZ tenant', async () => {
    process.env.REACT_APP_TENANT = 'CITZ';
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    const header = render(getHeader());
    await waitFor(async () => {
      const result = await header.findByText(config['CITZ'].title);
      expect(result.innerHTML).toBe(config['CITZ'].title);
    });
  });

  it('User displays default if no user name information found', async () => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        subject: 'test',
        authenticated: true,
        userInfo: {
          roles: [],
        },
      },
    });

    const { getByText } = render(getHeader());
    await waitFor(async () => {
      const name = getByText('default');
      expect(name).toBeVisible();
    });
  });

  describe('UserProfile user name display', () => {
    it('Displays keycloak display name if available', async () => {
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

      const { getByText } = render(getHeader());
      await waitFor(async () => {
        const name = getByText('display name');
        expect(name).toBeVisible();
      });
    });

    it('Displays first last name if no display name', async () => {
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          subject: 'test',
          authenticated: true,
          userInfo: {
            roles: [],
            firstName: 'firstName',
            surname: 'surname',
          },
        },
      });

      const { getByText } = render(getHeader());
      await waitFor(async () => {
        const name = getByText('firstName surname');
        expect(name).toBeVisible();
      });
    });

    it('displays appropriate organization', async () => {
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          subject: 'test',
          authenticated: true,
          userInfo: {
            organizations: ['1'],
            firstName: 'test',
            surname: 'user',
          },
        },
      });
      const { getByText } = render(getHeader());
      fireEvent.click(getByText(/test user/i));
      await waitFor(async () => {
        expect(getByText(/organizationVal/i)).toBeInTheDocument();
      });
    });
  });
});
