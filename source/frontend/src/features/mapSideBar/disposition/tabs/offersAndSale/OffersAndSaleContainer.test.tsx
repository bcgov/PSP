import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { render, RenderOptions } from '@/utils/test-utils';

import OffersAndSaleContainer, { IOffersAndSaleContainerProps } from './OffersAndSaleContainer';
import { IOffersAndSaleViewProps } from './OffersAndSaleView';

const history = createMemoryHistory();

const mockGetDispositionFileOffersApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetDispositionFileSalesApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetDispositionFileAppraisalApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockDeleteDispositionFileOfferApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const onSuccess = vi.fn();

vi.mock('@/hooks/repositories/useDispositionProvider', () => ({
  useDispositionProvider: () => {
    return {
      getDispositionFileOffers: mockGetDispositionFileOffersApi,
      getDispositionFileSale: mockGetDispositionFileSalesApi,
      getDispositionAppraisal: mockGetDispositionFileAppraisalApi,
      deleteDispositionOffer: mockDeleteDispositionFileOfferApi,
    };
  },
}));

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IOffersAndSaleViewProps | undefined;
const TestView: React.FC<IOffersAndSaleViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('OffersAndSale Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IOffersAndSaleContainerProps>;
    } = {},
  ) => {
    const component = render(
      <OffersAndSaleContainer
        dispositionFile={renderOptions?.props?.dispositionFile ?? mockDispositionFileResponse()}
        View={TestView}
        onSuccess={onSuccess}
      />,
      {
        history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_VIEW],
        ...renderOptions,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    vi.resetAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetDispositionFileOffersApi.execute).toHaveBeenCalled();
    expect(mockGetDispositionFileSalesApi.execute).toHaveBeenCalled();
  });
});
