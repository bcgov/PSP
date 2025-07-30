import moment from 'moment';

import { InterestHolderType } from '@/constants/interestHolderTypes';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import {
  getMockApiAcquisitionFileOwnerPerson,
  mockAcquisitionFileResponse,
  mockApiAcquisitionFileTeamPerson,
} from '@/mocks/acquisitionFiles.mock';
import { getMockApiInterestHolderPerson } from '@/mocks/interestHolders.mock';
import { getMockApiLease, getMockLeaseStakeholders } from '@/mocks/lease.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants/systemConstantsSlice';
import { act, getMockRepositoryObj, render, RenderOptions } from '@/utils/test-utils';

import { CompensationRequisitionFormModel } from '../models/CompensationRequisitionFormModel';
import UpdateCompensationRequisitionContainer, {
  UpdateCompensationRequisitionContainerProps,
} from './UpdateCompensationRequisitionContainer';
import { CompensationRequisitionFormProps } from './UpdateCompensationRequisitionForm';
import { PayeeOption } from '../../acquisition/models/PayeeOptionModel';
import { getMockApiDefaultCompensation, getMockCompReqAcqPayee } from '@/mocks/compensations.mock';
import { ApiGen_Concepts_CompReqLeasePayee } from '@/models/api/generated/ApiGen_Concepts_CompReqLeasePayee';

vi.mock('@/hooks/repositories/useAcquisitionProvider');
vi.mock('@/hooks/repositories/useFinancialCodeRepository');
vi.mock('@/hooks/repositories/useLeaseStakeholderRepository');
vi.mock('@/hooks/repositories/useInterestHolderRepository');
vi.mock('@/hooks/repositories/useRequisitionCompensationRepository');

const getAcquisitionOwnersApi = getMockRepositoryObj();

vi.mocked(useAcquisitionProvider, { partial: true }).mockReturnValue({
  getAcquisitionOwners: getAcquisitionOwnersApi,
});

const mockGetFinancialCodesApi = getMockRepositoryObj();

vi.mocked(useFinancialCodeRepository, { partial: true }).mockReturnValue({
  getFinancialActivityCodeTypes: mockGetFinancialCodesApi,
  getChartOfAccountsCodeTypes: mockGetFinancialCodesApi,
  getResponsibilityCodeTypes: mockGetFinancialCodesApi,
  getYearlyFinancialsCodeTypes: mockGetFinancialCodesApi,
});

const getLeaseFileInterestHoldersApi = getMockRepositoryObj();

vi.mocked(useLeaseStakeholderRepository, { partial: true }).mockReturnValue({
  getLeaseStakeholders: getLeaseFileInterestHoldersApi,
});

const getAcquisitionInterestHoldersApi = getMockRepositoryObj();

vi.mocked(useInterestHolderRepository, { partial: true }).mockReturnValue({
  getAcquisitionInterestHolders: getAcquisitionInterestHoldersApi,
});

const putCompensationRequisitionApi = getMockRepositoryObj();
const getCompensationRequisitionAcqPayeesApi = getMockRepositoryObj();
const getCompensationRequisitionLeasePayeesApi = getMockRepositoryObj();

vi.mocked(useCompensationRequisitionRepository, { partial: true }).mockReturnValue({
  updateCompensationRequisition: putCompensationRequisitionApi,
  getCompensationRequisitionAcqPayees: getCompensationRequisitionAcqPayeesApi,
  getCompensationRequisitionLeasePayees: getCompensationRequisitionLeasePayeesApi,
});

let viewProps: CompensationRequisitionFormProps | undefined;

const TestView: React.FC<CompensationRequisitionFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const onSuccess = vi.fn();
const onCancel = vi.fn();

