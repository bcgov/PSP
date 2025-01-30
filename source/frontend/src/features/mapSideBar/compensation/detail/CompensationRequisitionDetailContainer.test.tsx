import { useGenerateH120 } from '@/features/mapSideBar/acquisition/common/GenerateForm/hooks/useGenerateH120';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import {
  getMockApiDefaultCompensation,
  getMockCompensationPropertiesReq,
} from '@/mocks/compensations.mock';
import { getMockOrganization } from '@/mocks/organization.mock';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import {
  act,
  getMockRepositoryObj,
  render,
  RenderOptions,
  waitForEffects,
} from '@/utils/test-utils';

import {
  CompensationRequisitionDetailContainer,
  CompensationRequisitionDetailContainerProps,
} from './CompensationRequisitionDetailContainer';
import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

vi.mock('@/hooks/pims-api/useApiContacts');
vi.mock('@/hooks/repositories/useRequisitionCompensationRepository');
vi.mock('@/features/mapSideBar/acquisition/common/GenerateForm/hooks/useGenerateH120');

const getPersonConceptFn = vi.fn();
const getOrganizationConceptFn = vi.fn();
const setEditMode = vi.fn();
const onGenerate = vi.fn();

const mockGetCompReqPropertiesApi = getMockRepositoryObj();
const mockGetCompReqPayeesApi = getMockRepositoryObj([]);

vi.mocked(useCompensationRequisitionRepository, { partial: true }).mockReturnValue({
  getCompensationRequisitionProperties: mockGetCompReqPropertiesApi,
  getCompensationRequisitionPayees: mockGetCompReqPayeesApi,
});

vi.mocked(useApiContacts, { partial: true }).mockReturnValue({
  getPersonConcept: getPersonConceptFn,
  getOrganizationConcept: getOrganizationConceptFn,
});

vi.mocked(useGenerateH120).mockReturnValue(onGenerate);

let viewProps: CompensationRequisitionDetailViewProps | undefined;

const TestView = (props: CompensationRequisitionDetailViewProps) => {
  viewProps = props;
  return <>Content Rendered</>;
};

describe('Compensation Detail View container', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<CompensationRequisitionDetailContainerProps>;
    } = {},
  ) => {
    const utils = render(
      <CompensationRequisitionDetailContainer
        View={TestView}
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

    // wait for useEffect to complete
    await waitForEffects();

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    viewProps = undefined;

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

  it('renders the underlying form', async () => {
    const { getByText } = await setup();

    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(viewProps.fileType).toBe(ApiGen_CodeTypes_FileTypes.Acquisition);
    expect(viewProps.clientConstant).toBe('034');
    expect(viewProps.compensationPayees).toEqual([]);
  });

  it('makes request to get the properties selected for the compensation requisition', async () => {
    await setup({ props: { compensation: getMockApiDefaultCompensation() } });

    expect(mockGetCompReqPropertiesApi.execute).toHaveBeenCalledWith(
      ApiGen_CodeTypes_FileTypes.Acquisition,
      1,
    );
  });

  it('makes request to get the payees for the compensation requisition', async () => {
    await setup({ props: { compensation: getMockApiDefaultCompensation() } });

    expect(mockGetCompReqPayeesApi.execute).toHaveBeenCalledWith(1);
  });

  it('calls onGenerate when generation button is clicked', async () => {
    await setup({ props: { compensation: getMockApiDefaultCompensation() } });

    await act(async () => viewProps.onGenerate(viewProps.fileType, viewProps.compensation));
    expect(onGenerate).toHaveBeenCalled();
  });
});
