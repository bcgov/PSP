import { Claims } from '@/constants/claims';
import { getMockApiNote, mockNotesResponse } from '@/mocks/noteResponses.mock';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { INoteResultProps, NoteResults } from './NoteResults';

const setSort = vi.fn();
const onShowDetails = vi.fn();
const onDelete = vi.fn();

// mock auth library

// render component under test
const setup = (renderOptions: RenderOptions & Partial<INoteResultProps> = { results: [] }) => {
  const { results, ...rest } = renderOptions;
  const utils = render(
    <NoteResults
      sort={{}}
      results={results ?? []}
      setSort={setSort}
      onDelete={onDelete}
      onShowDetails={onShowDetails}
    />,
    {
      ...rest,
      claims: rest.claims ?? [Claims.NOTE_VIEW],
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

  it('displays the eye icon-button to open note details popup', async () => {
    setup({
      results: [getMockApiNote()],
    });

    const viewButton = await screen.findByTitle(/View Note/i);
    expect(viewButton).toBeVisible();
    await act(async () => userEvent.click(viewButton));
    expect(onShowDetails).toHaveBeenCalled();
  });

  it('displays the trashcan button to delete note, when the user has permissions', async () => {
    setup({
      results: [getMockApiNote()],
      claims: [Claims.NOTE_VIEW, Claims.NOTE_DELETE],
    });

    const deleteButton = await screen.findByTitle(/Delete Note/i);
    expect(deleteButton).toBeVisible();
    await act(async () => userEvent.click(deleteButton));
    expect(onDelete).toHaveBeenCalled();
  });

  it('displays the correct date when note is created after 5pm PST', async () => {
    setup({
      results: [{ ...getMockApiNote(), appCreateTimestamp: '2025-02-12T00:59:37.953' }],
    });
    expect(await screen.findByText('Feb 11, 2025')).toBeVisible();
  });
});
