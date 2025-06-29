import Claims from '@/constants/claims';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { toTypeCode } from '@/utils/formUtils';
import Roles from '@/constants/roles';
import { act, cleanup, render, RenderOptions, userEvent, waitForEffects } from '@/utils/test-utils';

import { ApiGen_CodeTypes_ManagementFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_ManagementFileStatusTypes';
import ManagementSummaryView, { IManagementSummaryViewProps } from './ManagementSummaryView';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';

// mock auth library

const onEdit = vi.fn();

const mockManagementFileApi = mockManagementFileResponse();

describe('ManagementSummaryView component', () => {
  // render component under test
  const setup = (
    props: Partial<IManagementSummaryViewProps>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <ManagementSummaryView managementFile={props.managementFile} onEdit={onEdit} />,
      {
        useMockAuthentication: true,
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {});

  afterEach(() => {
    cleanup();
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({
      managementFile: mockManagementFileApi,
    });
    await waitForEffects();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the edit button for users with management edit permissions', async () => {
    const { getByTitle, queryByTestId } = setup(
      { managementFile: mockManagementFileApi },
      { claims: [Claims.MANAGEMENT_EDIT] },
    );
    await waitForEffects();
    const editButton = getByTitle('Edit management file');
    expect(editButton).toBeVisible();
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(icon).toBeNull();
    await act(async () => userEvent.click(editButton));
    expect(onEdit).toHaveBeenCalled();
  });

  it('does not render the edit button for users that do not have management edit permissions', async () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        managementFile: mockManagementFileResponse(),
      },
      { claims: [] },
    );
    await waitForEffects();
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    const editButton = queryByTitle('Edit management file');
    expect(editButton).toBeNull();
    expect(icon).toBeNull();
  });

  it.each([
    ['Management File Status is "Completed"', ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE],
    ['Management File Status is "Archived"', ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED],
  ])(
    'renders the warning icon for management files in non-editable status - %s',
    async (_: string, fileStatus: ApiGen_CodeTypes_ManagementFileStatusTypes) => {
      const { queryByTitle, queryByTestId } = setup(
        {
          managementFile: {
            ...mockManagementFileResponse(),
            fileStatusTypeCode: toTypeCode(fileStatus),
          },
        },
        { claims: [Claims.MANAGEMENT_EDIT] },
      );
      await waitForEffects();
      const editButton = queryByTitle('Edit management file');
      const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
      expect(editButton).toBeNull();
      expect(icon).toBeVisible();
    },
  );

  it('it does not render the warning icon for management files in non-editable status for Admins', async () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        managementFile: {
          ...mockManagementFileResponse(),
          fileStatusTypeCode: toTypeCode(ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE),
        },
      },
      { claims: [Claims.MANAGEMENT_EDIT], roles: [Roles.SYSTEM_ADMINISTRATOR] },
    );
    await waitForEffects();
    const editButton = queryByTitle('Edit management file');
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(editButton).toBeVisible();
    expect(icon).toBeNull();
  });

  it('renders management team member person', async () => {
    const apiMock = mockManagementFileResponse();
    const { findByText } = setup(
      {
        managementFile: {
          ...apiMock,
          managementTeam: [
            {
              id: 1,
              managementFileId: 1,
              personId: 1,
              person: {
                ...getEmptyPerson(),
                id: 1,
                surname: 'Smith',
                firstName: 'Bob',
                middleNames: 'Billy',
                personOrganizations: [],
                personAddresses: [],
                contactMethods: [],
                rowVersion: 2,
              },
              teamProfileType: {
                id: 'NEGOTAGENT',
                description: 'Negotiation agent',
                isDisabled: false,
                displayOrder: null,
              },
              rowVersion: 2,
              organization: null,
              organizationId: null,
              primaryContact: null,
              primaryContactId: null,
              teamProfileTypeCode: null,
            },
          ],
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(await findByText(/Negotiation agent/)).toBeVisible();
    expect(await findByText(/Bob Billy Smith/)).toBeVisible();
  });

  it('renders management team member organization', async () => {
    const apiMock = mockManagementFileApi;
    const { findByText } = setup(
      {
        managementFile: {
          ...apiMock,
          managementTeam: [
            {
              id: 1,
              managementFileId: 1,
              organizationId: 1,
              organization: {
                ...getEmptyOrganization(),
                id: 1,
                name: 'Test Organization',
                alias: 'ABC Inc',
                incorporationNumber: '1234',
                comment: '',
                contactMethods: null,
                isDisabled: false,
                organizationAddresses: null,
                organizationPersons: null,
                rowVersion: null,
              },
              teamProfileType: {
                id: 'NEGOTAGENT',
                description: 'Negotiation agent',
                isDisabled: false,
                displayOrder: null,
              },
              rowVersion: 2,
              person: null,
              personId: null,
              primaryContact: null,
              primaryContactId: null,
              teamProfileTypeCode: null,
            },
          ],
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(await findByText(/Negotiation agent/)).toBeVisible();
    expect(await findByText(/Test Organization/)).toBeVisible();
    expect(await findByText(/No contacts available/)).toBeVisible();
  });

  it('renders management team member organization and primary contact', async () => {
    const apiMock = mockManagementFileApi;
    const { findByText } = setup(
      {
        managementFile: {
          ...apiMock,
          managementTeam: [
            {
              id: 1,
              managementFileId: 1,
              organizationId: 1,
              organization: {
                ...getEmptyOrganization(),
                id: 1,
                name: 'Test Organization',
                alias: 'ABC Inc',
                incorporationNumber: '1234',
                comment: '',
                contactMethods: null,
                isDisabled: false,
                organizationAddresses: null,
                organizationPersons: null,
                rowVersion: null,
              },
              primaryContactId: 1,
              primaryContact: {
                ...getEmptyPerson(),
                id: 1,
                surname: 'Smith',
                firstName: 'Bob',
                middleNames: 'Billy',
                personOrganizations: [],
                personAddresses: [],
                contactMethods: [],
                rowVersion: 2,
                comment: null,
                isDisabled: false,
                preferredName: null,
              },
              teamProfileType: {
                id: 'NEGOTAGENT',
                description: 'Negotiation agent',
                isDisabled: false,
                displayOrder: null,
              },
              rowVersion: 2,
              person: null,
              personId: null,
              teamProfileTypeCode: null,
            },
          ],
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(await findByText(/Negotiation agent/)).toBeVisible();
    expect(await findByText(/Test Organization/)).toBeVisible();
    expect(await findByText(/Primary contact/)).toBeVisible();
    expect(await findByText(/Bob Billy Smith/)).toBeVisible();
  });

  it('renders the project and product', async () => {
    const { queryByTestId } = setup({
      managementFile: mockManagementFileApi,
    });

    await waitForEffects();
    expect(queryByTestId('management-project')).toHaveTextContent('00048 - CLAIMS');
    expect(queryByTestId('management-product')).toHaveTextContent(
      '00055 AVALANCHE & PROGRAM REVIEW',
    );
  });
});
