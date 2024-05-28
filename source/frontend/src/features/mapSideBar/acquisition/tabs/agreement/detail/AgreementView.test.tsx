import { mockAgreementsResponse } from '@/mocks/agreements.mock';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import AgreementView, { IAgreementViewProps } from './AgreementView';
import StatusUpdateSolver from '../../fileDetails/detail/statusUpdateSolver';

// mock auth library

const mockViewProps: IAgreementViewProps = {
  agreements: [],
  statusUpdateSolver: new StatusUpdateSolver(),
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
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the agreement type ', () => {
    const { getByText } = setup();
    expect(getByText(/License Of Occupation/i)).toBeVisible();
  });
});
