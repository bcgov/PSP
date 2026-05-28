import { mockLookups } from '@/mocks/index.mock';

import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import UpdateAgreementForm, { IUpdateAgreementFormProps } from './UpdateAgreementForm';
import { AgreementFormModel } from '../models/AgreementFormModel';
import { act, RenderOptions, render, fillInput } from '@/utils/test-utils';

export const organizerMock = {
  canEditOrDeleteAgreement: vi.fn(),
};

const mockViewProps: IUpdateAgreementFormProps = {
  isLoading: false,
  initialValues: new AgreementFormModel(1),
  onSubmit: vi.fn(),
  onCancel: vi.fn(),
  fileType: 'acquisition',
};

describe('UpdateAcquisitionAgreementView component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IUpdateAgreementFormProps> } = {},
  ) => {
    const utils = render(
      <UpdateAgreementForm
        isLoading={false}
        initialValues={mockViewProps.initialValues}
        onSubmit={mockViewProps.onSubmit}
        onCancel={mockViewProps.onCancel}
        fileType={mockViewProps.fileType}
        isSection3={renderOptions?.props?.isSection3 ?? false}
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
      getAdvancePaymentDatePicker: () =>
        utils.container.querySelector(`input[name="advancePaymentDate"]`) as HTMLInputElement,
      getAgreementSignedDatePicker: () =>
        utils.container.querySelector(`input[name="agreementSignedDate"]`) as HTMLInputElement,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
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
    const { container, getByText } = await setup();

    await act(async () => {
      fillInput(container, 'agreementStatusTypeCode', 'CANCELLED', 'select');
    });

    expect(getByText(/Cancellation reason/i)).toBeVisible();
  });

  it('displays section 3 "Advance payment" and "Signed Date"', async () => {
    const { getAdvancePaymentDatePicker, getAgreementSignedDatePicker } = await setup({
      props: {
        isSection3: true,
      },
    });

    expect(getAdvancePaymentDatePicker()).toBeInTheDocument();
    expect(getAgreementSignedDatePicker()).toBeInTheDocument();
  });

  it('hides section 3 "Advance payment" and "Signed Date"', async () => {
    const { getAdvancePaymentDatePicker, getAgreementSignedDatePicker } = await setup({
      props: {
        isSection3: false,
      },
    });

    expect(getAdvancePaymentDatePicker()).not.toBeInTheDocument();
    expect(getAgreementSignedDatePicker()).not.toBeInTheDocument();
  });
});
