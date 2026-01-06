import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { mockLookups } from '@/mocks/lookups.mock';
import {
  getMockApiPropertyManagement,
  getMockApiPropertyManagementPurpose,
} from '@/mocks/propertyManagement.mock';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, cleanup, render, RenderOptions, userEvent, waitForEffects } from '@/utils/test-utils';

import {
  IPropertyManagementDetailViewProps,
  PropertyManagementDetailView,
} from './PropertyManagementDetailView';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();

const mockGetPersonApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

const mockGetOrganizationApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

vi.mock('@/features/contacts/repositories/useOrganizationRepository');
vi.mocked(useOrganizationRepository).mockImplementation(() => ({
  getOrganizationDetail: mockGetOrganizationApi,
}));

vi.mock('@/features/contacts/repositories/usePersonRepository');
vi.mocked(usePersonRepository).mockImplementation(() => ({
  getPersonDetail: mockGetPersonApi,
}));

describe('PropertyManagementDetailView component', () => {
  const setup = async (
    renderOptions?: RenderOptions & { props?: Partial<IPropertyManagementDetailViewProps> },
  ) => {
    renderOptions = renderOptions ?? {};
    const utils = render(
      <PropertyManagementDetailView
        {...renderOptions.props}
        propertyManagement={
          renderOptions.props?.propertyManagement ?? getMockApiPropertyManagement()
        }
        isLoading={renderOptions.props?.isLoading ?? false}
      />,
      {
        ...renderOptions,
        history: history,
        store: storeState,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {});

  afterEach(() => {
    cleanup();
    vi.clearAllMocks();
  });

  it('renders as expected when provided valid data object', async () => {
    const apiManagement: ApiGen_Concepts_PropertyManagement = {
      ...getMockApiPropertyManagement(),
      managementPurposes: [getMockApiPropertyManagementPurpose()],
    };
    const { asFragment } = await setup({ props: { propertyManagement: apiManagement } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', async () => {
    const { getByTestId } = await setup({ props: { isLoading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays existing values if they exist', async () => {
    const apiManagement: ApiGen_Concepts_PropertyManagement = {
      ...getMockApiPropertyManagement(),
      managementPurposes: [getMockApiPropertyManagementPurpose()],
    };
    const { getByText } = await setup({ props: { propertyManagement: apiManagement } });
    expect(getByText('BC Ferries')).toBeVisible();
  });

  it('displays responsible payer person', async () => {
    mockGetPersonApi.execute.mockResolvedValueOnce({
      id: 100,
      surname: 'Monga',
      firstName: 'Aman',
      middleNames: null,
      nameSuffix: null,
      preferredName: null,
      birthDate: null,
      comment: null,
      addressComment: null,
      useOrganizationAddress: false,
      isDisabled: false,
      managementActivityId: null,
      contactMethods: [],
      personAddresses: [],
      personOrganizations: [],
      rowVersion: 1,
    });

    const apiManagement: ApiGen_Concepts_PropertyManagement = {
      ...getMockApiPropertyManagement(),
      responsiblePayerPersonId: 100,
      managementPurposes: [getMockApiPropertyManagementPurpose()],
    };

    const { getByText } = await setup({
      props: { propertyManagement: apiManagement, isLoading: false },
    });
    await waitForEffects();

    expect(mockGetPersonApi.execute).toHaveBeenCalledTimes(1);
    expect(getByText(/Aman Monga/)).toBeVisible();
  });

  it('displays responsible payer organization', async () => {
    mockGetOrganizationApi.execute.mockResolvedValueOnce({
      id: 1000,
      parentOrganizationId: null,
      regionCode: null,
      districtCode: null,
      organizationTypeCode: 'REALTOR',
      identifierTypeCode: 'OTHINCORPNO',
      organizationIdentifier: 'DQ4EVA',
      name: 'TEST COMANY INC.',
      alias: null,
      incorporationNumber: null,
      website: null,
      comment: null,
      isDisabled: false,
      contactMethods: [
        {
          id: 7,
          personId: null,
          organizationId: 3,
          contactMethodType: {
            id: 'WORKPHONE',
            description: 'Work phone',
            isDisabled: false,
            displayOrder: null,
          },
          value: '6049983251',
          rowVersion: 1,
        },
      ],
      organizationAddresses: [
        {
          id: 2,
          organizationId: 3,
          address: {
            id: 2,
            streetAddress1: 'PO Box 2',
            streetAddress2: 'Stealth Camping',
            streetAddress3: 'Walmart Parking Lot',
            municipality: 'South Podunk',
            provinceStateId: 1,
            province: {
              id: 1,
              code: 'BC',
              description: 'British Columbia',
              displayOrder: 10,
            },
            countryId: 1,
            country: {
              id: 1,
              code: 'CA',
              description: 'Canada',
              displayOrder: 1,
            },
            districtCode: null,
            district: null,
            region: null,
            regionCode: null,
            countryOther: null,
            postal: 'H1I B0B',
            latitude: null,
            longitude: null,
            comment: null,
            rowVersion: 1,
          },
          addressUsageType: {
            id: 'MAILADDR',
            description: 'Mailing address',
            isDisabled: true,
            displayOrder: null,
          },
          rowVersion: 1,
        },
      ],
      organizationPersons: [
      ],
      parentOrganization: null,
      rowVersion: 1,
    });


    const apiManagement: ApiGen_Concepts_PropertyManagement = {
      ...getMockApiPropertyManagement(),
      responsiblePayerPersonId: null,
      responsiblePayerOrganizationId: 1000,
      managementPurposes: [getMockApiPropertyManagementPurpose()],
    };

    const { getByText } = await setup({
      props: { propertyManagement: apiManagement, isLoading: false },
    });
    await waitForEffects();

    expect(mockGetOrganizationApi.execute).toHaveBeenCalledTimes(1);
    expect(getByText(/TEST COMANY INC./)).toBeVisible();
  });

  it('does not throw an exception for an invalid data object', async () => {
    const { getByText } = await setup({
      props: { propertyManagement: {} as ApiGen_Concepts_PropertyManagement },
    });
    expect(getByText(/property purpose/i)).toBeVisible();
  });

  it('does not render the edit button if the user does not have management edit permissions', async () => {
    const { queryByTitle } = await setup({ claims: [] });
    const editButton = queryByTitle('Edit property management information');
    expect(editButton).toBeNull();
  });

  it('renders the edit button if the user has management edit permissions', async () => {
    const { getByTitle } = await setup({ claims: [Claims.MANAGEMENT_EDIT] });
    const editButton = getByTitle('Edit property management information');
    expect(editButton).toBeVisible();
  });

  it('switches to Edit mode when edit button is clicked', async () => {
    const { getByTitle } = await setup({ claims: [Claims.MANAGEMENT_EDIT] });
    const editButton = getByTitle('Edit property management information');
    await act(async () => userEvent.click(editButton));
    expect(history.location.search).toBe('?edit=true');
  });

  it('displays lease information for no active lease', async () => {
    const apiManagement: ApiGen_Concepts_PropertyManagement = {
      ...getMockApiPropertyManagement(),
      hasActiveLease: false,
      activeLeaseHasExpiryDate: false,
      managementPurposes: [getMockApiPropertyManagementPurpose()],
    };
    const { queryByTestId } = await setup({ props: { propertyManagement: apiManagement } });
    expect(queryByTestId('active-lease-information')).toHaveTextContent('No');
  });

  it('displays lease information for active lease', async () => {
    const apiManagement: ApiGen_Concepts_PropertyManagement = {
      ...getMockApiPropertyManagement(),
      hasActiveLease: true,
      activeLeaseHasExpiryDate: false,
      managementPurposes: [getMockApiPropertyManagementPurpose()],
    };
    const { queryByTestId } = await setup({ props: { propertyManagement: apiManagement } });
    expect(queryByTestId('active-lease-information')).toHaveTextContent('Yes (No Expiry Date)');
  });

  it('displays lease information for active lease with expiry date', async () => {
    const apiManagement: ApiGen_Concepts_PropertyManagement = {
      ...getMockApiPropertyManagement(),
      hasActiveLease: true,
      activeLeaseHasExpiryDate: true,
      managementPurposes: [getMockApiPropertyManagementPurpose()],
    };
    const { queryByTestId } = await setup({ props: { propertyManagement: apiManagement } });
    expect(queryByTestId('active-lease-information')).toHaveTextContent('Yes');
  });
});
