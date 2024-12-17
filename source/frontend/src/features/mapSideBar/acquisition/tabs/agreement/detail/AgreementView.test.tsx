import { Claims } from '@/constants';
import { mockAgreementsResponse } from '@/mocks/agreements.mock';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import AcquisitionFileStatusUpdateSolver from '../../fileDetails/detail/AcquisitionFileStatusUpdateSolver';
import AgreementView, { IAgreementViewProps } from './AgreementView';
import { ApiGen_CodeTypes_AcquisitionStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionStatusTypes';

// mock auth library

const mockViewProps: IAgreementViewProps = {
  agreements: [],
  statusUpdateSolver: new AcquisitionFileStatusUpdateSolver(),
  onGenerate: vi.fn(),
  loading: false,
  onDelete: vi.fn(),
};

describe('AgreementView component', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AgreementView
        loading={mockViewProps.loading}
        agreements={mockViewProps.agreements}
        statusUpdateSolver={mockViewProps.statusUpdateSolver}
        onGenerate={mockViewProps.onGenerate}
        onDelete={mockViewProps.onDelete}
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
    mockViewProps.statusUpdateSolver = new AcquisitionFileStatusUpdateSolver();
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
    mockViewProps.statusUpdateSolver = new AcquisitionFileStatusUpdateSolver({
      id: ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE,
      description: '',
      displayOrder: 1,
      isDisabled: false,
    });

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
});
