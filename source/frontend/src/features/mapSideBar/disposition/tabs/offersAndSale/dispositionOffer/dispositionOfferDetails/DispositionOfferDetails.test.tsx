import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { mockDispositionFileOfferApi } from '@/mocks/dispositionFiles.mock';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import DispositionOfferDetails, { IDispositionOfferDetailsProps } from './DispositionOfferDetails';

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

const mockDispositionOffer = mockDispositionFileOfferApi(100, 1);

const onDelete = jest.fn();

describe('Disposition Offer Detail View component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IDispositionOfferDetailsProps> },
  ) => {
    const utils = render(
      <DispositionOfferDetails
        {...renderOptions.props}
        index={0}
        dispositionOffer={renderOptions.props?.dispositionOffer ?? mockDispositionOffer}
        onDelete={onDelete}
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

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('displays the values of the Offer in the details', async () => {
    const { queryByTestId } = await setup({});

    expect(queryByTestId('offer[0].offerStatusTypeCode')).toHaveTextContent('Open');
    expect(queryByTestId('offer[0].offerName')).toHaveTextContent('TEST OFFER NAME');
    expect(queryByTestId('offer[0].offerDate')).toHaveTextContent('Dec 25, 2023');
    expect(queryByTestId('offer[0].offerExpiryDate')).toHaveTextContent('Dec 25, 2024');
    expect(queryByTestId('offer[0].offerPrice')).toHaveTextContent('$1,500,000.99');
    expect(queryByTestId('offer[0].notes')).toHaveTextContent('MY OFFER NOTES');
  });

  it('renders the edit and delete button for users with edit permissions', async () => {
    const { queryByTestId } = await setup({ claims: [Claims.DISPOSITION_EDIT] });

    expect(queryByTestId('Offer[0].edit-btn')).toBeInTheDocument();
    expect(queryByTestId('Offer[0].delete-btn')).toBeInTheDocument();
  });

  it('does not renders the edit and delete button for users with view permissions', async () => {
    const { queryByTestId } = await setup({ claims: [Claims.DISPOSITION_VIEW] });

    expect(queryByTestId('Offer[0].edit-btn')).not.toBeInTheDocument();
    expect(queryByTestId('Offer[0].delete-btn')).not.toBeInTheDocument();
  });

  it('calls the delete for the offer', async () => {
    const { getByTestId, findByText, getByTitle } = await setup({
      claims: [Claims.DISPOSITION_EDIT],
    });

    const deleteButton = getByTestId('Offer[0].delete-btn');
    act(() => userEvent.click(deleteButton));

    expect(onDelete).not.toHaveBeenCalled();
    expect(await findByText(/You have selected to delete this offer./i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('ok-modal')));
    expect(onDelete).toHaveBeenCalledWith(100);
  });

  it('calls iedit page with param', async () => {
    const { getByTestId } = await setup({
      claims: [Claims.DISPOSITION_EDIT],
    });

    const editButton = getByTestId('Offer[0].edit-btn');
    act(() => userEvent.click(editButton));

    expect(history.location.pathname).toBe('//offers/100/update');
  });
});
