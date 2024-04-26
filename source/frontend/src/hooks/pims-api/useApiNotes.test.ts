import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { NoteTypes } from '@/constants/noteTypes';
import { mockEntityNote, mockNotesResponse } from '@/mocks/noteResponses.mock';

import { useApiNotes } from './useApiNotes';

const mockAxios = new MockAdapter(axios);

describe('useApiNotes hook', () => {
  beforeEach(() => {
    mockAxios.onGet(`/notes/acquisition_file/1`).reply(200, mockNotesResponse());
    mockAxios.onDelete(`/notes/1/acquisition_file`).reply(200, true);
    mockAxios.onPost(`/notes/activity`).reply(200, mockEntityNote(1));
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiNotes);
    return result.current;
  };

  it('Gets a list of Notes', async () => {
    const { getNotes } = setup();
    const response = await getNotes(NoteTypes.Acquisition_File, 1);

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
    const response = await deleteNote(NoteTypes.Acquisition_File, 1);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(true);
  });
});
