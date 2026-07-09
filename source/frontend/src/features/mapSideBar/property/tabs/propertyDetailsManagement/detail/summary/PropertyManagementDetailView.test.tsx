import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { mockLookups } from '@/mocks/lookups.mock';
import {
  getMockApiPropertyManagement,
  getMockApiPropertyManagementPurpose,
} from '@/mocks/propertyManagement.mock';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  getMockRepositoryObj,
  render,
  RenderOptions,
  userEvent,
  waitForEffects,
} from '@/utils/test-utils';

import {
  IPropertyManagementDetailViewProps,
  PropertyManagementDetailView,
} from './PropertyManagementDetailView';
import { getMockOrganization } from '@/mocks/organization.mock';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();

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
        responsiblePayerPerson={renderOptions.props?.responsiblePayerPerson ?? undefined}
        responsiblePayerOrganization={renderOptions.props?.responsiblePayerOrganization ?? undefined}
        primaryContact={renderOptions.props?.primaryContact ?? undefined}
      />,
      {
        ...renderOptions,
        history: history,
        store: storeState,
        useMockAuthentication: true,
      },
    );

    await waitForEffects();

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
  const apiManagement: ApiGen_Concepts_PropertyManagement = {
    ...getMockApiPropertyManagement(),
    responsiblePayerPersonId: 100,
    managementPurposes: [getMockApiPropertyManagementPurpose()],
  };

  const { getByText } = await setup({
    props: {
      propertyManagement: apiManagement,
      isLoading: false,
      responsiblePayerPerson: {
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
      },
    },
  });

  expect(getByText(/Aman Monga/)).toBeVisible();
});

  it('displays responsible payer organization', async () => {
  const apiManagement: ApiGen_Concepts_PropertyManagement = {
    ...getMockApiPropertyManagement(),
    responsiblePayerPersonId: null,
    responsiblePayerOrganizationId: 1000,
    managementPurposes: [getMockApiPropertyManagementPurpose()],
  };

  const { getByText } = await setup({
    props: {
      propertyManagement: apiManagement,
      isLoading: false,
      responsiblePayerOrganization: {
        ...getMockOrganization(),
        id: 1000,
        name: 'TEST COMPANY INC.',
      },
    },
  });

  expect(getByText(/TEST COMPANY INC./)).toBeVisible();
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
