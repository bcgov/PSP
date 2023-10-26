import { createMemoryHistory } from 'history';

import AppRouter from './AppRouter';
import { Claims } from './constants';
import { ADD_ACTIVATE_USER, GET_REQUEST_ACCESS } from './constants/actionTypes';
import { AuthStateContext } from './contexts/authStateContext';
import { useApiUsers } from './hooks/pims-api/useApiUsers';
import { mockLookups } from './mocks/lookups.mock';
import { getMockPagedUsers, getUserMock } from './mocks/user.mock';
import { lookupCodesSlice } from './store/slices/lookupCodes';
import { networkSlice } from './store/slices/network/networkSlice';
import { tenantsSlice } from './store/slices/tenants';
import { defaultTenant } from './tenants/config/defaultTenant';
import { mockKeycloak, render, RenderOptions, screen } from './utils/test-utils';

const history = createMemoryHistory();
const storeState = {
  [tenantsSlice.name]: { defaultTenant },
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
  [networkSlice.name]: {
    [ADD_ACTIVATE_USER]: {},
    [GET_REQUEST_ACCESS]: {
      isFetching: false,
    },
  },
  loadingBar: {},
  keycloakReady: true,
};

jest.mock('@react-keycloak/web');

// Mock React.Suspense in tests
jest.mock('react', () => {
  const React = jest.requireActual('react');
  React.Suspense = ({ children }: any) => children;
  return React;
});

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

jest.mock('@/hooks/usePimsIdleTimer');

jest.mock('@/hooks/pims-api/useApiHealth', () => ({
  useApiHealth: () => ({
    getVersion: jest.fn().mockResolvedValue({ data: { environment: 'test', version: '1.0.0.0' } }),
  }),
}));

jest.mock('@/store/slices/tenants/useTenants', () => ({
  useTenants: () => ({ getSettings: jest.fn() }),
}));

jest.mock('./hooks/pims-api/useApiUsers');
(useApiUsers as jest.MockedFunction<typeof useApiUsers>).mockReturnValue({
  activateUser: jest.fn(),
  getUser: jest.fn().mockResolvedValue(getUserMock()),
  getUserInfo: jest.fn().mockResolvedValue(getUserMock()),
  getUsersPaged: jest.fn().mockResolvedValue(getMockPagedUsers()),
  putUser: jest.fn(),
  exportUsers: jest.fn(),
});

describe('PSP routing', () => {
  const setup = (url: string = '/', renderOptions: RenderOptions = {}) => {
    history.replace(url);
    const utils = render(
      <AuthStateContext.Provider value={{ ready: true }}>
        <AppRouter />
      </AuthStateContext.Provider>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  describe('public routes', () => {
    beforeEach(() => {
      mockKeycloak({ authenticated: false });
    });

    it('should redirect unauthenticated user to the login page', async () => {
      const { getByText } = setup('/');
      expect(getByText('Sign into PIMS with your government issued IDIR')).toBeVisible();
    });

    it('should show header and footer links', async () => {
      const { getByRole } = setup('/');
      expect(getByRole('link', { name: 'Disclaimer' })).toHaveAttribute(
        'href',
        'http://www.gov.bc.ca/gov/content/home/disclaimer',
      );
    });

    it('should show a page for non-supported browsers', async () => {
      const { getByText } = setup('/ienotsupported');
      expect(
        getByText('Please use a supported internet browser such as Chrome, Firefox or Edge.'),
      ).toBeVisible();
    });

    it('should show the access denied page', async () => {
      const { getByText, getByRole } = setup('/forbidden');
      expect(getByText('You do not have permission to view this page')).toBeVisible();
      expect(getByRole('link', { name: 'Go back to the map' })).toBeVisible();
    });

    it.each(['/page-not-found', '/fake-url'])(
      'should show the not found page when route is %s',
      async url => {
        const { getByText, getByRole } = setup(url);
        expect(getByText('Page not found')).toBeVisible();
        expect(getByRole('link', { name: 'Go back to the map' })).toBeVisible();
      },
    );
  });

  describe('authenticated routes', () => {
    it('should display the property list view', async () => {
      setup('/properties/list', { claims: [Claims.PROPERTY_VIEW] });
      const lazyElement = await screen.findByText('Civic Address');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/View Inventory/i);
    });

    it('should display the lease list view', async () => {
      setup('/lease/list', { claims: [Claims.LEASE_VIEW] });
      const lazyElement = await screen.findByText('L-File Number');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/View Lease & Licenses/i);
    });

    it('should display the acquisition list view', async () => {
      setup('/acquisition/list', { claims: [Claims.ACQUISITION_VIEW] });
      const lazyElement = await screen.findByText('Acquisition file name');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/View Acquisition Files/i);
    });

    it('should display the research list view', async () => {
      setup('/research/list', { claims: [Claims.RESEARCH_VIEW] });
      const lazyElement = await screen.findByText('File #');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/View Research Files/i);
    });

    it('should display the admin users page at the expected route', async () => {
      setup('/admin/users', { claims: [Claims.ADMIN_USERS] });
      const lazyElement = await screen.findByText('User Management');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/Users Management/i);
    });

    it('should display the edit user page at the expected route', async () => {
      setup('/admin/user/1', { claims: [Claims.ADMIN_USERS] });
      const lazyElement = await screen.findByText('User Information');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/Edit User/i);
    });
  });
});
