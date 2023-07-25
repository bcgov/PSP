import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { getMockExpropriationFile } from '@/mocks/index.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ExpropriationForm9, { IExpropriationForm9Props } from './ExpropriationForm9';

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
    renderOptions: RenderOptions & { props?: Partial<IExpropriationForm9Props> } = {},
  ) => {
    const utils = render(
      <ExpropriationForm9
        {...renderOptions.props}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? getMockExpropriationFile()}
        onGenerate={onGenerate}
        onError={onError}
      ></ExpropriationForm9>,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
      getRegisteredPlanNumbers: () =>
        utils.container.querySelector(`input[name="registeredPlanNumbers"]`) as HTMLInputElement,
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
    const { getByText, getByTestId, getByTitle, getRegisteredPlanNumbers } = await setup();

    // pick an organization from contact manager
    await act(() => userEvent.click(getByTitle('Select Contact')));
    await act(() => userEvent.click(getByTestId('selectrow-O3')));
    await act(() => userEvent.click(getByText('Select')));

    // fill other form fields
    await act(() => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getRegisteredPlanNumbers(), 'testing'));

    await act(() => userEvent.click(getByText('Generate')));

    expect(onGenerate).toBeCalled();
    expect(onError).not.toBeCalled();
  });

  it(`clears the form when Cancel button is clicked`, async () => {
    const { getByText, getByTestId, getRegisteredPlanNumbers } = await setup();

    await act(() => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getRegisteredPlanNumbers(), 'testing'));

    expect(getByTestId('selectrow-1')).toBeChecked();
    expect(getRegisteredPlanNumbers()).toHaveValue();

    await act(() => userEvent.click(getByText('Cancel')));

    expect(getByTestId('selectrow-1')).not.toBeChecked();
    expect(getRegisteredPlanNumbers()).not.toHaveValue();
  });

  it(`calls onError callback when generate endpoint fails`, async () => {
    const error = new Error('Network error');
    onGenerate.mockRejectedValueOnce(error);
    const { getByText, getByTestId, getByTitle, getRegisteredPlanNumbers } = await setup();

    // pick an organization from contact manager
    await act(() => userEvent.click(getByTitle('Select Contact')));
    await act(() => userEvent.click(getByTestId('selectrow-O3')));
    await act(() => userEvent.click(getByText('Select')));

    // fill other form fields
    await act(() => userEvent.click(getByTestId('selectrow-1')));
    await act(async () => userEvent.paste(getRegisteredPlanNumbers(), 'testing'));

    await act(() => userEvent.click(getByText('Generate')));

    expect(onGenerate).toBeCalled();
    expect(onError).toBeCalledWith(error);
  });
});
