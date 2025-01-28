import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import {
  mockAcquisitionFileResponse,
  mockApiAcquisitionFileTeamOrganization,
  mockApiAcquisitionFileTeamPerson,
} from '@/mocks/acquisitionFiles.mock';
import {
  getMockApiDefaultCompensation,
  getMockCompensationPropertiesReq,
} from '@/mocks/compensations.mock';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { emptyApiInterestHolder } from '@/mocks/interestHolder.mock';
import { getEmptyOrganization, getMockOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { getMockRepositoryObj, render, RenderOptions, waitForEffects } from '@/utils/test-utils';

import {
  CompensationRequisitionDetailContainer,
  CompensationRequisitionDetailContainerProps,
} from './CompensationRequisitionDetailContainer';
import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';

vi.mock('@/hooks/pims-api/useApiContacts');
vi.mock('@/hooks/repositories/useRequisitionCompensationRepository');

const getPersonConceptFn = vi.fn();
const getOrganizationConceptFn = vi.fn();
const setEditMode = vi.fn();

const mockGetCompReqPropertiesApi = getMockRepositoryObj();
const mockPutCompReqPropertiesApi = getMockRepositoryObj();

vi.mock('@/hooks/repositories/useRequisitionCompensationRepository', () => ({
  useCompensationRequisitionRepository: () => {
    return {
      getCompensationRequisitionProperties: mockGetCompReqPropertiesApi,
      getCompensationRequisitionPayees: mockGetCompReqPropertiesApi,
    };
  },
}));

vi.mocked(useApiContacts).mockImplementation(
  () =>
    ({
      getPersonConcept: getPersonConceptFn,
      getOrganizationConcept: getOrganizationConceptFn,
    } as unknown as ReturnType<typeof useApiContacts>),
);

let viewProps: CompensationRequisitionDetailViewProps;
const CompensationViewComponent = (props: CompensationRequisitionDetailViewProps) => {
  viewProps = props;
  return <></>;
};

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
        file={renderOptions.props?.file ?? mockAcquisitionFileResponse()}
        fileType={renderOptions.props?.fileType ?? ApiGen_CodeTypes_FileTypes.Acquisition}
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

    mockGetCompReqPropertiesApi.execute.mockResolvedValue(getMockCompensationPropertiesReq());
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('makes request to get the properties selected for the compensation requisition', async () => {
    const teamPerson = mockApiAcquisitionFileTeamPerson();
    setup({
      props: {
        compensation: {
          ...getMockApiDefaultCompensation(),
        },
      },
    });

    await waitForEffects();

    expect(mockGetCompReqPropertiesApi.execute).toHaveBeenCalled();
  });
});
