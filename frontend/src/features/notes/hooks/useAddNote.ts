import { AxiosResponse } from 'axios';
import { NoteTypes } from 'constants/index';
import { useApiNotes } from 'hooks/pims-api/useApiNotes';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_Note } from 'models/api/Note';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { useCallback } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that adds a note.
 */
export const useAddNote = () => {
  const { postNote } = useApiNotes();

  const { execute } = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_ResearchFile, any>>
  >({
    requestFunction: useCallback(
      async (type: NoteTypes, note: Api_Note) => await postNote(type, note),
      [postNote],
    ),
    requestName: 'AddNote',
    onSuccess: useAxiosSuccessHandler('Note saved'),
    onError: useAxiosErrorHandler(),
  });

  return { addResearchFile: execute };
};
