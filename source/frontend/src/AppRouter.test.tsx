import axios from 'axios';
import { createMemoryHistory } from 'history';

import AppRouter from './AppRouter';
import { Claims, Roles } from './constants';
import { ADD_ACTIVATE_USER, GET_REQUEST_ACCESS } from './constants/actionTypes';
import { AuthStateContext } from './contexts/authStateContext';
import { IGeocoderResponse } from './hooks/pims-api/interfaces/IGeocoder';
import { useApiAcquisitionFile } from './hooks/pims-api/useApiAcquisitionFile';
import { useApiGeocoder } from './hooks/pims-api/useApiGeocoder';
import { useApiLeases } from './hooks/pims-api/useApiLeases';
import { useApiProperties } from './hooks/pims-api/useApiProperties';
import { useApiResearchFile } from './hooks/pims-api/useApiResearchFile';
import { useApiUsers } from './hooks/pims-api/useApiUsers';
import { IResearchSearchResult } from './interfaces/IResearchSearchResult';
import { mockLookups } from './mocks/lookups.mock';
import { getMockPagedUsers, getUserMock } from './mocks/user.mock';
import { ApiGen_Base_Page } from './models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_AcquisitionFile } from './models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_Lease } from './models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_Property } from './models/api/generated/ApiGen_Concepts_Property';
import { lookupCodesSlice } from './store/slices/lookupCodes';
import { networkSlice } from './store/slices/network/networkSlice';
import { tenantsSlice } from './store/slices/tenants';
import { defaultTenant } from './tenants/config/defaultTenant';
import {
  act,
  mockKeycloak,
  prettyDOM,
  render,
  RenderOptions,
  screen,
  waitFor,
} from './utils/test-utils';
import { renderDate } from './components/Table';
import { vi } from 'vitest';

vi.mock('axios');
const mockedAxios = vi.mocked(axios);

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

// Mock React.Suspense in tests
vi.mock('react', () => {
  const React = vi.importActual('react') as any;
  React.Suspense = ({ children }: any) => children;
  return React as any;
});

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

vi.mock('@/hooks/usePimsIdleTimer');

vi.mock('@/hooks/pims-api/useApiHealth', () => ({
  useApiHealth: () => ({
    getVersion: vi
      .fn()
      .mockResolvedValue({ data: { environment: 'test', informationalVersion: '1.0.0.0' } }),
  }),
}));

vi.mock('@/store/slices/tenants/useTenants', () => ({
  useTenants: () => ({ getSettings: vi.fn() }),
}));

vi.mock('./hooks/pims-api/useApiUsers');
vi.mocked(useApiUsers).mockReturnValue({
  activateUser: vi.fn(),
  getUser: vi.fn().mockResolvedValue({ data: getUserMock() }),
  getUserInfo: vi.fn().mockResolvedValue({ data: getUserMock() }),
  getUsersPaged: vi.fn().mockResolvedValue({ data: getMockPagedUsers() }),
  putUser: vi.fn(),
  exportUsers: vi.fn(),
});

vi.mock('./hooks/pims-api/useApiProperties');
vi.mocked(useApiProperties).mockReturnValue({
  getPropertiesViewPagedApi: vi
    .fn()
    .mockResolvedValue({ data: {} as ApiGen_Base_Page<ApiGen_Concepts_Property> }),
  getMatchingPropertiesApi: vi.fn(),
  getPropertyAssociationsApi: vi.fn(),
  exportPropertiesApi: vi.fn(),
  getPropertiesApi: vi.fn(),
  getPropertyConceptWithIdApi: vi.fn(),
  putPropertyConceptApi: vi.fn(),
  getPropertyConceptWithPidApi: vi.fn(),
  getPropertyConceptWithPinApi: vi.fn(),
});

