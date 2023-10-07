import { Claims } from '@/constants';
import { PropertyEditForms } from '@/features/mapSideBar/property/PropertyViewSelector';
import { mockLookups } from '@/mocks/lookups.mock';
import {
  getMockApiPropertyManagement,
  getMockApiPropertyManagementPurpose,
} from '@/mocks/propertyManagement.mock';
import { Api_PropertyManagement } from '@/models/api/Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import {
  IPropertyManagementDetailViewProps,
  PropertyManagementDetailView,
} from './PropertyManagementDetailView';

jest.mock('@react-keycloak/web');

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const setEditManagementState = jest.fn();

describe('PropertyManagementDetailView component', () => {
  const setup = (
    renderOptions?: RenderOptions & { props?: Partial<IPropertyManagementDetailViewProps> },
  ) => {
    renderOptions = renderOptions ?? {};
    const utils = render(
      <PropertyManagementDetailView
        {...renderOptions.props}
        setEditManagementState={setEditManagementState}
        propertyManagement={
          renderOptions.props?.propertyManagement ?? getMockApiPropertyManagement()
        }
        isLoading={renderOptions.props?.isLoading ?? false}
      />,
      {
        ...renderOptions,
        store: storeState,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected when provided valid data object', () => {
    const apiManagement: Api_PropertyManagement = {
      ...getMockApiPropertyManagement(),
      managementPurposes: [getMockApiPropertyManagementPurpose()],
    };
    const { asFragment } = setup({ props: { propertyManagement: apiManagement } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', () => {
    const { getByTestId } = setup({ props: { isLoading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays existing values if they exist', () => {
    const apiManagement: Api_PropertyManagement = {
      ...getMockApiPropertyManagement(),
      managementPurposes: [getMockApiPropertyManagementPurpose()],
    };
    const { getByText } = setup({ props: { propertyManagement: apiManagement } });
    expect(getByText('BC Ferries')).toBeVisible();
  });

  it('does not throw an exception for an invalid data object', () => {
    const { getByText } = setup({ props: { propertyManagement: {} as Api_PropertyManagement } });
    expect(getByText(/property purpose/i)).toBeVisible();
  });

  it('does not render the edit button if the user does not have management edit permissions', () => {
    const { queryByTitle } = setup({ claims: [] });
    const editButton = queryByTitle('Edit property management information');
    expect(editButton).toBeNull();
  });

  it('renders the edit button if the user has management edit permissions', () => {
    const { getByTitle } = setup({ claims: [Claims.MANAGEMENT_EDIT] });
    const editButton = getByTitle('Edit property management information');
    expect(editButton).toBeVisible();
  });

  it('switches to Edit mode when edit button is clicked', async () => {
    const { getByTitle } = setup({ claims: [Claims.MANAGEMENT_EDIT] });
    const editButton = getByTitle('Edit property management information');
    await act(async () => userEvent.click(editButton));
    expect(setEditManagementState).toHaveBeenCalledWith({
      form: PropertyEditForms.UpdateManagementSummaryContainer,
      childId: null,
    });
  });
});
