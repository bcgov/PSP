import userEvent from '@testing-library/user-event';
import { ILeaseSearchResult } from 'interfaces';
import { act, render, RenderOptions } from 'utils/test-utils';

import { ILeaseSearchResultsProps, LeaseSearchResults } from './LeaseSearchResults';

const setSort = jest.fn();

// render component under test
const setup = (renderOptions: RenderOptions & ILeaseSearchResultsProps = { results: [] }) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<LeaseSearchResults results={results} setSort={setSort} />, { ...rest });
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  // long css selector to: get the FIRST header or table, then get the SVG up/down sort buttons
  const sortButtons = utils.container.querySelector(
    '.table .thead .tr .th:nth-of-type(1) .sortable-column div',
  ) as HTMLElement;
  return {
    ...utils,
    tableRows,
    sortButtons,
  };
};

const mockResults: ILeaseSearchResult[] = [
  {
    id: 1,
    lFileNo: 'L-123-456',
    address: '123 mock st',
    pidOrPin: '123',
    programName: 'TRAN-IT',
    tenantName: 'Chester Tester',
  },
  {
    id: 2,
    lFileNo: 'L-999-888',
    address: '456 mock st',
    pidOrPin: '999',
    programName: 'TRAN-IT',
    tenantName: 'Chester Tester',
  },
];

describe('Lease Search Results Table', () => {
  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({ results: mockResults });
    expect(asFragment()).toMatchSnapshot();
  });

  it('shows table with search results', async () => {
    const { tableRows, findByText } = setup({ results: mockResults });

    expect(tableRows.length).toBe(2);
    expect(await findByText(/123 mock st/i)).toBeInTheDocument();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    expect(tableRows.length).toBe(0);
    const toasts = await findAllByText('Lease / License details do not exist in PIMS inventory');
    expect(toasts[0]).toBeVisible();
  });

  it('sorts table when sort buttons are clicked', async () => {
    const { sortButtons } = setup({ results: mockResults });
    // click on sort buttons
    await act(async () => userEvent.click(sortButtons));
    // should be sorted in ascending order
    expect(setSort).toHaveBeenCalledWith({ lFileNo: 'asc' });
  });
});