vi.mock('./hooks/pims-api/useApiLeases');
vi.mocked(useApiLeases).mockReturnValue({
  getLeases: vi.fn().mockResolvedValue({
    data: {
      items: [{ lFileNo: 'l-1234' }],
      page: 1,
      total: 1,
      quantity: 1,
    } as ApiGen_Base_Page<ApiGen_Concepts_Lease>,
  }),
  getApiLease: vi.fn(),
  getLastUpdatedByApi: vi.fn(),
  postLease: vi.fn(),
  putApiLease: vi.fn(),
  exportLeases: vi.fn(),
  exportAggregatedLeases: vi.fn(),
  exportLeasePayments: vi.fn(),
});

vi.mock('./hooks/pims-api/useApiAcquisitionFile');
vi.mocked(useApiAcquisitionFile).mockReturnValue({
  getAcquisitionFiles: vi.fn().mockResolvedValue({
    data: {
      items: [{ fileName: 'test acq file' }],
      page: 1,
      total: 1,
      quantity: 1,
    } as ApiGen_Base_Page<ApiGen_Concepts_AcquisitionFile>,
  }),
  getAcquisitionFile: vi.fn(),
  getLastUpdatedByApi: vi.fn(),
  getAgreementReport: vi.fn(),
  getCompensationReport: vi.fn(),
  exportAcquisitionFiles: vi.fn(),
  postAcquisitionFile: vi.fn(),
  putAcquisitionFile: vi.fn(),
  putAcquisitionFileProperties: vi.fn(),
  getAcquisitionFileProperties: vi.fn(),
  getAcquisitionFileOwners: vi.fn(),
  getAllAcquisitionFileTeamMembers: vi.fn(),
  getAcquisitionFileProject: vi.fn(),
  getAcquisitionFileProduct: vi.fn(),
  getAcquisitionFileChecklist: vi.fn(),
  putAcquisitionFileChecklist: vi.fn(),
  getFileCompensationRequisitions: vi.fn(),
  getFileCompReqH120s: vi.fn(),
  postFileCompensationRequisition: vi.fn(),
  getAcquisitionFileForm8s: vi.fn(),
  postFileForm8: vi.fn(),
});

vi.mock('./hooks/pims-api/useApiResearchFile');
vi.mocked(useApiResearchFile).mockReturnValue({
  getResearchFiles: vi.fn().mockResolvedValue({
    data: {
      items: [{ fileName: 'test research file' }],
      page: 1,
      total: 1,
      quantity: 1,
    } as ApiGen_Base_Page<IResearchSearchResult>,
  }),
  getResearchFile: vi.fn(),
  postResearchFile: vi.fn(),
  putResearchFile: vi.fn(),
  getLastUpdatedByApi: vi.fn(),
  putResearchFileProperties: vi.fn(),
  putPropertyResearchFile: vi.fn(),
  getResearchFileProperties: vi.fn(),
});

vi.mock('./hooks/pims-api/useApiGeocoder');
vi.mocked(useApiGeocoder).mockReturnValue({
  searchAddressApi: vi.fn().mockResolvedValue({ data: [] as IGeocoderResponse[] }),
  getSitePidsApi: vi.fn(),
  getNearestToPointApi: vi.fn(),
});

const mocks = vi.hoisted(() => {
  return {
    useKeycloak: vi.fn(),
  };
});

vi.mock('@react-keycloak/web', () => {
  return {
    useKeycloak: mocks.useKeycloak,
  };
});

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      return children;
    }),
  };
});

