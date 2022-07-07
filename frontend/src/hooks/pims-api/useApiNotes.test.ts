import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { NoteTypes } from 'constants/noteTypes';
import { mockNotesResposne } from 'mocks/mockNoteResponses';

import { useApiNotes } from './useApiNotes';

const mockAxios = new MockAdapter(axios);

describe('useApiNotes testing suite', () => {
  beforeEach(() => {
    mockAxios.onGet(`/notes/file`).reply(200, mockNotesResposne);
    mockAxios.onDelete(`/notes/file/1`).reply(200, true);
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiNotes);
    return result.current;
  };

  it('Get Notes', async () => {
    const { getNotes } = setup();
    const response = await getNotes(NoteTypes.File);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockNotesResposne);
  });

  it('Delete Note', async () => {
    const { deleteNote } = setup();
    const response = await deleteNote(NoteTypes.File, 1);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(true);
  });
});
