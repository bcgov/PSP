import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import * as API from 'constants/API';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { tenantsSlice, useTenants } from 'store/slices/tenants';
import { config } from 'tenants';
import { defaultTenant } from 'tenants';
import { cleanup, fireEvent, mockKeycloak, render } from 'utils/test-utils';

import Header from './Header';

jest.mock('@react-keycloak/web');

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

const setup = () => {
  const utils = render(<Header />, { store, history });
  return { ...utils };
};

const mockAxios = new MockAdapter(axios);

const ORIG_ENV = process.env;

describe('App Header', () => {
  beforeEach(() => {
    process.env = { ...ORIG_ENV };
    mockAxios.onGet('/tenants/tenant.json').reply(200, config['MOTI']);
  });

  afterEach(() => {
    mockAxios.reset();
    cleanup();
  });

  afterAll(() => {
    process.env = ORIG_ENV;
  });

  it('renders correctly', async () => {
    mockKeycloak({ authenticated: false });
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders for MOTI tenant', async () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    mockKeycloak({ authenticated: false });
    const { findByText } = setup();
    expect(await findByText(config['MOTI'].title)).toBeInTheDocument();
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

    const { findByText } = setup();
    expect(await findByText('default')).toBeVisible();
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

      const { findByText } = setup();
      expect(await findByText('display name')).toBeVisible();
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

      const { findByText } = setup();
      expect(await findByText('firstName surname')).toBeVisible();
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

      const { findByText } = setup();
      const userLink = await findByText(/test user/i);
      expect(userLink).toBeVisible();
      fireEvent.click(userLink);
      expect(await findByText(/organizationVal/i)).toBeInTheDocument();
    });
  });
});
