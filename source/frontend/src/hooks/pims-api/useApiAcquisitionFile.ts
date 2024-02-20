import queryString from 'query-string';
import React from 'react';

import { ApiGen_Concepts_AcquisitionFilter } from '@/features/acquisition/list/interfaces';
import { IPagedItems } from '@/interfaces';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_ExpropriationPayment } from '@/models/api/generated/ApiGen_Concepts_ExpropriationPayment';
import { ApiGen_Concepts_FileChecklistItem } from '@/models/api/generated/ApiGen_Concepts_FileChecklistItem';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the acquisition file endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiAcquisitionFile = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getAcquisitionFiles: (params: IPaginateAcquisition | null) =>
        api.get<IPagedItems<ApiGen_Concepts_AcquisitionFile>>(
          `/acquisitionfiles/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getAcquisitionFile: (acqFileId: number) =>
        api.get<ApiGen_Concepts_AcquisitionFile>(`/acquisitionfiles/${acqFileId}`),
      getLastUpdatedByApi: (acqFileId: number) =>
        api.get<Api_LastUpdatedBy>(`/acquisitionfiles/${acqFileId}/updateInfo`),
      getAgreementReport: (filter: Api_ExportProjectFilter) =>
        api.post<Blob>(`/reports/acquisition/agreements`, filter, {
          responseType: 'blob',
          headers: {
            Accept: 'application/vnd.ms-excel',
          },
        }),
      getCompensationReport: (filter: Api_ExportProjectFilter) =>
        api.post<Blob>(`/reports/acquisition/compensation-requisitions`, filter, {
          responseType: 'blob',
          headers: {
            Accept: 'application/vnd.ms-excel',
          },
        }),
      exportAcquisitionFiles: (filter: IPaginateAcquisition) =>
        api.get<Blob>(
          `/reports/acquisition?${filter ? queryString.stringify({ ...filter, all: true }) : ''}`,
          {
            responseType: 'blob',
            headers: {
              Accept: 'application/vnd.ms-excel',
            },
          },
        ),
      postAcquisitionFile: (
        acqFile: ApiGen_Concepts_AcquisitionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.post<ApiGen_Concepts_AcquisitionFile>(
          `/acquisitionfiles?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          acqFile,
        ),
      putAcquisitionFile: (
        acqFile: ApiGen_Concepts_AcquisitionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<ApiGen_Concepts_AcquisitionFile>(
          `/acquisitionfiles/${acqFile.id}?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          acqFile,
        ),
      putAcquisitionFileProperties: (
        acqFile: ApiGen_Concepts_AcquisitionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<ApiGen_Concepts_AcquisitionFile>(
          `/acquisitionfiles/${acqFile?.id}/properties?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          acqFile,
        ),
      getAcquisitionFileProperties: (acqFileId: number) =>
        api.get<ApiGen_Concepts_AcquisitionFileProperty[]>(
          `/acquisitionfiles/${acqFileId}/properties`,
        ),
      getAcquisitionFileOwners: (acqFileId: number) =>
        api.get<ApiGen_Concepts_AcquisitionFileOwner[]>(`/acquisitionfiles/${acqFileId}/owners`),
      getAllAcquisitionFileTeamMembers: () =>
        api.get<ApiGen_Concepts_AcquisitionFileTeam[]>(`/acquisitionfiles/team-members`),
      getAcquisitionFileProject: (acqFileId: number) =>
        api.get<ApiGen_Concepts_Project>(`/acquisitionfiles/${acqFileId}/project`),
      getAcquisitionFileProduct: (acqFileId: number) =>
        api.get<ApiGen_Concepts_Product>(`/acquisitionfiles/${acqFileId}/product`),
      getAcquisitionFileChecklist: (acqFileId: number) =>
        api.get<ApiGen_Concepts_FileChecklistItem[]>(`/acquisitionfiles/${acqFileId}/checklist`),
      putAcquisitionFileChecklist: (acqFile: ApiGen_Concepts_FileWithChecklist) =>
        api.put<ApiGen_Concepts_AcquisitionFile>(
          `/acquisitionfiles/${acqFile?.id}/checklist`,
          acqFile.fileChecklistItems,
        ),
      getFileCompensationRequisitions: (acqFileId: number) =>
        api.get<ApiGen_Concepts_CompensationRequisition[]>(
          `/acquisitionfiles/${acqFileId}/compensation-requisitions`,
        ),
      getFileCompReqH120s: (acqFileId: number, finalOnly?: boolean) =>
        api.get<ApiGen_Concepts_CompensationFinancial[]>(
          `/acquisitionfiles/${acqFileId}/comp-req-h120s?finalOnly=${!!finalOnly}`,
        ),
      postFileCompensationRequisition: (
        acqFileId: number,
        compensationRequisition: ApiGen_Concepts_CompensationRequisition,
      ) =>
        api.post<ApiGen_Concepts_CompensationRequisition>(
          `/acquisitionfiles/${acqFileId}/compensation-requisitions`,
          compensationRequisition,
        ),
      getAcquisitionFileForm8s: (acqFileId: number) =>
        api.get<ApiGen_Concepts_ExpropriationPayment[]>(
          `/acquisitionfiles/${acqFileId}/expropriation-payments`,
        ),
      postFileForm8: (acqFileId: number, form8: ApiGen_Concepts_ExpropriationPayment) =>
        api.post<ApiGen_Concepts_ExpropriationPayment>(
          `/acquisitionfiles/${acqFileId}/expropriation-payments`,
          form8,
        ),
    }),
    [api],
  );
};

export type IPaginateAcquisition = IPaginateRequest<ApiGen_Concepts_AcquisitionFilter>;
