import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { mockNotesResposne } from 'mocks/mockNoteResponses';
import { NoteType } from 'models/api/Note';

import { useApiNotes } from './useApiNotes';

const mockAxios = new MockAdapter(axios);

describe('useApiNotes testing suite', () => {
  beforeEach(() => {
    mockAxios.onGet(`/notes/1`).reply(200, mockNotesResposne);
    mockAxios.onDelete(`/notes/1/1`).reply(200, true);
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
    const response = await getNotes(NoteType.File);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockNotesResposne);
  });

  it('Delete Note', async () => {
    const { deleteNote } = setup();
    const response = await deleteNote(NoteType.File, 1);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(true);
  });
});
