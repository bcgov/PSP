import { NoteTypes } from 'constants/index';
import { Api_EntityNote } from 'models/api/Note';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the notes endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiNotes = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      postNote: (type: NoteTypes, note: Api_EntityNote) =>
        api.post<Api_EntityNote>(`/notes/${type}`, note),
    }),
    [api],
  );
};
