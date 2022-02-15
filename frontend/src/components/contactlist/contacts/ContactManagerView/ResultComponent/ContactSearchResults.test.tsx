import userEvent from '@testing-library/user-event';
import { Claims } from 'constants/claims';
import { IContactSearchResult } from 'interfaces';
import { noop } from 'lodash';
import React from 'react';
import { act, mockKeycloak, render, RenderOptions } from 'utils/test-utils';

import { ContactSearchResults, IContactSearchResultsProps } from './ContactSearchResults';

// mock auth library
jest.mock('@react-keycloak/web');

const setSort = jest.fn();

// render component under test
const setup = (
  renderOptions: Partial<RenderOptions & IContactSearchResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(
    <ContactSearchResults
      setSelectedRows={noop}
      selectedRows={[]}
      results={results ?? []}
      setSort={setSort}
    />,
    {
      ...rest,
    },
  );
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  // long css selector to: get the FIRST header or table, then get the SVG up/down sort buttons
  const sortButtons = utils.container.querySelector(
    '.table .thead .tr .sortable-column div',
  ) as HTMLElement;
  return {
    ...utils,
    tableRows,
    sortButtons,
  };
};

const defaultSearchResult: IContactSearchResult = {
  id: 1,
  summary: 'summary',
  mailingAddress: '123 mock st',
  surname: 'last',
  firstName: 'first',
  organizationName: 'organizationName',
  email: 'email',
  municipalityName: 'city',
  provinceState: 'province',
  isDisabled: false,
  provinceStateId: 0,
};

const mockResults: IContactSearchResult[] = [
  {
    ...defaultSearchResult,
    personId: 1,
    id: 1,
  },
  {
    ...defaultSearchResult,
    id: 2,
  },
  {
    ...defaultSearchResult,
    id: 3,
    isDisabled: true,
  },
];

describe('Contact Search Results Table', () => {
  beforeEach(() => {
    setSort.mockClear();
    mockKeycloak({ claims: [Claims.CONTACT_VIEW] });
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({ results: mockResults });
    expect(asFragment()).toMatchSnapshot();
  });

  it('shows table with search results', async () => {
    const { tableRows, findAllByText } = setup({ results: mockResults });

    expect(tableRows.length).toBe(3);
    expect((await findAllByText(/123 mock st/i))[0]).toBeInTheDocument();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    expect(tableRows.length).toBe(0);
    const toasts = await findAllByText('No Contacts match the search criteria');
    expect(toasts[0]).toBeVisible();
  });

  it('sorts table when sort buttons are clicked', async () => {
    const { sortButtons } = setup({ results: mockResults });
    // click on sort buttons
    await act(async () => userEvent.click(sortButtons));
    // should be sorted in ascending order
    expect(setSort).toHaveBeenCalledWith({ summary: 'asc' });
  });

  describe('when user has CONTACT_EDIT claim', () => {
    it('shows edit contact button on each table row', async () => {
      mockKeycloak({ claims: [Claims.CONTACT_VIEW, Claims.CONTACT_EDIT] });
      const { tableRows, findAllByTitle } = setup({ results: mockResults });
      const editButtons = await findAllByTitle(/edit contact/i);

      // all rows should show edit buttons
      expect(editButtons).toHaveLength(tableRows.length);
      expect(editButtons[0]).toBeInTheDocument();
    });
  });

  describe(`when user doesn't have CONTACT_EDIT claim`, () => {
    it('does not show the edit button on table rows', () => {
      const { queryAllByTitle } = setup({ results: mockResults });
      const editButtons = queryAllByTitle(/edit contact/i);

      // edit buttons should not be rendered
      expect(editButtons).toHaveLength(0);
    });
  });
});
