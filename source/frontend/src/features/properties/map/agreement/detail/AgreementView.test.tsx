import { mockLookups } from 'mocks';
import { mockAgreementsResponse } from 'mocks/mockAgreements';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions } from 'utils/test-utils';

import AgreementView, { IAgreementViewProps } from './AgreementView';

// mock auth library
jest.mock('@react-keycloak/web');

const mockViewProps: IAgreementViewProps = {
  agreements: [],
  onEdit: jest.fn(),
  loading: false,
};

describe('AgreementView component', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AgreementView
        agreements={mockViewProps.agreements}
        onEdit={mockViewProps.onEdit}
        loading={mockViewProps.loading}
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
    jest.clearAllMocks();
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
