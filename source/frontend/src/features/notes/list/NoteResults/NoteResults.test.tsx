import noop from 'lodash/noop';

import { Claims } from '@/constants/claims';
import { mockNotesResponse } from '@/mocks/noteResponses.mock';
import { mockKeycloak, render, RenderOptions } from '@/utils/test-utils';

import { INoteResultProps, NoteResults } from './NoteResults';

const setSort = vi.fn();

// mock auth library

// render component under test
const setup = (renderOptions: RenderOptions & Partial<INoteResultProps> = { results: [] }) => {
  const { results, ...rest } = renderOptions;
  const utils = render(
    <NoteResults
      sort={{}}
      results={results ?? []}
      setSort={setSort}
      onDelete={noop}
      onShowDetails={noop}
    />,
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

describe('Note Results Table', () => {
  beforeEach(() => {
    mockKeycloak({ claims: [Claims.NOTE_DELETE] });
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({ results: mockNotesResponse() });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    expect(tableRows.length).toBe(0);
    const toasts = await findAllByText('No matching Notes found');
    expect(toasts[0]).toBeVisible();
  });
});
