import { noop } from 'lodash';
import { Api_Note } from 'models/api/Note';
import { render, RenderOptions } from 'utils/test-utils';

import { INoteResultProps, NoteResults } from './NoteResults';

const setSort = jest.fn();

// render component under test
const setup = (renderOptions: RenderOptions & Partial<INoteResultProps> = { results: [] }) => {
  const { results, ...rest } = renderOptions;
  const utils = render(
    <NoteResults sort={{}} results={results ?? []} setSort={setSort} onDelete={noop} />,
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

const mockResults: Api_Note[] = [
  { note: 'Note 1', appCreateTimestamp: '10-Jan-2022', appLastUpdateUserid: 'test user1', id: 1 },
  { note: 'Note 2', appCreateTimestamp: '10-Jan-2022', appLastUpdateUserid: 'test user2', id: 2 },
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
    const toasts = await findAllByText('No matching Notes found');
    expect(toasts[0]).toBeVisible();
  });
});
