import { IResearchSearchResult } from '@/interfaces/IResearchSearchResult';
import { render, RenderOptions } from '@/utils/test-utils';

import { IResearchSearchResultsProps, ResearchSearchResults } from './ResearchSearchResults';

const setSort = vi.fn();

// render component under test
const setup = (
  renderOptions: RenderOptions & Partial<IResearchSearchResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<ResearchSearchResults results={results ?? []} setSort={setSort} />, {
    ...rest,
  });
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  return {
    ...utils,
    tableRows,
  };
};

const mockResults: IResearchSearchResult[] = [];

describe('Research Search Results Table', () => {
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
    const toasts = await findAllByText('No matching Research Files found');
    expect(toasts[0]).toBeVisible();
  });
});
