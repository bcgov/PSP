import { NoteTypes } from 'constants/index';
import { Api_Note } from 'models/api/Note';
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
      postNote: (type: NoteTypes, note: Api_Note) => api.post<Api_Note>(`/notes/${type}`, note),
    }),
    [api],
  );
};
