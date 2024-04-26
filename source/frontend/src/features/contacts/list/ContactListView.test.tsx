import userEvent from '@testing-library/user-event';

import { defaultFilter } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import { Claims } from '@/constants/claims';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { IContactSearchResult } from '@/interfaces';
import {
  act,
  fillInput,
  mockKeycloak,
  render,
  RenderOptions,
  waitFor,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import { ContactListPage } from './ContactListPage';

// mock auth library

vi.mock('@/hooks/pims-api/useApiContacts');
const getContacts = vi.fn();
vi.mocked(useApiContacts).mockReturnValue({
  getContacts,
} as unknown as ReturnType<typeof useApiContacts>);

// render component under test
const setup = (renderOptions: RenderOptions = {}) => {
  const utils = render(<ContactListPage />, {
    ...renderOptions,
  });
  const searchButton = utils.getByTestId('search');
  return { searchButton, ...utils };
};

const setupMockSearch = (searchResults?: IContactSearchResult[]) => {
  const results = searchResults ?? [];
  const len = results.length;
  getContacts.mockResolvedValue({
    data: {
      items: results,
      quantity: len,
      total: len,
      page: 1,
      pageIndex: 0,
    },
  });
};

const defaultPersonSearchResult: IContactSearchResult = {
  id: '1',
  summary: 'summary',
  mailingAddress: '123 mock st',
  personId: 1,
  surname: 'last',
  firstName: 'first',
  email: 'email',
  municipalityName: 'city',
  provinceState: 'province',
  isDisabled: false,
  provinceStateId: 0,
};

const defaultPagedFilter = {
  ...defaultFilter,
  page: 1,
  quantity: 10,
  sort: undefined,
};

describe('Contact List View', () => {
  beforeEach(() => {
    getContacts.mockClear();
    mockKeycloak({ claims: [Claims.CONTACT_VIEW] });
  });

  it('matches snapshot', async () => {
    setupMockSearch();
    const { asFragment, getByText } = setup();
    await act(async () => {});
    setupMockSearch([defaultPersonSearchResult]);

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('searches by summary', async () => {
    setupMockSearch([defaultPersonSearchResult]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'summary', 'asummary');
    await act(async () => userEvent.click(searchButton));

    expect(getContacts).toHaveBeenCalledWith(
      expect.objectContaining({
        ...defaultPagedFilter,
        summary: 'asummary',
      }),
    );

    expect(await findByText(/123 mock st/i)).toBeInTheDocument();
  });

  it('searches by city/municipality', async () => {
    setupMockSearch([{ ...defaultPersonSearchResult, municipalityName: 'victoria' }]);
    const { container, searchButton, findByText } = setup({});
    fillInput(container, 'municipality', 'victoria');
    await act(async () => userEvent.click(searchButton));

    expect(getContacts).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultPagedFilter, municipality: 'victoria' }),
    );

    expect(await findByText(/victoria/i)).toBeInTheDocument();
  });

  it('searches all by default', async () => {
    setupMockSearch([defaultPersonSearchResult]);
    const { container, searchButton } = setup({});
    const allButton = container.querySelector(`#input-all`);
    allButton && (await act(async () => userEvent.click(allButton)));
    await act(async () => userEvent.click(searchButton));

    expect(getContacts).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultPagedFilter, searchBy: 'all' }),
    );

    expect(allButton).toBeChecked();
  });

  it('searches organizations if radio option selected', async () => {
    setupMockSearch([defaultPersonSearchResult]);
    const { container, searchButton } = setup({});
    const organizationsButton = container.querySelector(`#input-organizations`);
    organizationsButton && (await act(async () => userEvent.click(organizationsButton)));
    await act(async () => userEvent.click(searchButton));

    expect(getContacts).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultPagedFilter, searchBy: 'organizations' }),
    );

    expect(organizationsButton).toBeChecked();
  });

  it('searches persons if radio option selected', async () => {
    setupMockSearch([defaultPersonSearchResult]);
    const { container, searchButton } = setup({});
    const personButton = container.querySelector(`#input-persons`);
    personButton && (await act(async () => userEvent.click(personButton)));
    await act(async () => userEvent.click(searchButton));

    expect(getContacts).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultPagedFilter, searchBy: 'persons' }),
    );

    expect(personButton).toBeChecked();
  });

  it('searches for active contacts by default', async () => {
    setupMockSearch([defaultPersonSearchResult]);
    const { container, searchButton } = setup({});
    const activeCheck = container.querySelector(`#input-activeContactsOnly`);
    await act(async () => userEvent.click(searchButton));

    expect(getContacts).toHaveBeenCalledWith(expect.objectContaining({ ...defaultPagedFilter }));

    expect(activeCheck).toBeChecked();
  });

  it('searches for inactive contacts if checkbox unchecked', async () => {
    setupMockSearch([defaultPersonSearchResult]);
    const { container } = setup({});
    const activeCheck = container.querySelector(`#input-activeContactsOnly`);
    expect(activeCheck).not.toBeNull();
    await act(async () => userEvent.click(activeCheck as Element));

    expect(getContacts).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultPagedFilter, activeContactsOnly: false }),
    );

    expect(activeCheck).not.toBeChecked();
  });

  it('displays an error when no matching records found', async () => {
    setupMockSearch();
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'summary', 'a summary that does not exist');
    await act(async () => userEvent.click(searchButton));

    expect(getContacts).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultPagedFilter, summary: 'a summary that does not exist' }),
    );
    const toasts = await findAllByText('No Contacts match the search criteria');
    expect(toasts[0]).toBeVisible();
  });

  it('displays an error when when Search API is unreachable', async () => {
    // simulate a network error
    getContacts.mockRejectedValue(new Error('network error'));
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'summary', 'a summary resulting in a network error');
    await act(async () => userEvent.click(searchButton));

    expect(getContacts).toHaveBeenCalledWith(
      expect.objectContaining({
        ...defaultFilter,
        summary: 'a summary resulting in a network error',
      }),
    );
    const toasts = await findAllByText('network error');
    expect(toasts[0]).toBeVisible();
  });
});
