import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import {
  mockAcquisitionFileResponse,
  mockApiAcquisitionFileTeamOrganization,
  mockApiAcquisitionFileTeamPerson,
} from '@/mocks/acquisitionFiles.mock';
import { getMockApiDefaultCompensation } from '@/mocks/compensations.mock';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { emptyApiInterestHolder } from '@/mocks/interestHolder.mock';
import { getEmptyOrganization, getMockOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { render, RenderOptions, waitForEffects } from '@/utils/test-utils';

import {
  CompensationRequisitionDetailContainer,
  CompensationRequisitionDetailContainerProps,
} from './CompensationRequisitionDetailContainer';
import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

jest.mock('@/hooks/pims-api/useApiContacts');
const getPersonConceptFn = jest.fn();
const getOrganizationConceptFn = jest.fn();
(useApiContacts as jest.Mock).mockImplementation(() => ({
  getPersonConcept: getPersonConceptFn,
  getOrganizationConcept: getOrganizationConceptFn,
}));

let viewProps: CompensationRequisitionDetailViewProps;

const CompensationViewComponent = (props: CompensationRequisitionDetailViewProps) => {
  viewProps = props;
  return <></>;
};

const setEditMode = jest.fn();

describe('Compensation Detail View container', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<CompensationRequisitionDetailContainerProps> },
  ) => {
    const utils = render(
      <CompensationRequisitionDetailContainer
        View={CompensationViewComponent}
        {...renderOptions.props}
        loading={renderOptions.props?.loading ?? false}
        setEditMode={setEditMode}
        compensation={renderOptions.props?.compensation ?? getMockApiDefaultCompensation()}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
        clientConstant={renderOptions.props?.clientConstant ?? '034'}
      />,
      {
        ...renderOptions,
        claims: renderOptions?.claims ?? [],
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    getPersonConceptFn.mockResolvedValue({
      data: {
        id: 1,
        firstName: 'first',
        surname: 'last',
      } as ApiGen_Concepts_Person,
    });
    getOrganizationConceptFn.mockResolvedValue({
      data: getMockOrganization(),
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('makes request to get person concept for acquisition team payee', async () => {
    const teamPerson = mockApiAcquisitionFileTeamPerson();
    setup({
      props: {
        compensation: {
          ...getMockApiDefaultCompensation(),
          acquisitionFileTeamId: teamPerson.id ?? 1,
          acquisitionFileTeam: teamPerson,
        },
      },
    });

    await waitForEffects();
    expect(getPersonConceptFn).toHaveBeenCalled();
    expect(viewProps.compensationContactPerson).toStrictEqual({
      id: 1,
      firstName: 'first',
      surname: 'last',
    } as ApiGen_Concepts_Person);
  });

  it('makes request to get organization concept for acquisition team payee', async () => {
    const teamOrg = mockApiAcquisitionFileTeamOrganization();
    setup({
      props: {
        compensation: {
          ...getMockApiDefaultCompensation(),
          acquisitionFileTeamId: teamOrg.id ?? 1,
          acquisitionFileTeam: teamOrg,
        },
      },
    });

    await waitForEffects();
    expect(getOrganizationConceptFn).toHaveBeenCalled();
    expect(viewProps.compensationContactOrganization).toStrictEqual(getMockOrganization());
  });

  it('makes request to get person concept for interest holder payee', async () => {
    const ihPerson: ApiGen_Concepts_InterestHolder = {
      ...emptyApiInterestHolder,
      interestHolderId: 1,
      acquisitionFileId: 2,
      personId: 1,
      person: { ...getEmptyPerson(), id: 1, firstName: 'first', surname: 'last' },
    };
    setup({
      props: {
        compensation: {
          ...getMockApiDefaultCompensation(),
          interestHolderId: ihPerson.interestHolderId,
          interestHolder: ihPerson,
        },
      },
    });

    await waitForEffects();
    expect(getPersonConceptFn).toHaveBeenCalled();
    expect(viewProps.compensationContactPerson).toStrictEqual({
      id: 1,
      firstName: 'first',
      surname: 'last',
    } as ApiGen_Concepts_Person);
  });

  it('makes request to get organization concept for interest holder payee', async () => {
    const ihOrg: ApiGen_Concepts_InterestHolder = {
      ...emptyApiInterestHolder,
      interestHolderId: 1,
      acquisitionFileId: 2,
      organizationId: 100,
      organization: { ...getEmptyOrganization(), id: 100, name: 'ABC Inc' },
    };

    setup({
      props: {
        compensation: {
          ...getMockApiDefaultCompensation(),
          interestHolderId: ihOrg.interestHolderId,
          interestHolder: ihOrg,
        },
      },
    });

    await waitForEffects();
    expect(getOrganizationConceptFn).toHaveBeenCalled();
    expect(viewProps.compensationContactOrganization).toStrictEqual(getMockOrganization());
  });
});
