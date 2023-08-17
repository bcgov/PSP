import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockGetExpropriationPaymentApi } from '@/mocks/ExpropriationPayment.mock';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import ExpropriationForm8Details, {
  IExpropriationForm8DetailsProps,
} from './ExpropriationForm8Details';

const mockFom8Api = mockGetExpropriationPaymentApi(1, 1);
const mockAcquisitionFile = mockAcquisitionFileResponse(1);

const onDelete = jest.fn();
const onGenerate = jest.fn();

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

describe('Form 8 Detail View component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationForm8DetailsProps> },
  ) => {
    const utils = render(
      <ExpropriationForm8Details
        {...renderOptions.props}
        form8Index={0}
        form8={renderOptions.props?.form8 ?? mockFom8Api}
        acquisitionFileNumber={
          renderOptions.props?.acquisitionFileNumber ?? (mockAcquisitionFile.fileNumber as string)
        }
        onDelete={onDelete}
        onGenerate={onGenerate}
      />,
      {
        ...renderOptions,
        history: history,
        claims: renderOptions?.claims ?? [Claims.ACQUISITION_VIEW],
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

  it('displays the Form 8 information in the details', async () => {
    const { queryByTestId, queryByText } = await setup({
      claims: [Claims.ACQUISITION_VIEW],
    });

    expect(queryByTestId('form8[0].payee-name')).toHaveTextContent('John Doe');
    expect(queryByTestId('form8[0].exp-authority')).toHaveTextContent('FORTIS BC');
    expect(queryByTestId('form8[0].description')).toHaveTextContent('MY DESCRIPTION');

    expect(queryByText('Total Amount of Advance Taking')).toBeInTheDocument();
    expect(queryByText('$1,000.00')).toBeInTheDocument();
    expect(queryByText('$50.00')).toBeInTheDocument();
    expect(queryByText('$1,050.00')).toBeInTheDocument();

    expect(queryByText('Market Value')).toBeInTheDocument();
    expect(queryByText('$896,365.23')).toBeInTheDocument();
    expect(queryByText('$44,818.26')).toBeInTheDocument();
    expect(queryByText('$941,183.49')).toBeInTheDocument();

    expect(queryByTestId('form8[0].total-amount')).toHaveTextContent('$942,233.49');
  });

  it('renders the edit and delete button for users with acquisition edit permissions', async () => {
    const { queryByTestId } = await setup({ claims: [Claims.ACQUISITION_EDIT] });

    expect(queryByTestId('form8[0].edit-form8')).toBeInTheDocument();
    expect(queryByTestId('form8[0].delete-form8')).toBeInTheDocument();
  });

  it('does not render the edit and delete button for users without acquisition edit permissions', async () => {
    const { queryByTestId } = await setup({ claims: [Claims.ACQUISITION_VIEW] });

    expect(queryByTestId('form8[0].edit-form8')).not.toBeInTheDocument();
    expect(queryByTestId('form8[0].delete-form8')).not.toBeInTheDocument();
  });

  it('calls the generate form 8', async () => {
    const { getByTestId, queryByTestId } = await setup({ claims: [Claims.ACQUISITION_VIEW] });
    expect(queryByTestId('form8[0].generate-form8')).toBeInTheDocument();

    const generateButton = getByTestId('form8[0].generate-form8');
    await act(() => userEvent.click(generateButton));

    expect(onGenerate).toBeCalledWith(1, '1-12345-01');
  });

  it('calls the delete form 8', async () => {
    const { getByTestId, findByText, getByTitle } = await setup({
      claims: [Claims.ACQUISITION_EDIT],
    });

    const deleteButton = getByTestId('form8[0].delete-form8');
    act(() => userEvent.click(deleteButton));

    expect(onDelete).not.toHaveBeenCalled();
    expect(await findByText(/Do you wish to remove this Form 8./i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('ok-modal')));
    expect(onDelete).toHaveBeenCalledWith(1);
  });

  it('calls iedit page with param', async () => {
    const { getByTestId } = await setup({
      claims: [Claims.ACQUISITION_EDIT],
    });

    const editButton = getByTestId('form8[0].edit-form8');
    act(() => userEvent.click(editButton));

    expect(history.location.pathname).toBe('//1');
  });
});