describe('PSP routing', () => {
  const setup = (url = '/', renderOptions: RenderOptions = {}) => {
    history.replace(url);

    const defaultUserInfo = {
      organizations: [1],
      client_roles:
        [
          ...(renderOptions?.claims ?? []),
          Claims.LEASE_VIEW,
          Claims.RESEARCH_VIEW,
          Claims.PROJECT_VIEW,
          ...(renderOptions?.roles ?? [Roles.ACQUISITION_FUNCTIONAL]),
        ] ?? [],
      email: 'test@test.com',
      name: 'Chester Tester',
      idir_user_guid: '00000000000000000000000000000000',
    };

    mocks.useKeycloak.mockImplementation(() => ({
      keycloak: {
        userInfo: defaultUserInfo,
        subject: 'test',
        authenticated: !!renderOptions.claims,
        loadUserInfo: vi.fn().mockResolvedValue(defaultUserInfo),
      } as any,
      initialized: true,
    }));

    const utils = render(
      <AuthStateContext.Provider value={{ ready: true }}>
        <AppRouter />
      </AuthStateContext.Provider>,
      {
        ...renderOptions,
        store: storeState,
        history,
        useMockAuthentication: true,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {
    vi.mocked(mockedAxios.get).mockResolvedValue({ data: {}, status: 200 });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  describe('public routes', () => {
    it('should redirect unauthenticated user to the login page', async () => {
      const { getByText } = setup('/');
      await screen.findByText('v1.0.0.0');
      expect(getByText('Sign into PIMS with your government issued IDIR')).toBeVisible();
    });

    it('should show header and footer links', async () => {
      const { getByRole } = setup('/');
      await screen.findByText('v1.0.0.0');
      expect(getByRole('link', { name: 'Disclaimer' })).toHaveAttribute(
        'href',
        'http://www.gov.bc.ca/gov/content/home/disclaimer',
      );
    });

    it('should show a page for non-supported browsers', async () => {
      const { getByText } = setup('/ienotsupported');
      await screen.findByText('v1.0.0.0');
      expect(
        getByText('Please use a supported internet browser such as Chrome, Firefox or Edge.'),
      ).toBeVisible();
    });

    it('should show the access denied page', async () => {
      const { getByText, getByRole } = setup('/forbidden');
      await screen.findByText('v1.0.0.0');
      expect(getByText('You do not have permission to view this page')).toBeVisible();
      expect(getByRole('link', { name: 'Go back to the map' })).toBeVisible();
    });

    it.each(['/page-not-found', '/fake-url'])(
      'should show the not found page when route is %s',
      async url => {
        const { getByText, getByRole } = setup(url);
        await screen.findByText('v1.0.0.0');
        expect(getByText('Page not found')).toBeVisible();
        expect(getByRole('link', { name: 'Go back to the map' })).toBeVisible();
      },
    );
  });

  describe('authenticated routes', () => {
    it('should display the property list view', async () => {
      setup('/properties/list', { claims: [Claims.PROPERTY_VIEW] });
      await screen.findByText('v1.0.0.0');
      const lazyElement = await screen.findByText('Civic Address');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/View Inventory/i);
    });

    it('should display the lease list view', async () => {
      setup('/lease/list', { claims: [Claims.LEASE_VIEW] });
      await screen.findByText('v1.0.0.0');
      const lazyElement = await screen.findByText('l-1234');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/View Lease & Licenses/i);
    });

    it('should display the acquisition list view', async () => {
      setup('/acquisition/list', { claims: [Claims.ACQUISITION_VIEW] });
      await screen.findByText('v1.0.0.0');
      const lazyElement = await screen.findByText('test acq file');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/View Acquisition Files/i);
    });

    it('should display the research list view', async () => {
      setup('/research/list', { claims: [Claims.RESEARCH_VIEW] });
      await screen.findByText('v1.0.0.0');
      const lazyElement = await screen.findByText('test research file');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/View Research Files/i);
    });

    it('should display the admin users page at the expected route', async () => {
      setup('/admin/users', { claims: [Claims.ADMIN_USERS] });
      await screen.findByText('v1.0.0.0');
      const lazyElement = await screen.findByText('Smith');
      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/Users Management/i);
    });

    it('should display the edit user page at the expected route', async () => {
      setup('/admin/user/1', { claims: [Claims.ADMIN_USERS] });
      await screen.findByText('v1.0.0.0');
      const lazyElement = await screen.findByDisplayValue('Smith');

      expect(lazyElement).toBeInTheDocument();
      expect(document.title).toMatch(/Edit User/i);
    });
  });
});
