import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { render, RenderOptions } from '@/utils/test-utils';

import {
  AcquisitionSearchResults,
  IAcquisitionSearchResultsProps,
} from './AcquisitionSearchResults';
import { AcquisitionSearchResultModel } from './models';

const setSort = jest.fn();

// render component under test
const setup = (
  renderOptions: RenderOptions & Partial<IAcquisitionSearchResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<AcquisitionSearchResults results={results ?? []} setSort={setSort} />, {
    ...rest,
  });
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  return {
    ...utils,
    tableRows,
  };
};

const mockResults: Api_AcquisitionFile[] = [];

describe('Acquisition Search Results Table', () => {
  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({
      results: mockResults.map(a => AcquisitionSearchResultModel.fromApi(a)),
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

  it('displays historical file #', async () => {
    const { getByText, findAllByText } = setup({ results: [] });

    await findAllByText('No matching results can be found. Try widening your search criteria.');
    expect(getByText('Historical file #')).toBeVisible();
  });
});
