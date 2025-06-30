import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { getMockExpropriationFile } from '@/mocks/index.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ExpropriationForm5, { IExpropriationForm5Props } from './ExpropriationForm5';

// mock auth library

vi.mock('@/hooks/pims-api/useApiContacts');
const getContacts = vi.fn();
vi.mocked(useApiContacts).mockReturnValue({
  getContacts,
} as unknown as ReturnType<typeof useApiContacts>);

const onGenerate = vi.fn();
const onError = vi.fn();

describe('Expropriation Form 1', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationForm5Props> } = {},
  ) => {
    const utils = render(
      <ExpropriationForm5
        {...renderOptions.props}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? getMockExpropriationFile()}
        onGenerate={onGenerate}
        formikRef={renderOptions.props?.formikRef ?? null}
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

  afterEach(() => vi.clearAllMocks());

  it('matches snapshot', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it(`clears the form when Cancel button is clicked`, async () => {
    const { getByText, getByTestId } = await setup();

    await act(async () => userEvent.click(getByTestId('selectrow-1')));
    expect(getByTestId('selectrow-1')).toBeChecked();

    await act(async () => userEvent.click(getByText('Clear Form')));
    expect(getByTestId('selectrow-1')).not.toBeChecked();
  });

  // it(`submits the form 5 when Generate button is clicked`, async () => {
  //   const { getByText, getByTestId, getSelectContactForm5Button } = await setup({});
  //   // pick an organization from contact manager
  //   await act(async () => userEvent.click(getSelectContactForm5Button()));
  //   await act(async () => userEvent.click(getByTestId('selectrow-O3')));
  //   await act(async () => userEvent.click(getByText('Select')));

  //   // fill other form fields
  //   await act(async () => userEvent.click(getByTestId('selectrow-1')));
  //   await act(async () => userEvent.click(getByText(/Generate Form 5/i)));

  //   expect(onGenerate).toHaveBeenCalled();
  //   expect(onError).not.toHaveBeenCalled();
  // });

  // it(`calls onError callback when generate endpoint fails`, async () => {
  //   const error = new Error('Network error');
  //   onGenerate.mockRejectedValueOnce(error);
  //   const { getByText, getByTestId, getByTitle } = await setup();

  //   // pick an organization from contact manager
  //   await act(async () => userEvent.click(getByTitle('Select Contact')));
  //   await act(async () => userEvent.click(getByTestId('selectrow-O3')));
  //   await act(async () => userEvent.click(getByText('Select')));

  //   // fill other form fields
  //   await act(async () => userEvent.click(getByTestId('selectrow-1')));
  //   await act(async () => userEvent.click(getByText(/Generate Form 5/i)));

  //   expect(onGenerate).toHaveBeenCalled();
  //   expect(onError).toHaveBeenCalledWith(error);
  // });
});
