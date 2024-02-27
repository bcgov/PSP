import React from 'react';

import { NoteTypes } from '@/constants/noteTypes';
import { ApiGen_Concepts_EntityNote } from '@/models/api/generated/ApiGen_Concepts_EntityNote';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the note endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiNotes = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getNotes: (type: NoteTypes, entityId: number) =>
        api.get<ApiGen_Concepts_Note[]>(`/notes/${type}/${entityId}`),
      getNote: (noteId: number) => api.get<ApiGen_Concepts_Note>(`/notes/${noteId}`),
      postNote: (type: NoteTypes, note: ApiGen_Concepts_EntityNote) =>
        api.post<ApiGen_Concepts_EntityNote>(`/notes/${type}`, note),
      putNote: (note: ApiGen_Concepts_Note) =>
        api.put<ApiGen_Concepts_Note>(`/notes/${note.id}`, note),
      deleteNote: (type: NoteTypes, noteId: number) =>
        api.delete<boolean>(`/notes/${noteId}/${type}`),
    }),
    [api],
  );
};
