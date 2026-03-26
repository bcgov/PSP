import { Claims } from '@/constants';
import { mockAgreementsResponse } from '@/mocks/agreements.mock';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';
import AgreementView, { IAgreementViewProps } from './AgreementView';

// mock auth library

const mockViewProps: IAgreementViewProps = {
  agreements: [],
  onGenerate: vi.fn(),
  loading: false,
  onDelete: vi.fn(),
};

describe('AgreementView component', () => {
  const setup = (renderOptions: RenderOptions & { props?: Partial<IAgreementViewProps> } = {}) => {
    const utils = render(
      <AgreementView
        loading={mockViewProps.loading}
        agreements={mockViewProps.agreements}
        onGenerate={mockViewProps.onGenerate}
        onDelete={mockViewProps.onDelete}
        isSection3={renderOptions?.props?.isSection3 ?? false}
        isFileFinalStatus={renderOptions?.props?.isFileFinalStatus ?? false}
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
    mockViewProps.agreements = mockAgreementsResponse();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the agreement type', () => {
    const { getByText } = setup();
    expect(getByText(/License Of Occupation/i)).toBeVisible();
  });

  it('calls onGenerate when generation button is clicked', async () => {
    const { getAllByTitle } = setup();

    const generateButtons = getAllByTitle(/Download File/i);
    expect(generateButtons).toHaveLength(2);
    await act(async () => userEvent.click(generateButtons[0]));

    expect(mockViewProps.onGenerate).toHaveBeenCalled();
  });

  it('displays confirmation modal when Delete Agreement button is clicked', async () => {
    const { getAllByTitle } = setup({
      claims: [Claims.ACQUISITION_EDIT],
    });

    const removeButtons = getAllByTitle(/Delete Agreement/i);
    expect(removeButtons).toHaveLength(2);
    await act(async () => userEvent.click(removeButtons[0]));

    expect(
      screen.getByText(/You have selected to delete this Agreement/i, { exact: false }),
    ).toBeVisible();
  });

  it('displays warning tooltips instead of edit/add buttons when file in final status', async () => {
    const { queryByText } = setup({
      claims: [Claims.ACQUISITION_EDIT],
      props: { isFileFinalStatus: true },
    });

    const removeButton = queryByText(/Delete Agreement/i);
    expect(removeButton).toBeNull();

    const editButton = queryByText(/Edit Agreement/i);
    expect(removeButton).toBeNull();

    expect(screen.getByTestId(/tooltip-icon-agreement-cannot-add-tooltip/i)).toBeVisible();
  });

  it('displays section 3 "Advance payment" and "Signed Date"', async () => {
    const { getByTestId } = setup({
      claims: [Claims.ACQUISITION_EDIT],
      props: { isFileFinalStatus: true, isSection3: true },
    });

    const advancePaymentDate0 = getByTestId('agreement[0].advancePaymentDate');
    expect(advancePaymentDate0).toHaveTextContent('');
    const advancePaymentDate1 = getByTestId('agreement[1].advancePaymentDate');
    expect(advancePaymentDate1).toHaveTextContent('Mar 26, 2026');

    const agreementSignedDate0 = getByTestId('agreement[0].agreementSignedDate');
    expect(agreementSignedDate0).toHaveTextContent('');
    const agreementSignedDate1 = getByTestId('agreement[1].agreementSignedDate');
    expect(agreementSignedDate1).toHaveTextContent('Mar 25, 2026');
  });

  it('hides section 3 "Advance payment" and "Signed Date"', async () => {
    const { queryByTestId } = setup({
      claims: [Claims.ACQUISITION_EDIT],
      props: { isFileFinalStatus: true, isSection3: false },
    });

    const advancePaymentDate = queryByTestId('agreement[0].advancePaymentDate');
    expect(advancePaymentDate).not.toBeInTheDocument();

    const agreementSignedDate = queryByTestId('agreement[0].agreementSignedDate');
    expect(agreementSignedDate).not.toBeInTheDocument();
  });
});
