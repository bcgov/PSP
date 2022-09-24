import { IAcquisitionFilter } from 'features/acquisition/list/interfaces';
import { IPagedItems } from 'interfaces';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import queryString from 'query-string';
import React from 'react';

import { IPaginateRequest, useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the acquisition file endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiAcquisitionFile = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getAcquisitionFiles: (params: IPaginateAcquisition | null) =>
        api.get<IPagedItems<Api_AcquisitionFile>>(
          `/acquisitionfiles/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getAcquisitionFile: (acqFileId: number) =>
        api.get<Api_AcquisitionFile>(`/acquisitionfiles/${acqFileId}`),
      postAcquisitionFile: (acqFile: Api_ResearchFile) =>
        api.post<Api_AcquisitionFile>(`/acquisitionfiles`, acqFile),
      putAcquisitionFile: (acqFile: Api_ResearchFile) =>
        api.put<Api_AcquisitionFile>(`/acquisitionfiles/${acqFile.id}`, acqFile),
      putAcquisitionFileProperties: (acqFile: Api_AcquisitionFile) =>
        api.put<Api_ResearchFile>(`/acquisitionfiles/${acqFile?.id}/properties`, acqFile),
    }),
    [api],
  );
};

export type IPaginateAcquisition = IPaginateRequest<IAcquisitionFilter>;
