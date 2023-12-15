import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { mockDispositionFileOfferApi } from '@/mocks/dispositionFiles.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import DispositionOfferDetails, { IDispositionOfferDetailsProps } from './DispositionOfferDetails';

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

const mockDispositionOffer = mockDispositionFileOfferApi(100, 1);

describe('Disposition Offer Detail View component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IDispositionOfferDetailsProps> },
  ) => {
    const utils = render(
      <DispositionOfferDetails
        {...renderOptions.props}
        index={0}
        dispositionOffer={renderOptions.props?.dispositionOffer ?? mockDispositionOffer}
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

  it('displays the values of the Offer in the details', async () => {
    const { queryByTestId } = await setup({});

    expect(queryByTestId('offer[0].offerStatusTypeCode')).toHaveTextContent('Open');
    expect(queryByTestId('offer[0].offerName')).toHaveTextContent('TEST OFFER NAME');
    expect(queryByTestId('offer[0].offerDate')).toHaveTextContent('DEV 25, 2023');
    expect(queryByTestId('offer[0].offerExpiryDate')).toHaveTextContent('DEV 25, 2024');
    expect(queryByTestId('offer[0].offerPrice')).toHaveTextContent('$1,500,000.99');
    expect(queryByTestId('offer[0].notes')).toHaveTextContent('MY OFFER NOTES');
  });
});
