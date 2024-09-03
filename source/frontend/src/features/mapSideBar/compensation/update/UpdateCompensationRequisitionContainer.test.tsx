import moment from 'moment';

import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { getMockApiDefaultCompensation } from '@/mocks/compensations.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants/systemConstantsSlice';
import { act, render, RenderOptions, waitFor, waitForEffects } from '@/utils/test-utils';

import UpdateCompensationRequisitionContainer, {
  UpdateCompensationRequisitionContainerProps,
} from './UpdateCompensationRequisitionContainer';
import { CompensationRequisitionFormProps } from './UpdateCompensationRequisitionForm';
import { CompensationRequisitionFormModel } from '../models/CompensationRequisitionFormModel';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { getMockApiLease, getMockLeaseStakeholders } from '@/mocks/lease.mock';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { PayeeType } from '../../acquisition/models/PayeeTypeModel';
import { CompensationPayeeFormModel } from '../models/AcquisitionPayeeFormModel';

const mockGetAcquisitionOwnersApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      getAcquisitionOwners: mockGetAcquisitionOwnersApi,
      getAcquisitionFileSolicitors: {
        execute: vi.fn(),
        loading: false,
      },
      getAcquisitionFileRepresentatives: {
        execute: vi.fn(),
        loading: false,
      },
    };
  },
}));

const mockGetApi = {
  error: undefined,
  response: [],
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useFinancialCodeRepository', () => ({
  useFinancialCodeRepository: () => {
    return {
      getFinancialActivityCodeTypes: mockGetApi,
      getChartOfAccountsCodeTypes: mockGetApi,
      getResponsibilityCodeTypes: mockGetApi,
      getYearlyFinancialsCodeTypes: mockGetApi,
    };
  },
}));

const getLeaseFileInterestHoldersApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useLeaseStakeholderRepository', () => ({
  useLeaseStakeholderRepository: () => {
    return {
      getLeaseStakeholders: getLeaseFileInterestHoldersApi,
    };
  },
}));

const getAcquisitionInterestHoldersApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};
vi.mock('@/hooks/repositories/useInterestHolderRepository', () => ({
  useInterestHolderRepository: () => {
    return {
      getAcquisitionInterestHolders: getAcquisitionInterestHoldersApi,
    };
  },
}));

const putCompensationRequisitionApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};
vi.mock('@/hooks/repositories/useRequisitionCompensationRepository', () => ({
  useCompensationRequisitionRepository: () => {
    return {
      updateCompensationRequisition: putCompensationRequisitionApi,
    };
  },
}));

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
    const component = render(
      <UpdateCompensationRequisitionContainer
        compensation={renderOptions?.props?.compensation ?? getMockApiDefaultCompensation()}
        fileType={renderOptions?.props?.fileType ?? ApiGen_CodeTypes_FileTypes.Acquisition}
        file={renderOptions?.props?.file ?? mockAcquisitionFileResponse(1)}
        onSuccess={onSuccess}
        onCancel={onCancel}
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

    return {
      ...component,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    await act(async () => {});
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

  it('makes request to update the compensation with payees', async () => {
    const acquisitionFileMock: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
    };
    getAcquisitionInterestHoldersApi.execute.mockResolvedValue([]);
    mockGetAcquisitionOwnersApi.execute.mockResolvedValue(mockAcquisitionFileOwnersResponse());

    const mockCompensationUpdate = getMockApiDefaultCompensation(1, null);
    putCompensationRequisitionApi.execute.mockResolvedValue({
      ...mockCompensationUpdate,
      acquisitionOwnerId: 1,
    });

    await act(async () => {
      setup({
        props: {
          compensation: mockCompensationUpdate,
          fileType: ApiGen_CodeTypes_FileTypes.Acquisition,
          file: acquisitionFileMock,
        },
      });
    });
    await waitForEffects();

    expect(mockGetAcquisitionOwnersApi.execute).toHaveBeenCalled();
    expect(getAcquisitionInterestHoldersApi.execute).toHaveBeenCalled();

    const compReqModel = CompensationRequisitionFormModel.fromApi(mockCompensationUpdate);
    compReqModel.payee = new CompensationPayeeFormModel(mockCompensationUpdate.id);
    compReqModel.payee.payeeKey = PayeeOption.generateKey(1, PayeeType.Owner);

    await act(async () => {
      viewProps?.onSave(compReqModel);
    });

    expect(putCompensationRequisitionApi.execute).toHaveBeenCalledWith(
      ApiGen_CodeTypes_FileTypes.Acquisition,
      expect.objectContaining({ acquisitionOwnerId: 1 }),
    );
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
    mockGetApi.execute = vi.fn().mockResolvedValue(expiredFinancialCodes);
    await setup();
    await act(async () => {});

    await waitFor(async () => {
      expect(viewProps?.financialActivityOptions).toHaveLength(1);
      expect(viewProps?.chartOfAccountsOptions).toHaveLength(1);
      expect(viewProps?.responsiblityCentreOptions).toHaveLength(1);
      expect(viewProps?.yearlyFinancialOptions).toHaveLength(1);
    });
  });

  it('LEASE - makes request to update the compensation with payees', async () => {
    const mockCompensationUpdate = {
      ...getMockApiDefaultCompensation(1, null),
      acquisitionOwnerId: null,
    };
    getLeaseFileInterestHoldersApi.execute.mockResolvedValue(getMockLeaseStakeholders());

    await act(async () => {
      setup({
        props: {
          compensation: mockCompensationUpdate,
          fileType: ApiGen_CodeTypes_FileTypes.Lease,
          file: { ...getMockApiLease() },
        },
      });
    });
    await waitForEffects();

    expect(getLeaseFileInterestHoldersApi.execute).toHaveBeenCalled();

    const updatedCompensationModel = new CompensationRequisitionFormModel(1, null, 1, '');
    updatedCompensationModel.detailedRemarks = 'my update';

    const testPayeeOption: PayeeOption = PayeeOption.createLeaseStakeholder(
      getMockLeaseStakeholders()[0],
    );

    const leasePayeeOptions: PayeeOption[] = getMockLeaseStakeholders().map(x =>
      PayeeOption.createLeaseStakeholder(x),
    );

    updatedCompensationModel.payee.payeeKey = testPayeeOption.value;

    await act(async () => {
      viewProps?.onSave(updatedCompensationModel);
    });

    const concept = updatedCompensationModel.toApi(leasePayeeOptions);
    expect(putCompensationRequisitionApi.execute).toHaveBeenCalledWith(
      ApiGen_CodeTypes_FileTypes.Lease,
      {
        ...concept,
        compReqLeaseStakeholder: [
          {
            compReqLeaseStakeholderId: null,
            compensationRequisitionId: 1,
            leaseStakeholder: null,
            leaseStakeholderId: 2,
          },
        ],
      },
    );
  });
});
