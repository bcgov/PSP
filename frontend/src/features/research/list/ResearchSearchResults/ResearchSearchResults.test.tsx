import { IResearchSearchResult } from 'interfaces/IResearchSearchResult';
import { render, RenderOptions } from 'utils/test-utils';

import { IResearchSearchResultsProps, ResearchSearchResults } from './ResearchSearchResults';

const setSort = jest.fn();

// render component under test
const setup = (renderOptions: RenderOptions & IResearchSearchResultsProps = { results: [] }) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<ResearchSearchResults results={results} setSort={setSort} />, { ...rest });
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

const mockResults: IResearchSearchResult[] = [];

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
