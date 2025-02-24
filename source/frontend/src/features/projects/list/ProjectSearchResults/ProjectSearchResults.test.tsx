import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { render, RenderOptions, screen } from '@/utils/test-utils';

import { ProjectSearchResultModel } from './models';
import { IProjectSearchResultsProps, ProjectSearchResults } from './ProjectSearchResults';

const setSort = vi.fn();

// render component under test
const setup = (
  renderOptions: RenderOptions & Partial<IProjectSearchResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<ProjectSearchResults results={results ?? []} setSort={setSort} />, {
    ...rest,
  });
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  return {
    ...utils,
    tableRows,
  };
};

const mockResults: ApiGen_Concepts_Project[] = [];

describe('Project Search Results Table', () => {
  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({
      results: mockResults.map(a => ProjectSearchResultModel.fromApi(a)),
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    const toasts = await findAllByText(
      'No matching results can be found. Try widening your search criteria.',
    );

    expect(tableRows.length).toBe(0);
    expect(toasts[0]).toBeVisible();
  });

  it('displays the correct date when project is created after 5pm PST', async () => {
    setup({
      results: [
        ProjectSearchResultModel.fromApi({
          ...mockProjectGetResponse(),
          appCreateTimestamp: '2025-02-12T00:59:37.953',
          appLastUpdateTimestamp: '2025-02-12T00:59:37.953',
        }),
      ],
    });
    expect(await screen.findByText('Feb 11, 2025')).toBeVisible();
  });
});
