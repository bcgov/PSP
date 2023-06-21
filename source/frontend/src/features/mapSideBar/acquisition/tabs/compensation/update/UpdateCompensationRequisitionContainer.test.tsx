import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import {
  getMockApiCompensation,
  getMockApiDefaultCompensation,
  getMockApiFinalCompensation,
} from '@/mocks/compensations.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants/systemConstantsSlice';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { CompensationRequisitionFormModel, PayeeOption } from './models';
import UpdateCompensationRequisitionContainer, {
  UpdateCompensationRequisitionContainerProps,
} from './UpdateCompensationRequisitionContainer';
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

jest.mock('@/hooks/repositories/useFinancialCodeRepository', () => ({
  useInterestHolderRepository: () => {
    return {
      getFinancialActivityCodeTypes: {
        execute: jest.fn(),
        loading: false,
      },
      getChartOfAccountsCodeTypes: {
        execute: jest.fn(),
        loading: false,
      },
      getYearlyFinancialsCodeTypes: {
        execute: jest.fn(),
        loading: false,
      },
      getResponsibilityCodeTypes: {
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
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<UpdateCompensationRequisitionContainerProps> },
  ) => {
    const component = render(
      <UpdateCompensationRequisitionContainer
        compensation={renderOptions?.props?.compensation ?? getMockApiCompensation()}
        acquisitionFile={renderOptions?.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
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
    jest.clearAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup({});
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('Calls onSuccess when the compensation is saved successfully', async () => {
    const mockCompensationUpdate = getMockApiFinalCompensation();
    await setup({
      props: { compensation: mockCompensationUpdate },
    });
    mockUpdateCompensation.mockResolvedValue(mockCompensationUpdate);

    const model = CompensationRequisitionFormModel.fromApi(mockCompensationUpdate);
    model.detailedRemarks = 'Remarks updated value';
    model.fiscalYear = '2022/2023';

    await act(async () => {
      viewProps?.onSave(model);
    });

    expect(mockUpdateCompensation).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
  });

  it('does not call onSucess if the returned value is invalid', async () => {
    const mockCompensationUpdate = getMockApiFinalCompensation();
    mockUpdateCompensation.mockResolvedValue(undefined);

    await setup({
      props: { compensation: mockCompensationUpdate },
    });

    const model = CompensationRequisitionFormModel.fromApi(mockCompensationUpdate);
    model.detailedRemarks = 'update';
    await act(async () => {
      await viewProps?.onSave(model);
    });

    expect(mockUpdateCompensation).toHaveBeenCalled();
    expect(onSuccess).not.toHaveBeenCalled();
  });

  it('makes request to update the compensation with payees', async () => {
    const mockCompensationUpdate = getMockApiFinalCompensation();
    await setup({
      props: { compensation: mockCompensationUpdate },
    });

    mockCompensationUpdate.detailedRemarks = 'my update';
    mockUpdateCompensation.mockResolvedValue(mockCompensationUpdate);

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
});
