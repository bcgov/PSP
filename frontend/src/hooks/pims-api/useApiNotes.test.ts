import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { NoteTypes } from 'constants/noteTypes';
import { mockEntityNote, mockNotesResponse } from 'mocks/mockNoteResponses';

import { useApiNotes } from './useApiNotes';

const mockAxios = new MockAdapter(axios);

describe('useApiNotes hook', () => {
  beforeEach(() => {
    mockAxios.onGet(`/notes/file/owner/1`).reply(200, mockNotesResponse());
    mockAxios.onDelete(`/notes/file/1`).reply(200, true);
    mockAxios.onPost(`/notes/activity`).reply(200, mockEntityNote(1));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiNotes);
    return result.current;
  };

  it('Gets a list of Notes', async () => {
    const { getNotes } = setup();
    const response = await getNotes(NoteTypes.File, 1);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockNotesResponse());
  });

  it('Creates a Note', async () => {
    const { postNote } = setup();
    const response = await postNote(NoteTypes.Activity, mockEntityNote());

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockEntityNote(1));
  });

  it('Deletes a Note', async () => {
    const { deleteNote } = setup();
    const response = await deleteNote(NoteTypes.File, 1);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(true);
  });
});
