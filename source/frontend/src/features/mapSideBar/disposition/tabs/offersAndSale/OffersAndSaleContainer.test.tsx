import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { render, RenderOptions } from '@/utils/test-utils';

import OffersAndSaleContainer, { IOffersAndSaleContainerProps } from './OffersAndSaleContainer';
import { IOffersAndSaleContainerViewProps } from './OffersAndSaleContainerView';

const history = createMemoryHistory();

const mockGetDispositionFileOffersApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockGetDispositionFileSalesApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockGetDispositionFileApi = mockDispositionFileResponse(1);

jest.mock('@/hooks/repositories/useDispositionProvider', () => ({
  useDispositionProvider: () => {
    return {
      getDispositionFileOffers: mockGetDispositionFileOffersApi,
      getDispositionFileSales: mockGetDispositionFileSalesApi,
    };
  },
}));

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IOffersAndSaleContainerViewProps | undefined;
const TestView: React.FC<IOffersAndSaleContainerViewProps> = props => {
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
        dispositionFile={renderOptions?.props?.dispositionFile ?? mockGetDispositionFileApi}
        View={TestView}
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
    jest.resetAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetDispositionFileOffersApi.execute).toHaveBeenCalled();
    expect(mockGetDispositionFileSalesApi.execute).toHaveBeenCalled();
  });
});
