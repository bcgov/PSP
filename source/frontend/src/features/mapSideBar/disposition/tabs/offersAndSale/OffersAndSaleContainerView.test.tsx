import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import OffersAndSaleContainerView, {
  IOffersAndSaleContainerViewProps,
} from './OffersAndSaleContainerView';

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

const mockDispositionFileApi = mockDispositionFileResponse(1);

describe('Disposition Offer Detail View component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IOffersAndSaleContainerViewProps> },
  ) => {
    const utils = render(
      <OffersAndSaleContainerView
        {...renderOptions.props}
        loading={renderOptions.props?.loading ?? false}
        dispositionFile={renderOptions.props?.dispositionFile ?? mockDispositionFileApi}
        dispositionOffers={[]}
        dispositionSale={renderOptions.props?.dispositionFile?.sales[0] ?? null}
      />,
      {
        ...renderOptions,
        history: history,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_VIEW],
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  // it('renders as expected', async () => {
  //   const { asFragment } = await setup({});
  //   const fragment = await waitFor(() => asFragment());
  //   expect(fragment).toMatchSnapshot();
  // });

  it('displays a message when Disposition has no Appraisal', async () => {
    const mockDisposition = mockDispositionFileApi;
    mockDisposition.sales = [];
    mockDisposition.appraisedValueAmount = null;
    mockDisposition.appraisalDate = null;
    mockDisposition.bcaValueAmount = null;
    mockDisposition.bcaRollYear = null;
    mockDisposition.listPriceAmount = null;

    const { getByText } = await setup({
      props: { dispositionFile: mockDisposition, dispositionOffers: [] },
    });
    expect(
      getByText(/There are no value details indicated with this disposition file/i),
    ).toBeVisible();
  });

  it('displays a message when Disposition has no offers', async () => {
    const mockDisposition = mockDispositionFileApi;
    mockDisposition.sales = [];

    const { getByText } = await setup({
      props: { dispositionFile: mockDisposition, dispositionOffers: [] },
    });
    expect(getByText(/There are no offers indicated with this disposition file/i)).toBeVisible();
  });

  it('displays a message when Disposition SALE has no data', async () => {
    const mockDisposition = mockDispositionFileApi;
    mockDisposition.sales = [];

    const { getByText } = await setup({
      props: { dispositionFile: mockDisposition, dispositionOffers: [] },
    });
    expect(
      getByText(/There are no sale details indicated with this disposition file/i),
    ).toBeVisible();
  });

  // Displays the appraisal Data

  // Displays the SALE Data
});
