import { Api_ResearchFile } from 'models/api/ResearchFile';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the acquisition file endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiAcquisitionFile = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getAcquisitionFile: (acqFileId: number) =>
        api.get<Api_ResearchFile>(`/acquisitionfiles/${acqFileId}`),
      postAcquisitionFile: (acqFile: Api_ResearchFile) =>
        api.post<Api_ResearchFile>(`/acquisitionfiles`, acqFile),
    }),
    [api],
  );
};
