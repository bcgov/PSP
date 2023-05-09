import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { mockLookups } from 'mocks';
import { getMockApiDefaultCompensation } from 'mocks/mockCompensations';
import { Api_Compensation } from 'models/api/Compensation';
import { createRef } from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions } from 'utils/test-utils';

import { CompensationRequisitionFormModel } from '../models';
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
        formikRef={createRef()}
        compensation={mockCompensation}
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
    let updatedCompensation: Api_Compensation | undefined;

    let updatedCompensationModel = new CompensationRequisitionFormModel(
      mockCompensation.id,
      mockCompensation.acquisitionFileId,
    );
    updatedCompensationModel.detailedRemarks = 'my update';

    await act(async () => {
      updatedCompensation = await viewProps?.onSave(updatedCompensationModel);
    });

    expect(mockUpdateCompensation).toHaveBeenCalledWith(updatedCompensationModel.toApi());
    expect(updatedCompensation).toStrictEqual(mockCompensation);
  });
});
