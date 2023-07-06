import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { getMockExpropriationFile } from '@/mocks/index.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ExpropriationForm5, { IExpropriationForm5Props } from './ExpropriationForm5';

// mock auth library
jest.mock('@react-keycloak/web');

jest.mock('@/hooks/pims-api/useApiContacts');
const getContacts = jest.fn();
(useApiContacts as jest.Mock).mockReturnValue({
  getContacts,
});

const onGenerate = jest.fn();

describe('Expropriation Form 1', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationForm5Props> } = {},
  ) => {
    const utils = render(
      <ExpropriationForm5
        {...renderOptions.props}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? getMockExpropriationFile()}
        onGenerate={onGenerate}
      ></ExpropriationForm5>,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
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
    const { getByText, getByTestId, getByTitle } = await setup();

    // pick an organization from contact manager
    await act(() => userEvent.click(getByTitle('Select Contact')));
    await act(() => userEvent.click(getByTestId('selectrow-O3')));
    await act(() => userEvent.click(getByText('Select')));

    // fill other form fields
    await act(() => userEvent.click(getByTestId('selectrow-1')));
    await act(() => userEvent.click(getByText('Generate')));

    expect(onGenerate).toBeCalled();
  });

  it(`clears the form when Cancel button is clicked`, async () => {
    const { getByText, getByTestId } = await setup();

    await act(() => userEvent.click(getByTestId('selectrow-1')));
    expect(getByTestId('selectrow-1')).toBeChecked();

    await act(() => userEvent.click(getByText('Cancel')));
    expect(getByTestId('selectrow-1')).not.toBeChecked();
  });
});
