import { mockLookups } from '@/mocks/index.mock';

import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import UpdateAgreementForm, { IUpdateAgreementFormProps } from './UpdateAgreementForm';
import { AgreementFormModel } from '../models/AgreementFormModel';
import {
  act,
  RenderOptions,
  render,
  fillInput,
  waitForEffects,
  selectOptions,
} from '@/utils/test-utils';

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
  const setup = async (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateAgreementForm
        isLoading={false}
        initialValues={mockViewProps.initialValues}
        onSubmit={mockViewProps.onSubmit}
        onCancel={mockViewProps.onCancel}
        fileType={mockViewProps.fileType}
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
});
