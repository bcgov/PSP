import { mockLookups } from '@/mocks/index.mock';

import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import UpdateAcquisitionAgreementView, { IUpdateAcquisitionAgreementViewProps } from './UpdateAcquisitionAgreementView';
import { AcquisitionAgreementFormModel } from '../models/AcquisitionAgreementFormModel';
import { act, RenderOptions, render, fillInput, waitForEffects, selectOptions } from '@/utils/test-utils';


export const organizerMock = {
  canEditOrDeleteAgreement: jest.fn(),
};

const mockViewProps: IUpdateAcquisitionAgreementViewProps = {
  isLoading: false,
  initialValues: new AcquisitionAgreementFormModel(1),
  onSave: jest.fn(),
  onSuccess: jest.fn(),
  onError: jest.fn(),
  onCancel: jest.fn(),
};

describe('UpdateAcquisitionAgreementView component', () => {
  const setup = async (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateAcquisitionAgreementView
        isLoading={false}
        initialValues={mockViewProps.initialValues}
        onSave={mockViewProps.onSave}
        onSuccess={mockViewProps.onSuccess}
        onError={mockViewProps.onError}
        onCancel={mockViewProps.onCancel}
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

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('does not display cancellation note by default', async () => {
    const { queryByText } = await setup();

    expect(queryByText(/Cancellation reason/i)).toBeNull();
  });

  it('displays cancellation note if status is cancelled', async () => {
    const {container, getByText } = await setup();

    await act(async () => {
      fillInput(container, 'agreementStatusTypeCode', 'CANCELLED', 'select');
    });

    expect(getByText(/Cancellation reason/i)).toBeVisible();
  });
});
