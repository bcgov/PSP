import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from 'mocks/acquisitionFiles.mock';
import { getMockApiDefaultCompensation } from 'mocks/compensations.mock';
import { mockLookups } from 'mocks/lookups.mock';
import { Api_CompensationRequisition } from 'models/api/CompensationRequisition';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions } from 'utils/test-utils';

import { CompensationRequisitionFormModel, PayeeOption } from '../models';
import UpdateCompensationRequisitionContainer from './UpdateCompensationRequisitionContainer';
import { CompensationRequisitionFormProps } from './UpdateCompensationRequisitionForm';

jest.mock('hooks/repositories/useRequisitionCompensationRepository');
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

jest.mock('hooks/repositories/useAcquisitionProvider', () => ({
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

let viewProps: CompensationRequisitionFormProps | undefined;
const TestView: React.FC<CompensationRequisitionFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const mockCompensation = getMockApiDefaultCompensation();
const onSuccess = jest.fn();
const onCancel = jest.fn();

describe('UpdateAgreementsContainer component', () => {
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

  it('Calls onSuccess when the compensation is saved successfully', async () => {
    setup();
    mockUpdateCompensation.mockResolvedValue(mockCompensation);

    let updatedCompensationModel = new CompensationRequisitionFormModel(
      mockCompensation.id,
      mockCompensation.acquisitionFileId,
    );
    updatedCompensationModel.detailedRemarks = 'Remarks updated value';

    await act(async () => {
      viewProps?.onSave(updatedCompensationModel);
    });

    expect(mockUpdateCompensation).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
  });

  it('does not call onSucess if the returned value is invalid', async () => {
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

  it('makes request to update the compensation and returns the response', async () => {
    setup();
    mockCompensation.detailedRemarks = 'my update';
    mockUpdateCompensation.mockResolvedValue(mockCompensation);
    let updatedCompensation: Api_CompensationRequisition | undefined;

    let updatedCompensationModel = new CompensationRequisitionFormModel(
      mockCompensation.id,
      mockCompensation.acquisitionFileId,
    );
    updatedCompensationModel.detailedRemarks = 'my update';

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
});
