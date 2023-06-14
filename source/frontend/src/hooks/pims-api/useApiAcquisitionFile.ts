import queryString from 'query-string';
import React from 'react';

import { IAcquisitionFilter } from '@/features/acquisition/list/interfaces';
import { IPagedItems } from '@/interfaces';
import {
  Api_AcquisitionFile,
  Api_AcquisitionFileChecklistItem,
  Api_AcquisitionFileOwner,
  Api_AcquisitionFileProperty,
  Api_AcquisitionFileRepresentative,
  Api_AcquisitionFileSolicitor,
} from '@/models/api/AcquisitionFile';
import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_Product, Api_Project } from '@/models/api/Project';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

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
      postAcquisitionFile: (
        acqFile: Api_AcquisitionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.post<Api_AcquisitionFile>(
          `/acquisitionfiles?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          acqFile,
        ),
      putAcquisitionFile: (
        acqFile: Api_AcquisitionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<Api_AcquisitionFile>(
          `/acquisitionfiles/${acqFile.id}?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          acqFile,
        ),
      putAcquisitionFileProperties: (
        acqFile: Api_AcquisitionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<Api_AcquisitionFile>(
          `/acquisitionfiles/${acqFile?.id}/properties?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          acqFile,
        ),
      getAcquisitionFileProperties: (acqFileId: number) =>
        api.get<Api_AcquisitionFileProperty[]>(`/acquisitionfiles/${acqFileId}/properties`),
      getAcquisitionFileOwners: (acqFileId: number) =>
        api.get<Api_AcquisitionFileOwner[]>(`/acquisitionfiles/${acqFileId}/owners`),
      getAcquisitionFileSolicitors: (acqFileId: number) =>
        api.get<Api_AcquisitionFileSolicitor[]>(`/acquisitionfiles/${acqFileId}/owner-solicitors`),
      getAcquisitionFileRepresentatives: (acqFileId: number) =>
        api.get<Api_AcquisitionFileRepresentative[]>(
          `/acquisitionfiles/${acqFileId}/owner-representatives`,
        ),
      getAcquisitionFileProject: (acqFileId: number) =>
        api.get<Api_Project>(`/acquisitionfiles/${acqFileId}/project`),
      getAcquisitionFileProduct: (acqFileId: number) =>
        api.get<Api_Product>(`/acquisitionfiles/${acqFileId}/product`),
      getAcquisitionFileChecklist: (acqFileId: number) =>
        api.get<Api_AcquisitionFileChecklistItem[]>(`/acquisitionfiles/${acqFileId}/checklist`),
      putAcquisitionFileChecklist: (acqFile: Api_AcquisitionFile) =>
        api.put<Api_AcquisitionFile>(`/acquisitionfiles/${acqFile?.id}/checklist`, acqFile),
      getFileCompensationRequisitions: (acqFileId: number) =>
        api.get<Api_CompensationRequisition[]>(
          `/acquisitionfiles/${acqFileId}/compensation-requisitions`,
        ),
      getFileCompReqH120s: (acqFileId: number, finalOnly?: boolean) =>
        api.get<Api_CompensationFinancial[]>(
          `/acquisitionfiles/${acqFileId}/comp-req-h120s?finalOnly=${!!finalOnly}`,
        ),
      postFileCompensationRequisition: (
        acqFileId: number,
        compensationRequisition: Api_CompensationRequisition,
      ) =>
        api.post<Api_CompensationRequisition>(
          `/acquisitionfiles/${acqFileId}/compensation-requisitions`,
          compensationRequisition,
        ),
    }),
    [api],
  );
};

export type IPaginateAcquisition = IPaginateRequest<IAcquisitionFilter>;
