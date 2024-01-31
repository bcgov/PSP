import Claims from '@/constants/claims';
import { useApiAcquisitionFile } from '@/hooks/pims-api/useApiAcquisitionFile';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { act, cleanup, render, RenderOptions, userEvent, waitForEffects } from '@/utils/test-utils';

import AcquisitionSummaryView, { IAcquisitionSummaryViewProps } from './AcquisitionSummaryView';

// mock auth library
jest.mock('@react-keycloak/web');

const onEdit = jest.fn();

jest.mock('@/hooks/pims-api/useApiContacts');
const getPersonConceptFn = jest.fn();
(useApiContacts as jest.Mock).mockImplementation(() => ({
  getPersonConcept: getPersonConceptFn,
}));

jest.mock('@/hooks/pims-api/useApiAcquisitionFile');
const getAcquisitionFileOwnersFn = jest.fn();
(useApiAcquisitionFile as jest.Mock).mockImplementation(() => ({
  getAcquisitionFileOwners: getAcquisitionFileOwnersFn,
}));

describe('AcquisitionSummaryView component', () => {
  // render component under test
  const setup = (
    props: Partial<IAcquisitionSummaryViewProps>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <AcquisitionSummaryView acquisitionFile={props.acquisitionFile} onEdit={onEdit} />,
      {
        useMockAuthentication: true,
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {
    getPersonConceptFn.mockResolvedValue({
      data: {
        id: 100,
        firstName: 'Foo',
        middleNames: 'Bar',
        surname: 'Baz',
      } as ApiGen_Concepts_Person,
    });
    getAcquisitionFileOwnersFn.mockResolvedValue({
      data: mockAcquisitionFileOwnersResponse(1),
    });
  });

  afterEach(() => {
    cleanup();
    jest.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({ acquisitionFile: mockAcquisitionFileResponse() });
    await waitForEffects();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the edit button for users with acquisition edit permissions', async () => {
    const { getByTitle } = setup(
      { acquisitionFile: mockAcquisitionFileResponse() },
      { claims: [Claims.ACQUISITION_EDIT] },
    );
    await waitForEffects();
    const editButton = getByTitle('Edit acquisition file');
    expect(editButton).toBeVisible();
    await act(async () => userEvent.click(editButton));
    expect(onEdit).toHaveBeenCalled();
  });

  it('does not render the edit button for users that do not have acquisition edit permissions', async () => {
    const { queryByTitle } = setup(
      {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    const editButton = queryByTitle('Edit acquisition file');
    expect(editButton).toBeNull();
  });

  it('does not render the edit button for non-admin users when the file is finalized', async () => {
    const { queryByTitle, getByTestId } = setup(
      {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
          fileStatusTypeCode: toTypeCodeNullable('COMPLETE'),
        },
      },
      { claims: [Claims.ACQUISITION_EDIT] },
    );
    await waitForEffects();
    const editButton = queryByTitle('Edit acquisition file');
    expect(editButton).toBeNull();
    const editWarningText = getByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(editWarningText).toBeVisible();
  });

  it('renders historical file number', async () => {
    const mockResponse = mockAcquisitionFileResponse();
    const { getByText } = setup({ acquisitionFile: mockResponse }, { claims: [] });
    await waitForEffects();
    expect(getByText('legacy file number')).toBeVisible();
  });

  it('renders owner solicitor information with primary contact', async () => {
    const { findByText } = setup(
      { acquisitionFile: mockAcquisitionFileResponse() },
      { claims: [] },
    );
    expect(await findByText('Millennium Inc')).toBeVisible();
    expect(await findByText(/Primary contact/)).toBeVisible();
    expect(await findByText('Foo Bar Baz')).toBeVisible();
  });

  it('renders owner representative information', async () => {
    const { getByText } = setup({ acquisitionFile: mockAcquisitionFileResponse() }, { claims: [] });
    await waitForEffects();
    expect(getByText('Han Solo')).toBeVisible();
    expect(getByText('test representative comment')).toBeVisible();
  });

  it('renders acquisition team member person', async () => {
    const apiMock = mockAcquisitionFileResponse();
    const { findByText } = setup(
      {
        acquisitionFile: {
          ...apiMock,
          acquisitionTeam: [
            {
              id: 1,
              acquisitionFileId: 1,
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
    expect(await findByText(/Bob Billy Smith/)).toBeVisible();
  });

  it('renders acquisition team member organization', async () => {
    const apiMock = mockAcquisitionFileResponse();
    const { findByText } = setup(
      {
        acquisitionFile: {
          ...apiMock,
          acquisitionTeam: [
            {
              id: 1,
              acquisitionFileId: 1,
              organizationId: 1,
              organization: {
                ...getEmptyOrganization(),
                id: 1,
                name: 'Test Organization',
                alias: 'ABC Inc',
                incorporationNumber: '1234',
                comment: '',
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

  it('renders acquisition team member organization and primary contact', async () => {
    const apiMock = mockAcquisitionFileResponse();
    const { findByText } = setup(
      {
        acquisitionFile: {
          ...apiMock,
          acquisitionFileInterestHolders: [],
          acquisitionTeam: [
            {
              id: 1,
              acquisitionFileId: 1,
              organizationId: 1,
              organization: {
                ...getEmptyOrganization(),
                id: 1,
                name: 'Test Organization',
                alias: 'ABC Inc',
                incorporationNumber: '1234',
                comment: '',
              },
              primaryContactId: 1,
              primaryContact: {
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
});
