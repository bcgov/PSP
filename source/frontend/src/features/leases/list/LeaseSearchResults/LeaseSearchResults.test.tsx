import { ILeaseSearchResult } from '@/interfaces';
import { render, RenderOptions } from '@/utils/test-utils';

import { ILeaseSearchResultsProps, LeaseSearchResults } from './LeaseSearchResults';

const setSort = jest.fn();

// render component under test
const setup = (
  renderOptions: RenderOptions & Partial<ILeaseSearchResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<LeaseSearchResults results={results ?? []} setSort={setSort} />, {
    ...rest,
  });
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

const mockResults: ILeaseSearchResult[] = [
  {
    id: 1,
    lFileNo: 'L-123-456',
    programName: 'TRAN-IT',
    tenantNames: ['Chester Tester'],
    properties: [{ id: 123, address: '123 mock st', pin: '123' }],
  },
  {
    id: 2,
    lFileNo: 'L-999-888',
    programName: 'TRAN-IT',
    tenantNames: ['Chester Tester'],
    properties: [{ id: 124, address: '456 mock st', pid: '999' }],
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

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    expect(tableRows.length).toBe(0);
    const toasts = await findAllByText('Lease / License details do not exist in PIMS inventory');
    expect(toasts[0]).toBeVisible();
  });
});
