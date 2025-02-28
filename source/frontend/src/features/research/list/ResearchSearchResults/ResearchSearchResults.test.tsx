import { Claims } from '@/constants';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { render, RenderOptions, screen } from '@/utils/test-utils';

import { IResearchSearchResultsProps, ResearchSearchResults } from './ResearchSearchResults';

const setSort = vi.fn();

// render component under test
const setup = (
  renderOptions: RenderOptions & Partial<IResearchSearchResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<ResearchSearchResults results={results ?? []} setSort={setSort} />, {
    ...rest,
    claims: rest.claims ?? [Claims.RESEARCH_VIEW],
  });
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  return {
    ...utils,
    tableRows,
  };
};

describe('Research Search Results Table', () => {
  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({ results: [getMockResearchFile()] });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows } = setup({ results: [] });

    expect(tableRows.length).toBe(0);
    const toasts = await screen.findAllByText('No matching Research Files found');
    expect(toasts[0]).toBeVisible();
  });

  it('displays the correct date when research file is created after 5pm PST', async () => {
    setup({
      results: [{ ...getMockResearchFile(), appCreateTimestamp: '2025-02-12T00:59:37.953' }],
    });
    expect(await screen.findByText('Feb 11, 2025')).toBeVisible();
  });
});
