import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { NoteTypes } from '@/constants/index';
import { useApiNotes } from '@/hooks/pims-api/useApiNotes';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_EntityNote, Api_Note } from '@/models/api/Note';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that interacts with the Notes API.
 */
export const useNoteRepository = () => {
  const { getNote, postNote, putNote } = useApiNotes();

  const addNoteApi = useApiRequestWrapper<
    (type: NoteTypes, note: Api_EntityNote) => Promise<AxiosResponse<Api_EntityNote, any>>
  >({
    requestFunction: useCallback(
      async (type: NoteTypes, note: Api_EntityNote) => await postNote(type, note),
      [postNote],
    ),
    requestName: 'AddNote',
    onSuccess: useAxiosSuccessHandler('Note saved'),
    onError: useAxiosErrorHandler(),
  });

  const getNoteApi = useApiRequestWrapper<
    (noteId: number) => Promise<AxiosResponse<Api_Note, any>>
  >({
    requestFunction: useCallback(async (noteId: number) => await getNote(noteId), [getNote]),
    requestName: 'GetNote',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateNoteApi = useApiRequestWrapper<
    (note: Api_Note) => Promise<AxiosResponse<Api_Note, any>>
  >({
    requestFunction: useCallback(async (note: Api_Note) => await putNote(note), [putNote]),
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
