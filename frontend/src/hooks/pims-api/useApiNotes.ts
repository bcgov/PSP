import { NoteTypes } from 'constants/noteTypes';
import { Api_EntityNote, Api_Note } from 'models/api/Note';
import React from 'react';

import { useAxiosApi } from './';

/**
 * PIMS API wrapper to centralize all AJAX requests to the note endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiNotes = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getNotes: (type: NoteTypes, entityId: number) =>
        api.get<Api_Note[]>(`/notes/${type}/owner/${entityId}`),
      getNote: (type: NoteTypes, noteId: number) => api.get<Api_Note>(`/notes/${type}/${noteId}`),
      postNote: (type: NoteTypes, note: Api_EntityNote) =>
        api.post<Api_EntityNote>(`/notes/${type}`, note),
      putNote: (type: NoteTypes, note: Api_Note) =>
        api.put<Api_Note>(`/notes/${type}/${note.id ?? 0}`, note),
      deleteNote: (type: NoteTypes, noteId: number) =>
        api.delete<boolean>(`/notes/${type}/${noteId}`),
    }),
    [api],
  );
};
