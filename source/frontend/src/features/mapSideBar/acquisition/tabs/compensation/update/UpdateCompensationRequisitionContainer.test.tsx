import moment from 'moment';

import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { getMockApiDefaultCompensation } from '@/mocks/compensations.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_FinancialCode } from '@/models/api/FinancialCode';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, waitFor } from '@/utils/test-utils';

import { CompensationRequisitionFormModel, PayeeOption } from './models';
import UpdateCompensationRequisitionContainer from './UpdateCompensationRequisitionContainer';
import { CompensationRequisitionFormProps } from './UpdateCompensationRequisitionForm';

jest.mock('@/hooks/repositories/useRequisitionCompensationRepository');
type Provider = typeof useCompensationRequisitionRepository;

const mockUpdateCompensation = jest.fn();
(useCompensationRequisitionRepository as jest.MockedFunction<Provider>).mockReturnValue({
  updateCompensationRequisition: {
    error: undefined,
    response: undefined,
    execute: mockUpdateCompensation,
    loading: false,
  },
} as unknown as ReturnType<Provider>);

jest.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      getAcquisitionOwners: {
        error: undefined,
        response: mockAcquisitionFileOwnersResponse(1),
        execute: jest.fn().mockReturnValue(mockAcquisitionFileOwnersResponse(1)),
        loading: false,
      },
      getAcquisitionFileSolicitors: {
        execute: jest.fn(),
        loading: false,
      },
      getAcquisitionFileRepresentatives: {
        execute: jest.fn(),
        loading: false,
      },
    };
  },
}));

jest.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      getAcquisitionOwners: {
        error: undefined,
        response: mockAcquisitionFileOwnersResponse(1),
        execute: jest.fn().mockReturnValue(mockAcquisitionFileOwnersResponse(1)),
        loading: false,
      },
      getAcquisitionFileSolicitors: {
        execute: jest.fn(),
        loading: false,
      },
      getAcquisitionFileRepresentatives: {
        execute: jest.fn(),
        loading: false,
      },
    };
  },
}));

jest.mock('@/hooks/repositories/useInterestHolderRepository', () => ({
  useInterestHolderRepository: () => {
    return {
      getAcquisitionInterestHolders: {
        execute: jest.fn(),
        loading: false,
      },
    };
  },
}));

const mockGetApi = {
  error: undefined,
  response: [],
  execute: jest.fn(),
  loading: false,
};

jest.mock('@/hooks/repositories/useFinancialCodeRepository', () => ({
  useFinancialCodeRepository: () => {
    return {
      getFinancialActivityCodeTypes: mockGetApi,
      getChartOfAccountsCodeTypes: mockGetApi,
      getResponsibilityCodeTypes: mockGetApi,
      getYearlyFinancialsCodeTypes: mockGetApi,
    };
  },
}));

let viewProps: CompensationRequisitionFormProps | undefined;
const TestView: React.FC<CompensationRequisitionFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const mockCompensation = getMockApiDefaultCompensation();
const onSuccess = jest.fn();
const onCancel = jest.fn();

describe('UpdateCompensationRequisition Container component', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateCompensationRequisitionContainer
        compensation={mockCompensation}
        acquisitionFile={mockAcquisitionFileResponse()}
        onSuccess={onSuccess}
        onCancel={onCancel}
        View={TestView}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it.skip('Calls onSuccess when the compensation is saved successfully', async () => {
    setup();
    mockUpdateCompensation.mockResolvedValue(mockCompensation);

    let updatedCompensationModel = new CompensationRequisitionFormModel(
      mockCompensation.id,
      mockCompensation.acquisitionFileId,
    );
    updatedCompensationModel.detailedRemarks = 'Remarks updated value';
    updatedCompensationModel.fiscalYear = '2022/2023';
    updatedCompensationModel.payeeKey = '1';

    await act(async () => {
      viewProps?.onSave(updatedCompensationModel);
    });

    expect(mockUpdateCompensation).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
  });

  it.skip('does not call onSucess if the returned value is invalid', async () => {
    setup();
    mockUpdateCompensation.mockResolvedValue(undefined);

    let updatedCompensationModel = new CompensationRequisitionFormModel(
      mockCompensation.id,
      mockCompensation.acquisitionFileId,
    );

    await act(async () => {
      await viewProps?.onSave(updatedCompensationModel);
    });

    expect(mockUpdateCompensation).toHaveBeenCalled();
    expect(onSuccess).not.toHaveBeenCalled();
  });

  it.skip('makes request to update the compensation and returns the response', async () => {
    setup();
    mockCompensation.detailedRemarks = 'my update';
    mockUpdateCompensation.mockResolvedValue(mockCompensation);
    let updatedCompensation: Api_CompensationRequisition | undefined;

    let updatedCompensationModel = new CompensationRequisitionFormModel(
      mockCompensation.id,
      mockCompensation.acquisitionFileId,
    );
    updatedCompensationModel.detailedRemarks = 'my update';
    updatedCompensationModel.payeeKey = '1';

    await act(async () => {
      updatedCompensation = await viewProps?.onSave(updatedCompensationModel);
    });

    expect(mockUpdateCompensation).toHaveBeenCalledWith(updatedCompensationModel.toApi([]));
    expect(updatedCompensation).toStrictEqual(mockCompensation);
  });

  it('makes request to update the compensation with payees', async () => {
    await setup();

    mockCompensation.detailedRemarks = 'my update';
    mockUpdateCompensation.mockResolvedValue(mockCompensation);

    let updatedCompensationModel = new CompensationRequisitionFormModel(
      mockCompensation.id,
      mockCompensation.acquisitionFileId,
    );
    updatedCompensationModel.detailedRemarks = 'my update';

    const testPayeeOption: PayeeOption = PayeeOption.createOwner(
      mockAcquisitionFileOwnersResponse(1)[0],
    );

    updatedCompensationModel.payeeKey = testPayeeOption.value;

    setTimeout(async () => {
      await act(async () => {
        await viewProps?.onSave(updatedCompensationModel);
      });

      expect(mockUpdateCompensation).toHaveBeenCalledWith(
        updatedCompensationModel.toApi([testPayeeOption]),
      );
    }, 500);
  });

  it('filters expired financial codes when updating', async () => {
    const expiredFinancialCodes: Api_FinancialCode[] = [
      {
        id: 1,
        type: 'expired',
        code: '1',
        description: '1',
        effectiveDate: moment().add(-2, 'days').format('YYYY-MM-DD'),
        expiryDate: moment().add(-1, 'days').format('YYYY-MM-DD'),
      },
      {
        id: 2,
        type: 'non-expired',
        code: '2',
        description: '2',
        effectiveDate: moment().add(-2, 'days').format('YYYY-MM-DD'),
        expiryDate: moment().add(1, 'days').format('YYYY-MM-DD'),
      },
    ];
    mockGetApi.execute = jest.fn().mockResolvedValue(expiredFinancialCodes);
    await setup();

    await waitFor(async () => {
      expect(viewProps?.financialActivityOptions).toHaveLength(1);
      expect(viewProps?.chartOfAccountsOptions).toHaveLength(1);
      expect(viewProps?.responsiblityCentreOptions).toHaveLength(1);
      expect(viewProps?.yearlyFinancialOptions).toHaveLength(1);
    });
  });
});
