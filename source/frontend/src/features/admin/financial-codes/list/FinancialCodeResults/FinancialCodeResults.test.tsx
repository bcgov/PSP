import { Roles } from '@/constants/index';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { render, RenderOptions } from '@/utils/test-utils';

import { FinancialCodeResults, IFinancialCodeResultsProps } from './FinancialCodeResults';

// mock auth library

const setSort = vi.fn();

// render component under test
const setup = (
  renderOptions: RenderOptions & Partial<IFinancialCodeResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(
    <FinancialCodeResults sort={{}} results={results ?? []} setSort={setSort} />,
    {
      ...rest,
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    },
  );
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  return {
    ...utils,
    tableRows,
  };
};

const mockResults: ApiGen_Concepts_FinancialCode[] = [];

describe('Financial Code Results Table', () => {
  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({ results: mockResults });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findByText } = setup({ results: [] });

    const errorMessage = await findByText(
      'No matching results can be found. Try widening your search criteria.',
    );

    expect(tableRows.length).toBe(0);
    expect(errorMessage).toBeVisible();
  });
});
