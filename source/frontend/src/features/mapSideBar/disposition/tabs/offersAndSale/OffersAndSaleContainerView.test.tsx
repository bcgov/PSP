import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import {
  mockDispositionFileResponse,
  mockDispositionFileSaleApi,
} from '@/mocks/dispositionFiles.mock';
import { render, RenderOptions, waitFor, waitForEffects } from '@/utils/test-utils';

import OffersAndSaleContainerView, {
  IOffersAndSaleContainerViewProps,
} from './OffersAndSaleContainerView';

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

const mockDispositionFileApi = mockDispositionFileResponse(1);
const mockDispositionSaleApi = mockDispositionFileSaleApi(1);

const onDelete = jest.fn();

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
        dispositionSale={renderOptions.props?.dispositionSale ?? null}
        dispositionAppraisal={renderOptions.props?.dispositionAppraisal ?? null}
        onDispositionOfferDeleted={onDelete}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
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

  it('renders as expected', async () => {
    const { asFragment } = await setup({
      props: {
        dispositionFile: mockDispositionFileApi,
        dispositionOffers: [],
        dispositionSale: mockDispositionSaleApi,
      },
    });

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('displays a message when Disposition has no Appraisal', async () => {
    const mockDisposition = mockDispositionFileApi;
    mockDisposition.dispositionSale = null;
    mockDisposition.dispositionAppraisal = null;

    const { getByText } = await setup({
      props: { dispositionFile: mockDisposition, dispositionOffers: [] },
    });
    expect(
      getByText(/There are no sale details indicated with this disposition file/i),
    ).toBeVisible();
  });

  it('displays the Disposition Appraisal and Value when available', async () => {
    const mockDisposition = mockDispositionFileResponse(1);

    const { queryByTestId } = await setup({
      props: {
        dispositionFile: mockDisposition,
        dispositionOffers: [],
        dispositionAppraisal: mockDisposition.dispositionAppraisal,
      },
    });

    expect(queryByTestId('disposition-file.appraisedValueAmount')).toHaveTextContent('$550,000.00');
    expect(queryByTestId('disposition-file.appraisalDate')).toHaveTextContent('Dec 28, 2023');
    expect(queryByTestId('disposition-file.bcaValueAmount')).toHaveTextContent('$600,000.00');
    expect(queryByTestId('disposition-file.bcaAssessmentRollYear')).toHaveTextContent('2023');
    expect(queryByTestId('disposition-file.listPriceAmount')).toHaveTextContent('$590,000.00');
  });

  it('hides the edit button for Appraisal for users without permissions', async () => {
    const { queryByTitle } = await setup({
      props: { dispositionFile: mockDispositionFileResponse() },
      claims: [Claims.DISPOSITION_VIEW],
    });
    await waitForEffects();

    const editButton = queryByTitle('Edit Appraisal');

    expect(editButton).toBeNull();
  });

  it('renders the edit button for Appraisal for users with disposition edit permissions', async () => {
    const { getByTitle } = await setup({
      props: { dispositionFile: mockDispositionFileResponse() },
      claims: [Claims.DISPOSITION_EDIT],
    });
    await waitForEffects();

    const editButton = getByTitle('Edit Appraisal');

    expect(editButton).toBeVisible();
  });

  it('displays a message when Disposition has no offers', async () => {
    const mockDisposition = mockDispositionFileApi;
    mockDisposition.dispositionSale = null;

    const { getByText } = await setup({
      props: { dispositionFile: mockDisposition, dispositionOffers: [] },
    });
    expect(getByText(/There are no offers indicated with this disposition file/i)).toBeVisible();
  });

  it('displays a message when Disposition SALE has no data', async () => {
    const mockDisposition = mockDispositionFileApi;

    const { getByText } = await setup({
      props: { dispositionFile: mockDisposition, dispositionOffers: [], dispositionSale: null },
    });

    expect(
      getByText(/There are no sale details indicated with this disposition file/i),
    ).toBeVisible();
  });

  it('displays the Disposition Sale information when available', async () => {
    const { queryByTestId, getByText } = await setup({
      props: {
        dispositionFile: mockDispositionFileApi,
        dispositionOffers: [],
        dispositionSale: mockDispositionSaleApi,
      },
    });

    expect(queryByTestId('disposition-sale.finalConditionRemovalDate')).toHaveTextContent(
      'Jan 30, 2022',
    );
    expect(queryByTestId('disposition-sale.saleCompletionDate')).toHaveTextContent('Jan 30, 2024');
    expect(queryByTestId('disposition-sale.saleFiscalYear')).toHaveTextContent('2023');
    expect(queryByTestId('disposition-sale.finalSaleAmount')).toHaveTextContent('$746,325.23');
    expect(queryByTestId('disposition-sale.realtorCommissionAmount')).toHaveTextContent(
      '$12,500.27',
    );
    expect(queryByTestId('disposition-sale.isGstRequired')).toHaveTextContent('Yes');
    expect(queryByTestId('disposition-sale.gstCollectedAmount')).toHaveTextContent('36,489.36');
    expect(queryByTestId('disposition-sale.netBookAmount')).toHaveTextContent('$246.2');
    expect(queryByTestId('disposition-sale.totalCostAmount')).toHaveTextContent('$856,320.36');
    expect(queryByTestId('disposition-sale.netProceedsBeforeSppAmount')).toHaveTextContent(
      '-$159,230.96',
    );
    expect(queryByTestId('disposition-sale.sppAmount')).toHaveTextContent('$1,000.00');
    expect(queryByTestId('disposition-sale.netProceedsAfterSppAmount')).toHaveTextContent(
      '-$160,230.96',
    );
    expect(queryByTestId('disposition-sale.remediationAmount')).toHaveTextContent('$1.00');

    // Purchasers
    expect(getByText(`Stinky Cheese`)).toBeInTheDocument();
    expect(getByText(`Alejandro Sanchez`)).toBeInTheDocument();
    expect(getByText(`Ministry of Transportation and Infrastructure`)).toBeInTheDocument();
    expect(getByText(`Manuel Rodriguez`)).toBeInTheDocument();

    // Agent
    expect(getByText(`JOHN DOE`)).toBeInTheDocument();

    // Solicitor
    expect(getByText(`JANE DOE`)).toBeInTheDocument();
  });
});
