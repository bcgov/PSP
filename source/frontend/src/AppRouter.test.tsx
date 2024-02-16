import axios from 'axios';
import { createMemoryHistory } from 'history';

import AppRouter from './AppRouter';
import { Claims } from './constants';
import { ADD_ACTIVATE_USER, GET_REQUEST_ACCESS } from './constants/actionTypes';
import { AuthStateContext } from './contexts/authStateContext';
import { IGeocoderResponse } from './hooks/pims-api/interfaces/IGeocoder';
import { useApiAcquisitionFile } from './hooks/pims-api/useApiAcquisitionFile';
import { useApiGeocoder } from './hooks/pims-api/useApiGeocoder';
import { useApiLeases } from './hooks/pims-api/useApiLeases';
import { useApiProperties } from './hooks/pims-api/useApiProperties';
import { useApiResearchFile } from './hooks/pims-api/useApiResearchFile';
import { useApiUsers } from './hooks/pims-api/useApiUsers';
import { ILeaseSearchResult, IPagedItems, IProperty } from './interfaces';
import { IResearchSearchResult } from './interfaces/IResearchSearchResult';
import { mockLookups } from './mocks/lookups.mock';
import { getMockPagedUsers, getUserMock } from './mocks/user.mock';
import { ApiGen_Concepts_AcquisitionFile } from './models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { lookupCodesSlice } from './store/slices/lookupCodes';
import { networkSlice } from './store/slices/network/networkSlice';
import { tenantsSlice } from './store/slices/tenants';
import { defaultTenant } from './tenants/config/defaultTenant';
import { mockKeycloak, render, RenderOptions, screen } from './utils/test-utils';

jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

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
  getUser: jest.fn().mockResolvedValue({ data: getUserMock() }),
  getUserInfo: jest.fn().mockResolvedValue({ data: getUserMock() }),
  getUsersPaged: jest.fn().mockResolvedValue({ data: getMockPagedUsers() }),
  putUser: jest.fn(),
  exportUsers: jest.fn(),
});

jest.mock('./hooks/pims-api/useApiProperties');
(useApiProperties as jest.MockedFunction<typeof useApiProperties>).mockReturnValue({
  getPropertiesPagedApi: jest.fn().mockResolvedValue({ data: {} as IPagedItems<IProperty> }),
  getMatchingPropertiesApi: jest.fn(),
  getPropertyAssociationsApi: jest.fn(),
  exportPropertiesApi: jest.fn(),
  getPropertiesApi: jest.fn(),
  getPropertyConceptWithIdApi: jest.fn(),
  putPropertyConceptApi: jest.fn(),
});

jest.mock('./hooks/pims-api/useApiLeases');
(useApiLeases as jest.MockedFunction<typeof useApiLeases>).mockReturnValue({
  getLeases: jest.fn().mockResolvedValue({ data: {} as IPagedItems<ILeaseSearchResult> }),
  getApiLease: jest.fn(),
  getLastUpdatedByApi: jest.fn(),
  postLease: jest.fn(),
  putApiLease: jest.fn(),
  exportLeases: jest.fn(),
  exportAggregatedLeases: jest.fn(),
  exportLeasePayments: jest.fn(),
});

jest.mock('./hooks/pims-api/useApiAcquisitionFile');
(useApiAcquisitionFile as jest.MockedFunction<typeof useApiAcquisitionFile>).mockReturnValue({
  getAcquisitionFiles: jest
    .fn()
    .mockResolvedValue({ data: {} as IPagedItems<ApiGen_Concepts_AcquisitionFile> }),
  getAcquisitionFile: jest.fn(),
  getLastUpdatedByApi: jest.fn(),
  getAgreementReport: jest.fn(),
  getCompensationReport: jest.fn(),
  exportAcquisitionFiles: jest.fn(),
  postAcquisitionFile: jest.fn(),
  putAcquisitionFile: jest.fn(),
  putAcquisitionFileProperties: jest.fn(),
  getAcquisitionFileProperties: jest.fn(),
  getAcquisitionFileOwners: jest.fn(),
  getAllAcquisitionFileTeamMembers: jest.fn(),
  getAcquisitionFileProject: jest.fn(),
  getAcquisitionFileProduct: jest.fn(),
  getAcquisitionFileChecklist: jest.fn(),
  putAcquisitionFileChecklist: jest.fn(),
  getFileCompensationRequisitions: jest.fn(),
  getFileCompReqH120s: jest.fn(),
  postFileCompensationRequisition: jest.fn(),
  getAcquisitionFileForm8s: jest.fn(),
  postFileForm8: jest.fn(),
});

jest.mock('./hooks/pims-api/useApiResearchFile');
(useApiResearchFile as jest.MockedFunction<typeof useApiResearchFile>).mockReturnValue({
  getResearchFiles: jest.fn().mockResolvedValue({ data: {} as IPagedItems<IResearchSearchResult> }),
  getResearchFile: jest.fn(),
  postResearchFile: jest.fn(),
  putResearchFile: jest.fn(),
  getLastUpdatedByApi: jest.fn(),
  putResearchFileProperties: jest.fn(),
  putPropertyResearchFile: jest.fn(),
  getResearchFileProperties: jest.fn(),
});

jest.mock('./hooks/pims-api/useApiGeocoder');
(useApiGeocoder as jest.MockedFunction<typeof useApiGeocoder>).mockReturnValue({
  searchAddressApi: jest.fn().mockResolvedValue({ data: [] as IGeocoderResponse[] }),
  getSitePidsApi: jest.fn(),
  getNearestToPointApi: jest.fn(),
});

describe('PSP routing', () => {
  const setup = (url = '/', renderOptions: RenderOptions = {}) => {
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

  beforeEach(() => {
    mockedAxios.get.mockResolvedValue({ data: {}, status: 200 });
  });

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
