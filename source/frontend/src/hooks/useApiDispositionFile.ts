import React from 'react';

import { Api_DispositionFile, Api_DispositionFileProperty } from '@/models/api/DispositionFile';
import { Api_LastUpdatedBy } from '@/models/api/File';

import useAxiosApi from './pims-api/useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the disposition file endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiDispositionFile = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      postDispositionFileApi: (dispositionFile: Api_DispositionFile) =>
        api.post<Api_DispositionFile>('/dispositionfiles', dispositionFile),
      getDispositionFile: (dispositionFileId: number) =>
        api.get<Api_DispositionFile>(`/dispositionfiles/${dispositionFileId}`),
      getLastUpdatedByApi: (dispositionFileId: number) =>
        api.get<Api_LastUpdatedBy>(`/dispositionfiles/${dispositionFileId}/updateInfo`),
      getDispositionFileProperties: (dispositionFileId: number) =>
        api.get<Api_DispositionFileProperty[]>(`/dispositionfiles/${dispositionFileId}/properties`),
    }),
    [api],
  );
};
