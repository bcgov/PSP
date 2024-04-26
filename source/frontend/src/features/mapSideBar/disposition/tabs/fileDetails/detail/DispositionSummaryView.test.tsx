import Claims from '@/constants/claims';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { toTypeCode, toTypeCodeNullable } from '@/utils/formUtils';
import { DispositionFileStatus } from '@/constants/dispositionFileStatus';
import Roles from '@/constants/roles';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { act, cleanup, render, RenderOptions, userEvent, waitForEffects } from '@/utils/test-utils';

import DispositionSummaryView, { IDispositionSummaryViewProps } from './DispositionSummaryView';

// mock auth library

const onEdit = vi.fn();

const mockDispositionFileApi = mockDispositionFileResponse();

describe('DispositionSummaryView component', () => {
  // render component under test
  const setup = (
    props: Partial<IDispositionSummaryViewProps>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <DispositionSummaryView dispositionFile={props.dispositionFile} onEdit={onEdit} />,
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
      dispositionFile: mockDispositionFileApi,
    });
    await waitForEffects();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the edit button for users with disposition edit permissions', async () => {
    const { getByTitle, queryByTestId } = setup(
      { dispositionFile: mockDispositionFileApi },
      { claims: [Claims.DISPOSITION_EDIT] },
    );
    await waitForEffects();
    const editButton = getByTitle('Edit disposition file');
    expect(editButton).toBeVisible();
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(icon).toBeNull();
    await act(async () => userEvent.click(editButton));
    expect(onEdit).toHaveBeenCalled();
  });

  it('does not render the edit button for users that do not have disposition edit permissions', async () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        dispositionFile: mockDispositionFileResponse(),
      },
      { claims: [] },
    );
    await waitForEffects();
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    const editButton = queryByTitle('Edit disposition file');
    expect(editButton).toBeNull();
    expect(icon).toBeNull();
  });

  it('renders the warning icon for disposition files in non-editable status', async () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        dispositionFile: {
          ...mockDispositionFileResponse(),
          fileStatusTypeCode: toTypeCode(DispositionFileStatus.Complete),
        },
      },
      { claims: [Claims.DISPOSITION_EDIT] },
    );
    await waitForEffects();
    const editButton = queryByTitle('Edit disposition file');
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(editButton).toBeNull();
    expect(icon).toBeVisible();
  });

  it('it does not render the warning icon for disposition files in non-editable status for Admins', async () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        dispositionFile: {
          ...mockDispositionFileResponse(),
          fileStatusTypeCode: toTypeCode(DispositionFileStatus.Complete),
        },
      },
      { claims: [Claims.DISPOSITION_EDIT], roles: [Roles.SYSTEM_ADMINISTRATOR] },
    );
    await waitForEffects();
    const editButton = queryByTitle('Edit disposition file');
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(editButton).toBeVisible();
    expect(icon).toBeNull();
  });

  it('renders historical file number', async () => {
    const mockResponse = mockDispositionFileApi;
    const { getByText } = setup({ dispositionFile: mockResponse }, { claims: [] });
    await waitForEffects();
    expect(getByText('FILE_REFERENCE 8128827 3EAD56A')).toBeVisible();
  });

  it('renders other initiating document field when iniating document is OTHER', async () => {
    const { getByText } = setup(
      {
        dispositionFile: {
          ...mockDispositionFileResponse(),
          initiatingDocumentTypeCode: toTypeCodeNullable('OTHER'),
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(getByText(/Other \(initiating document\)/i, { exact: false })).toBeVisible();
  });

  it('renders other other disposition type field when disposition type is OTHER', async () => {
    const { getByText } = setup(
      {
        dispositionFile: {
          ...mockDispositionFileResponse(),
          dispositionTypeCode: toTypeCodeNullable('OTHER'),
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(getByText(/Other \(disposition type\)/i, { exact: false })).toBeVisible();
  });

  it('renders disposition team member person', async () => {
    const apiMock = mockDispositionFileResponse();
    const { findByText } = setup(
      {
        dispositionFile: {
          ...apiMock,
          dispositionTeam: [
            {
              id: 1,
              dispositionFileId: 1,
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

  it('renders disposition team member organization', async () => {
    const apiMock = mockDispositionFileApi;
    const { findByText } = setup(
      {
        dispositionFile: {
          ...apiMock,
          dispositionTeam: [
            {
              id: 1,
              dispositionFileId: 1,
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

  it('renders disposition team member organization and primary contact', async () => {
    const apiMock = mockDispositionFileApi;
    const { findByText } = setup(
      {
        dispositionFile: {
          ...apiMock,
          dispositionTeam: [
            {
              id: 1,
              dispositionFileId: 1,
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
      dispositionFile: mockDispositionFileApi,
    });

    await waitForEffects();
    expect(queryByTestId('dsp-project')).toHaveTextContent('00048 - CLAIMS');
    expect(queryByTestId('dsp-product')).toHaveTextContent('00055 AVALANCHE & PROGRAM REVIEW');
  });
});
