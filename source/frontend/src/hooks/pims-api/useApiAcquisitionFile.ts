import { IAcquisitionFilter } from 'features/acquisition/list/interfaces';
import { IPagedItems } from 'interfaces';
import {
  Api_AcquisitionFile,
  Api_AcquisitionFileChecklistItem,
  Api_AcquisitionFileOwner,
  Api_AcquisitionFileProperty,
} from 'models/api/AcquisitionFile';
import { Api_Compensation } from 'models/api/Compensation';
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
      postAcquisitionFile: (acqFile: Api_AcquisitionFile) =>
        api.post<Api_AcquisitionFile>(`/acquisitionfiles`, acqFile),
      putAcquisitionFile: (acqFile: Api_AcquisitionFile, userOverride = false) =>
        api.put<Api_AcquisitionFile>(
          `/acquisitionfiles/${acqFile.id}?userOverride=${userOverride}`,
          acqFile,
        ),
      putAcquisitionFileProperties: (acqFile: Api_AcquisitionFile) =>
        api.put<Api_AcquisitionFile>(`/acquisitionfiles/${acqFile?.id}/properties`, acqFile),
      getAcquisitionFileProperties: (acqFileId: number) =>
        api.get<Api_AcquisitionFileProperty[]>(`/acquisitionfiles/${acqFileId}/properties`),
      getAcquisitionFileOwners: (acqFileId: number) =>
        api.get<Api_AcquisitionFileOwner[]>(`/acquisitionfiles/${acqFileId}/owners`),
      getAcquisitionFileChecklist: (acqFileId: number) =>
        api.get<Api_AcquisitionFileChecklistItem[]>(`/acquisitionfiles/${acqFileId}/checklist`),
      putAcquisitionFileChecklist: (acqFile: Api_AcquisitionFile) =>
        api.put<Api_AcquisitionFile>(`/acquisitionfiles/${acqFile?.id}/checklist`, acqFile),
      getFileCompensationRequisitions: (acqFileId: number) =>
        api.get<Api_Compensation[]>(`/acquisitionfiles/${acqFileId}/compensation-requisitions`),
      postFileCompensationRequisition: (
        acqFileId: number,
        compensationRequisition: Api_Compensation,
      ) =>
        api.post<Api_Compensation>(
          `/acquisitionfiles/${acqFileId}/compensation-requisitions`,
          compensationRequisition,
        ),
    }),
    [api],
  );
};

export type IPaginateAcquisition = IPaginateRequest<IAcquisitionFilter>;
