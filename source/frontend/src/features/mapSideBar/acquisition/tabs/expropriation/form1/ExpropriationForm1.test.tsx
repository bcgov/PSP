import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { getMockExpropriationFile } from '@/mocks/index.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ExpropriationForm1, { IExpropriationForm1Props } from './ExpropriationForm1';

// mock auth library
jest.mock('@react-keycloak/web');

jest.mock('@/hooks/pims-api/useApiContacts');
const getContacts = jest.fn();
(useApiContacts as jest.Mock).mockReturnValue({
  getContacts,
});

const onGenerate = jest.fn();
const onError = jest.fn();

describe('Expropriation Form 1', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationForm1Props> } = {},
  ) => {
    const utils = render(
      <ExpropriationForm1
        {...renderOptions.props}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? getMockExpropriationFile()}
        onGenerate={onGenerate}
        onError={onError}
      ></ExpropriationForm1>,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
      getNatureOfInterest: () =>
        utils.container.querySelector(`input[name="landInterest"]`) as HTMLInputElement,
      getPurpose: () => utils.container.querySelector(`input[name="purpose"]`) as HTMLInputElement,
      getContactManagerSearchButton: () => utils.getByTestId('search'),
    };
  };

  beforeEach(() => {
    const organization = getMockContactOrganizationWithOnePerson();
    getContacts.mockResolvedValue({
      data: {
        items: [organization],
        quantity: 1,
        total: 1,
        page: 1,
        pageIndex: 0,
      },
    });
  });

  afterEach(() => jest.clearAllMocks());

  it('matches snapshot', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('validates form values before generating', async () => {
    const { getByText } = await setup();
    await act(() => userEvent.click(getByText('Generate')));

    expect(getByText('Expropriation authority is required')).toBeInTheDocument();
    expect(getByText('At lease one impacted property is required')).toBeInTheDocument();
  });

  it(`submits the form when Generate button is clicked`, async () => {
    const { getByText, getByTestId, getByTitle, getNatureOfInterest, getPurpose } = await setup();

    // pick an organization from contact manager
    await act(() => userEvent.click(getByTitle('Select Contact')));
    await act(() => userEvent.click(getByTestId('selectrow-O3')));
    await act(() => userEvent.click(getByText('Select')));

    // fill other form fields
    await act(() => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getNatureOfInterest(), 'foo'));
    await act(async () => userEvent.paste(getPurpose(), 'bar'));

    await act(() => userEvent.click(getByText('Generate')));

    expect(onGenerate).toBeCalled();
    expect(onError).not.toBeCalled();
  });

  it(`clears the form when Cancel button is clicked`, async () => {
    const { getByText, getByTestId, getNatureOfInterest, getPurpose } = await setup();

    await act(() => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getNatureOfInterest(), 'foo'));
    await act(async () => userEvent.paste(getPurpose(), 'bar'));

    expect(getByTestId('selectrow-1')).toBeChecked();
    expect(getNatureOfInterest()).toHaveValue();
    expect(getPurpose()).toHaveValue();

    await act(() => userEvent.click(getByText('Cancel')));

    expect(getByTestId('selectrow-1')).not.toBeChecked();
    expect(getNatureOfInterest()).not.toHaveValue();
    expect(getPurpose()).not.toHaveValue();
  });

  it(`calls onError callback when generate endpoint fails`, async () => {
    const error = new Error('Network error');
    onGenerate.mockRejectedValueOnce(error);
    const { getByText, getByTestId, getByTitle, getNatureOfInterest, getPurpose } = await setup();

    // pick an organization from contact manager
    await act(() => userEvent.click(getByTitle('Select Contact')));
    await act(() => userEvent.click(getByTestId('selectrow-O3')));
    await act(() => userEvent.click(getByText('Select')));

    // fill other form fields
    await act(() => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getNatureOfInterest(), 'foo'));
    await act(async () => userEvent.paste(getPurpose(), 'bar'));

    await act(() => userEvent.click(getByText('Generate')));

    expect(onGenerate).toBeCalled();
    expect(onError).toBeCalledWith(error);
  });
});
