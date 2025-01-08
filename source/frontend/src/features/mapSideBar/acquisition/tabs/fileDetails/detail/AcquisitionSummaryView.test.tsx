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
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { formatMinistryProject, toTypeCodeNullable } from '@/utils/formUtils';
import { act, cleanup, render, RenderOptions, userEvent, waitForEffects } from '@/utils/test-utils';

import AcquisitionSummaryView, { IAcquisitionSummaryViewProps } from './AcquisitionSummaryView';
import { mockProjects } from '@/mocks/projects.mock';

// mock auth library

const onEdit = vi.fn();

vi.mock('@/hooks/pims-api/useApiContacts');
const getPersonConceptFn = vi.fn();
vi.mocked(useApiContacts).mockImplementation(
  () =>
    ({
      getPersonConcept: getPersonConceptFn,
    } as unknown as ReturnType<typeof useApiContacts>),
);

vi.mock('@/hooks/pims-api/useApiAcquisitionFile');
const getAcquisitionFileOwnersFn = vi.fn();
vi.mocked(useApiAcquisitionFile).mockImplementation(
  () =>
    ({
      getAcquisitionFileOwners: getAcquisitionFileOwnersFn,
    } as unknown as ReturnType<typeof useApiAcquisitionFile>),
);

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
    vi.clearAllMocks();
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
      { acquisitionFile: mockAcquisitionFileResponse() },
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

  it.each([
    ['with project number', '001', `FTProjectTest`],
    ['without project number', null, `FTProjectTest`],
  ])(
    'renders the file Project Number and name concatenated - %s',
    async (_: string, projectNumber: string | null, projectDescription: string) => {
      const { getByText } = setup(
        {
          acquisitionFile: {
            ...mockAcquisitionFileResponse(),
            project: {
              id: 1,
              projectStatusTypeCode: null,
              code: projectNumber,
              description: projectDescription,
              costTypeCode: null,
              businessFunctionCode: null,
              workActivityCode: null,
              regionCode: null,
              note: null,
              projectPersons: [],
              projectProducts: [],
              ...getEmptyBaseAudit(1),
            },
          },
        },
        { claims: [] },
      );
      await waitForEffects();

      expect(getByText('Ministry project:')).toBeVisible();
      expect(getByText(formatMinistryProject(projectNumber, projectDescription))).toBeVisible();
    },
  );

  it('renders historical file number', async () => {
    const { getByText } = setup({ acquisitionFile: mockAcquisitionFileResponse() }, { claims: [] });
    await waitForEffects();
    expect(getByText('legacy file number')).toBeVisible();
  });

  it('renders the file progress statuses', async () => {
    const { getByTestId } = setup(
      {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
          estimatedCompletionDate: '2030-01-10T00:00:00',
          possessionDate: '2035-03-10T00:00:00',
          acquisitionFileProgressStatuses: [
            {
              id: 3,
              acquisitionFileId: 64,
              progressStatusTypeCode: {
                id: 'OWNRCNTCTD',
                description: 'Owner contacted',
                isDisabled: false,
                displayOrder: 6,
              },
              appCreateTimestamp: '2024-12-27T17:33:21.553',
              appLastUpdateTimestamp: '2024-12-27T17:33:21.553',
              appLastUpdateUserid: 'EHERRERA',
              appCreateUserid: 'EHERRERA',
              appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              rowVersion: 1,
            },
            {
              id: 4,
              acquisitionFileId: 64,
              progressStatusTypeCode: {
                id: 'WTG4PAPLN',
                description: 'Waiting for PA plan',
                isDisabled: false,
                displayOrder: 7,
              },
              appCreateTimestamp: '2024-12-27T17:33:21.553',
              appLastUpdateTimestamp: '2024-12-27T17:33:21.553',
              appLastUpdateUserid: 'EHERRERA',
              appCreateUserid: 'EHERRERA',
              appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              rowVersion: 1,
            },
          ],
          acquisitionFileAppraisalStatusTypeCode: {
            id: 'RECEIVED',
            description: 'Received',
            isDisabled: false,
            displayOrder: 3,
          },
          acquisitionFileLegalSurveyStatusTypeCode: {
            id: 'OUT4SIGN',
            description: 'Out for Signatures',
            isDisabled: false,
            displayOrder: 4,
          },
          acquisitionFileTakingStatuses: [
            {
              id: 4,
              acquisitionFileId: 64,
              takingStatusTypeCode: {
                id: 'PARTLTACQ',
                description: 'Partial Acquisition',
                isDisabled: false,
                displayOrder: 3,
              },
              appCreateTimestamp: '2024-12-27T17:33:21.553',
              appLastUpdateTimestamp: '2024-12-27T17:33:21.553',
              appLastUpdateUserid: 'EHERRERA',
              appCreateUserid: 'EHERRERA',
              appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              rowVersion: 1,
            },
          ],
          acquisitionFileExpropiationRiskStatusTypeCode: {
            id: 'MED',
            description: 'Medium',
            isDisabled: false,
            displayOrder: 3,
          },
        },
      },
      { claims: [] },
    );
    await waitForEffects();

    expect(getByTestId('prg-file-progress-status')).toHaveTextContent('Owner contacted');
    expect(getByTestId('prg-file-progress-status')).toHaveTextContent('Waiting for PA plan');
    expect(getByTestId('prg-appraisal-status')).toHaveTextContent('Received');
    expect(getByTestId('prg-legal-survey-status')).toHaveTextContent('Out for Signatures');
    expect(getByTestId('prg-taking-type-status')).toHaveTextContent('Partial Acquisition');
    expect(getByTestId('prg-expropiation-risk-status')).toHaveTextContent('Medium');
  });

  it('renders acquisition-related dates', async () => {
    const { getByText, getByTestId } = setup(
      {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
          estimatedCompletionDate: '2030-01-10T00:00:00',
          possessionDate: '2035-03-10T00:00:00',
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(getByText('Jan 10, 2030')).toBeVisible();
    expect(getByText('Mar 10, 2035')).toBeVisible();
    expect(getByTestId('assigned-date')).toHaveTextContent('Dec 18, 2024');
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
    const { findByText } = setup(
      {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
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
    const { findByText } = setup(
      {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
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
    const { findByText } = setup(
      {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
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

  describe('Sub-interest files', () => {
    it('renders sub file interest type', async () => {
      const { getByTestId } = setup(
        {
          acquisitionFile: {
            ...mockAcquisitionFileResponse(),
            parentAcquisitionFileId: 100,
            subfileInterestTypeCode: {
              id: 'SUBTENANT',
              description: 'Sub-Tenant / Lease',
              isDisabled: false,
              displayOrder: 8,
            },
            otherSubfileInterestType: null,
          },
        },
        { claims: [] },
      );
      await waitForEffects();

      expect(getByTestId('subFile-interest-type')).toHaveTextContent('Sub-Tenant / Lease');
    });

    it('renders OTHER sub file interest type', async () => {
      const { getByTestId } = setup(
        {
          acquisitionFile: {
            ...mockAcquisitionFileResponse(),
            parentAcquisitionFileId: 100,
            subfileInterestTypeCode: {
              id: 'OTHER',
              description: 'Other',
              isDisabled: false,
              displayOrder: 8,
            },
            otherSubfileInterestType: 'SOME OTHER VALUE',
          },
        },
        { claims: [] },
      );
      await waitForEffects();

      expect(getByTestId('subFile-interest-type')).toHaveTextContent('Other-SOME OTHER VALUE');
    });

    it('renders sub-interest information section', async () => {
      const { getByText } = setup(
        {
          acquisitionFile: {
            ...mockAcquisitionFileResponse(),
            parentAcquisitionFileId: 100,
            subfileInterestTypeCode: {
              id: 'SUBTENANT',
              description: 'Sub-Tenant / Lease',
              isDisabled: false,
              displayOrder: 8,
            },
            otherSubfileInterestType: null,
          },
        },
        { claims: [] },
      );
      await waitForEffects();

      expect(getByText('Sub-Interest Information')).toBeVisible();
      expect(
        getByText(
          'Each property in this sub-file should be impacted by the sub-interest(s) in this section',
        ),
      ).toBeVisible();
      expect(getByText(/Sub-interest solicitor/i)).toBeVisible();
      expect(getByText(/Sub-interest representative/i)).toBeVisible();
    });
  });

  describe('Project persons', () => {
    it('renders project persons if present', async () => {
      const { findByText } = setup(
        {
          acquisitionFile: {
            ...mockAcquisitionFileResponse(),
            project: {
              ...mockAcquisitionFileResponse().project,
              projectPersons: [
                {
                  id: 1,
                  personId: 1,
                  person: {
                    ...getEmptyPerson(),
                    id: 1,
                    surname: 'Doe',
                    firstName: 'John',
                    middleNames: 'A',
                  },
                  project: mockProjects()[0],
                  projectId: 1,
                  ...getEmptyBaseAudit(),
                },
              ],
            },
          },
        },
        { claims: [] },
      );
      await waitForEffects();
      expect(await findByText(/Management team member/)).toBeVisible();
      expect(await findByText(/John A Doe/)).toBeVisible();
    });

    it('renders multiple project persons if present', async () => {
      const { findByText, findAllByText } = setup(
        {
          acquisitionFile: {
            ...mockAcquisitionFileResponse(),
            project: {
              ...mockAcquisitionFileResponse().project,
              projectPersons: [
                {
                  id: 1,
                  personId: 1,
                  person: {
                    ...getEmptyPerson(),
                    id: 1,
                    surname: 'Doe',
                    firstName: 'John',
                    middleNames: 'A',
                  },
                  project: mockProjects()[0],
                  projectId: 1,
                  ...getEmptyBaseAudit(),
                },
                {
                  id: 2,
                  personId: 2,
                  person: {
                    ...getEmptyPerson(),
                    id: 2,
                    surname: 'Smith',
                    firstName: 'Jane',
                    middleNames: 'B',
                  },
                  project: mockProjects()[0],
                  projectId: 1,
                  ...getEmptyBaseAudit(),
                },
              ],
            },
          },
        },
        { claims: [] },
      );
      await waitForEffects();
      expect((await findAllByText(/Management team member/))[0]).toBeVisible();
      expect(await findByText(/John A Doe/)).toBeVisible();
      expect(await findByText(/Jane B Smith/)).toBeVisible();
    });
  });
});
