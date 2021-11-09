import userEvent from '@testing-library/user-event';
import { IContactSearchResult } from 'interfaces';
import React from 'react';
import { act, render, RenderOptions } from 'utils/test-utils';

import { ContactSearchResults, IContactSearchResultsProps } from './ContactSearchResults';

const setSort = jest.fn();

// render component under test
const setup = (renderOptions: RenderOptions & IContactSearchResultsProps = { results: [] }) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<ContactSearchResults results={results} setSort={setSort} />, { ...rest });
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
  municipality: 'city',
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
});
