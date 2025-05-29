import Claims from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { getMockApiNote, mockNotesResponse } from '@/mocks/noteResponses.mock';
import {
  act,
  getMockRepositoryObj,
  render,
  RenderOptions,
  screen,
  userEvent,
  within,
} from '@/utils/test-utils';

import NoteListContainer, { INoteListContainerProps } from './NoteListContainer';
import { NoteListView } from './NoteListView';

vi.mock('@/hooks/repositories/useNoteRepository');
const mockGetAllNotesApi = getMockRepositoryObj([]);
const mockGetNoteApi = getMockRepositoryObj();
const mockAddNoteApi = getMockRepositoryObj();
const mockUpdateNoteApi = getMockRepositoryObj();
const mockDeleteNoteApi = getMockRepositoryObj();

vi.mock('@/hooks/pims-api/useApiUsers');

const onSuccess = vi.fn();

describe('Note List Container', () => {
  const setup = async (renderOptions?: RenderOptions & Partial<INoteListContainerProps>) => {
    // render component under test
    const rendered = render(
      <NoteListContainer
        type={renderOptions?.type ?? NoteTypes.Acquisition_File}
        entityId={renderOptions?.entityId ?? 1}
        onSuccess={renderOptions?.onSuccess ?? onSuccess}
        NoteListView={NoteListView}
      />,
      {
        ...renderOptions,
        claims: renderOptions?.claims ?? [
          Claims.NOTE_VIEW,
          Claims.NOTE_EDIT,
          Claims.NOTE_ADD,
          Claims.NOTE_DELETE,
        ],
      },
    );

    const getTableRows = () => {
      return document.querySelectorAll<HTMLDivElement>('.table .tbody .tr-wrapper');
    };

    await act(async () => {});

    return {
      ...rendered,
      getTableRows,
      getTableCell: (row = 0, col = 0) => {
        const rows = getTableRows();
        const cells = within(rows[row]).getAllByRole('cell');
        if (col < cells.length) {
          return cells[col];
        }
        return null;
      },
    };
  };

  beforeEach(() => {
    vi.mocked(useNoteRepository, { partial: true }).mockReturnValue({
      getAllNotes: mockGetAllNotesApi,
      getNote: mockGetNoteApi,
      addNote: mockAddNoteApi,
      updateNote: mockUpdateNoteApi,
      deleteNote: mockDeleteNoteApi,
    });
    vi.mocked(useApiUsers, { partial: true }).mockReturnValue({
      getUserInfo: vi.fn().mockResolvedValue({}),
    });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('should call the API Endpoint with given type', async () => {
    await setup({ type: NoteTypes.Acquisition_File, entityId: 1 });
    expect(mockGetAllNotesApi.execute).toHaveBeenCalledWith(NoteTypes.Acquisition_File, 1);
  });

  it('should have the Notes header in the component', async () => {
    await setup({ type: NoteTypes.Acquisition_File, entityId: 1 });
    expect(await screen.findByText(`Notes`)).toBeInTheDocument();
  });

  it('should display notes by default in descending order of created date', async () => {
    vi.mocked(useNoteRepository, { partial: true }).mockReturnValue({
      getAllNotes: getMockRepositoryObj([...mockNotesResponse()]),
      addNote: mockAddNoteApi,
      updateNote: mockUpdateNoteApi,
      deleteNote: mockDeleteNoteApi,
    });

    const { getTableRows, getTableCell, debug } = await setup({
      type: NoteTypes.Acquisition_File,
      entityId: 1,
    });

    expect(getTableRows()).toHaveLength(4);
    expect(getTableCell(0, 0)).toHaveTextContent('Note 4');
    expect(getTableCell(1, 0)).toHaveTextContent('Note 3');
    expect(getTableCell(2, 0)).toHaveTextContent('Note 2');
    expect(getTableCell(3, 0)).toHaveTextContent('Note 1');
  });

  it('shows the modal when onAdd is triggered', async () => {
    await setup();

    const addButton = screen.getByText(/Add a Note/i);
    expect(addButton).toBeInTheDocument();
    await act(async () => userEvent.click(addButton));

    expect(screen.getByLabelText('Type a note:')).toBeVisible();
  });

  it('dismisses the popup and does not call the API when the user cancels', async () => {
    await setup();

    const addButton = screen.getByText(/Add a Note/i);
    expect(addButton).toBeInTheDocument();
    await act(async () => userEvent.click(addButton));

    expect(screen.getByLabelText('Type a note:')).toBeVisible();
    const cancel = await screen.findByTitle('cancel-modal');
    expect(cancel).toBeVisible();
    await act(async () => userEvent.click(cancel));

    expect(onSuccess).not.toHaveBeenCalled();
  });

  it('calls onSuccess when a note is added successfully', async () => {
    mockAddNoteApi.execute.mockResolvedValueOnce(getMockApiNote());

    await setup();

    const addButton = screen.getByText(/Add a Note/i);
    expect(addButton).toBeInTheDocument();
    await act(async () => userEvent.click(addButton));

    const noteInput = screen.getByLabelText('Type a note:');
    expect(noteInput).toBeVisible();
    await act(async () => {
      userEvent.paste(noteInput, 'Lorem Ipsum note');
    });

    const ok = await screen.findByTitle('ok-modal');
    expect(ok).toBeVisible();
    await act(async () => userEvent.click(ok));

    expect(onSuccess).toHaveBeenCalled();
  });

  it('populates the modal with existing values', async () => {
    const apiNote = getMockApiNote();
    vi.mocked(useNoteRepository, { partial: true }).mockReturnValue({
      getAllNotes: getMockRepositoryObj([apiNote]),
      addNote: mockAddNoteApi,
      updateNote: mockUpdateNoteApi,
      deleteNote: mockDeleteNoteApi,
      getNote: getMockRepositoryObj(apiNote),
    });

    await setup();

    expect(await screen.findByText(apiNote.note)).toBeVisible();

    const viewButton = await screen.findByTitle(/View Note/i);
    expect(viewButton).toBeVisible();
    await act(async () => userEvent.click(viewButton));

    const noteField: HTMLTextAreaElement = await screen.findByTitle('Note');
    expect(noteField).toBeVisible();
    expect(noteField).toBeInstanceOf(HTMLTextAreaElement);
    expect(noteField).toHaveAttribute('readonly');
    expect(noteField).toHaveTextContent(apiNote.note);
  });

  it('shows a confirmation popup when onDelete is triggered', async () => {
    const apiNote = getMockApiNote();
    vi.mocked(useNoteRepository, { partial: true }).mockReturnValue({
      getAllNotes: getMockRepositoryObj([apiNote]),
      addNote: mockAddNoteApi,
      updateNote: mockUpdateNoteApi,
      deleteNote: mockDeleteNoteApi,
      getNote: getMockRepositoryObj(apiNote),
    });

    await setup();

    expect(await screen.findByText(apiNote.note)).toBeVisible();

    const trashcan = await screen.findByTitle(/Delete Note/i);
    expect(trashcan).toBeVisible();
    await act(async () => userEvent.click(trashcan));

    expect(await screen.findByText('Are you sure you want to delete this note?')).toBeVisible();
  });

  it('calls the API when the user confirms the removal', async () => {
    const apiNote = getMockApiNote();
    vi.mocked(useNoteRepository, { partial: true }).mockReturnValue({
      getAllNotes: getMockRepositoryObj([apiNote]),
      addNote: mockAddNoteApi,
      updateNote: mockUpdateNoteApi,
      deleteNote: mockDeleteNoteApi,
      getNote: getMockRepositoryObj(apiNote),
    });

    await setup();

    expect(await screen.findByText(apiNote.note)).toBeVisible();

    const trashcan = await screen.findByTitle(/Delete Note/i);
    expect(trashcan).toBeVisible();
    await act(async () => userEvent.click(trashcan));

    expect(await screen.findByText('Are you sure you want to delete this note?')).toBeVisible();
    await act(async () => userEvent.click(screen.getByTitle('ok-modal')));

    expect(mockDeleteNoteApi.execute).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
  });
});