describe('UpdateCompensationRequisition Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<UpdateCompensationRequisitionContainerProps>;
    } = {},
  ) => {
    const utils = render(
      <UpdateCompensationRequisitionContainer
        compensation={renderOptions?.props?.compensation ?? getMockApiDefaultCompensation()}
        fileType={renderOptions?.props?.fileType ?? ApiGen_CodeTypes_FileTypes.Acquisition}
        file={renderOptions?.props?.file ?? mockAcquisitionFileResponse(1)}
        onSuccess={renderOptions?.props?.onSuccess ?? onSuccess}
        onCancel={renderOptions?.props?.onCancel ?? onCancel}
        View={TestView}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
          [systemConstantsSlice.name]: { systemConstants: [{ name: 'GST', value: '5.0' }] },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

    // wait for useEffect
    await act(async () => {});

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    // owners
    getAcquisitionOwnersApi.execute.mockResolvedValue([getMockApiAcquisitionFileOwnerPerson()]);
    // interest holders
    const ownerSolicitor = getMockApiInterestHolderPerson();
    ownerSolicitor.interestHolderType.id = InterestHolderType.OWNER_SOLICITOR;
    const ownerRep = getMockApiInterestHolderPerson();
    ownerRep.interestHolderType.id = InterestHolderType.OWNER_REPRESENTATIVE;
    getAcquisitionInterestHoldersApi.execute.mockResolvedValue([ownerSolicitor, ownerRep]);
    // legacy payee
    getCompensationRequisitionAcqPayeesApi.execute.mockImplementation(async () => {
      const existingPayees = [{ ...getMockCompReqAcqPayee(1), legacyPayee: 'Sample LEGACY payee' }];
      getCompensationRequisitionAcqPayeesApi.response = [...existingPayees];
      return [...existingPayees];
    });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();

    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('calls onSuccess when the compensation is saved successfully', async () => {
    const mockCompensationUpdate = getMockApiDefaultCompensation();
    await setup({
      props: { compensation: mockCompensationUpdate },
    });
    putCompensationRequisitionApi.execute.mockResolvedValue(mockCompensationUpdate);

    const model = CompensationRequisitionFormModel.fromApi(mockCompensationUpdate);
    model.detailedRemarks = 'Remarks updated value';
    model.fiscalYear = '2022/2023';

    await act(async () => {
      viewProps?.onSave(model);
    });

    expect(putCompensationRequisitionApi.execute).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
  });

  it('does not call onSuccess if the returned value is invalid', async () => {
    const mockCompensationUpdate = getMockApiDefaultCompensation();
    putCompensationRequisitionApi.execute.mockResolvedValue(undefined);

    await setup({
      props: { compensation: mockCompensationUpdate },
    });

    const model = CompensationRequisitionFormModel.fromApi(mockCompensationUpdate);
    model.detailedRemarks = 'update';
    await act(async () => {
      await viewProps?.onSave(model);
    });

    expect(putCompensationRequisitionApi.execute).toHaveBeenCalled();
    expect(onSuccess).not.toHaveBeenCalled();
  });

  it('filters expired financial codes when updating', async () => {
    const expiredFinancialCodes: ApiGen_Concepts_FinancialCode[] = [
      {
        id: 1,
        type: ApiGen_Concepts_FinancialCodeTypes.Responsibility,
        code: '1',
        description: '1',
        effectiveDate: moment().add(-2, 'days').format('YYYY-MM-DD'),
        expiryDate: moment().add(-1, 'days').format('YYYY-MM-DD'),
        displayOrder: null,
        ...getEmptyBaseAudit(),
      },
      {
        id: 2,
        type: ApiGen_Concepts_FinancialCodeTypes.WorkActivity,
        code: '2',
        description: '2',
        effectiveDate: moment().add(-2, 'days').format('YYYY-MM-DD'),
        expiryDate: moment().add(1, 'days').format('YYYY-MM-DD'),
        displayOrder: null,
        ...getEmptyBaseAudit(),
      },
    ];
    mockGetFinancialCodesApi.execute = vi.fn().mockResolvedValue(expiredFinancialCodes);
    await setup();

    expect(viewProps?.financialActivityOptions).toHaveLength(1);
    expect(viewProps?.chartOfAccountsOptions).toHaveLength(1);
    expect(viewProps?.responsiblityCentreOptions).toHaveLength(1);
    expect(viewProps?.yearlyFinancialOptions).toHaveLength(1);
  });

  describe('ACQUISITION compensation', () => {
    it('calls several endpoints for the list of compensation payees (Owners, Interest Holders, Team members, Legacy Payee)', async () => {
      await setup({
        props: {
          file: {
            ...mockAcquisitionFileResponse(1),
            acquisitionTeam: [
              {
                ...mockApiAcquisitionFileTeamPerson(),
                teamProfileTypeCode: 'MOTILAWYER',
                teamProfileType: {
                  id: 'MOTILAWYER',
                  description: 'MOTI Layer',
                  isDisabled: false,
                  displayOrder: null,
                },
              },
            ],
          },
        },
      });

      expect(getAcquisitionOwnersApi.execute).toHaveBeenCalled();
      expect(getAcquisitionInterestHoldersApi.execute).toHaveBeenCalled();
      // 1 owner + 2 interest holders + 1 team member + 1 legacy payee
      expect(viewProps.payeeOptions).toHaveLength(5);
    });
  });

  describe('LEASE compensation', () => {
    it('makes request to update the compensation with payees', async () => {
      const mockCompensationUpdate = {
        ...getMockApiDefaultCompensation(1, null),
        acquisitionOwnerId: null,
      };
      getLeaseFileInterestHoldersApi.execute.mockResolvedValue(getMockLeaseStakeholders());
      getCompensationRequisitionLeasePayeesApi.execute.mockResolvedValue([
        {
          leaseStakeholderId: 2,
          leaseStakeholder: getMockLeaseStakeholders()[0],
          compensationRequisitionId: 1,
        },
      ] as ApiGen_Concepts_CompReqLeasePayee[]);

      await setup({
        props: {
          compensation: mockCompensationUpdate,
          fileType: ApiGen_CodeTypes_FileTypes.Lease,
          file: { ...getMockApiLease() },
        },
      });

      expect(getLeaseFileInterestHoldersApi.execute).toHaveBeenCalled();

      const updatedCompensationModel = new CompensationRequisitionFormModel(1, null, 1, '');
      updatedCompensationModel.detailedRemarks = 'my update';

      updatedCompensationModel.payees = [
        PayeeOption.createLeaseStakeholder(1, getMockLeaseStakeholders()[0]),
      ];

      await act(async () => {
        viewProps?.onSave(updatedCompensationModel);
      });

      expect(putCompensationRequisitionApi.execute).toHaveBeenCalledWith(
        ApiGen_CodeTypes_FileTypes.Lease,
        expect.objectContaining({
          compReqLeasePayees: [
            expect.objectContaining({
              compensationRequisitionId: 1,
              leaseStakeholder: null,
              leaseStakeholderId: 2,
            }),
          ],
        }),
      );
    });
  });
});
