import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { NoteTypes } from '@/constants/index';
import { useApiNotes } from '@/hooks/pims-api/useApiNotes';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_EntityNote } from '@/models/api/generated/ApiGen_Concepts_EntityNote';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that interacts with the Notes API.
 */
export const useNoteRepository = () => {
  const { getNote, postNote, putNote } = useApiNotes();

  const addNoteApi = useApiRequestWrapper<
    (
      type: NoteTypes,
      note: ApiGen_Concepts_EntityNote,
    ) => Promise<AxiosResponse<ApiGen_Concepts_EntityNote, any>>
  >({
    requestFunction: useCallback(
      async (type: NoteTypes, note: ApiGen_Concepts_EntityNote) => await postNote(type, note),
      [postNote],
    ),
    requestName: 'AddNote',
    onSuccess: useAxiosSuccessHandler('Note saved'),
    onError: useAxiosErrorHandler(),
  });

  const getNoteApi = useApiRequestWrapper<
    (noteId: number) => Promise<AxiosResponse<ApiGen_Concepts_Note, any>>
  >({
    requestFunction: useCallback(async (noteId: number) => await getNote(noteId), [getNote]),
    requestName: 'GetNote',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateNoteApi = useApiRequestWrapper<
    (note: ApiGen_Concepts_Note) => Promise<AxiosResponse<ApiGen_Concepts_Note, any>>
  >({
    requestFunction: useCallback(
      async (note: ApiGen_Concepts_Note) => await putNote(note),
      [putNote],
    ),
    requestName: 'UpdateNote',
    onSuccess: useAxiosSuccessHandler('Note saved'),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      addNote: addNoteApi,
      getNote: getNoteApi,
      updateNote: updateNoteApi,
    }),
    [addNoteApi, getNoteApi, updateNoteApi],
  );
};
