import queryString from 'query-string';
import React from 'react';

import { IPagedItems } from '@/interfaces';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_DispositionFileAppraisal } from '@/models/api/generated/ApiGen_Concepts_DispositionFileAppraisal';
import { ApiGen_Concepts_DispositionFileOffer } from '@/models/api/generated/ApiGen_Concepts_DispositionFileOffer';
import { ApiGen_Concepts_DispositionFileProperty } from '@/models/api/generated/ApiGen_Concepts_DispositionFileProperty';
import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';
import { ApiGen_Concepts_DispositionFileTeam } from '@/models/api/generated/ApiGen_Concepts_DispositionFileTeam';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

export type IPaginateDisposition = IPaginateRequest<Api_DispositionFilter>;

/**
 * PIMS API wrapper to centralize all AJAX requests to the disposition file endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiDispositionFile = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getDispositionFilesPagedApi: (params: IPaginateDisposition | null) =>
        api.get<IPagedItems<ApiGen_Concepts_DispositionFile>>(
          `/dispositionfiles/search?${params ? queryString.stringify(params) : ''}`,
        ),
      postDispositionFileApi: (
        dispositionFile: ApiGen_Concepts_DispositionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.post<ApiGen_Concepts_DispositionFile>(
          `/dispositionfiles?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          dispositionFile,
        ),
      getDispositionFile: (dispositionFileId: number) =>
        api.get<ApiGen_Concepts_DispositionFile>(`/dispositionfiles/${dispositionFileId}`),
      putDispositionFileApi: (
        dispositionFileId: number,
        dispositionFile: ApiGen_Concepts_DispositionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<ApiGen_Concepts_DispositionFile>(
          `/dispositionfiles/${dispositionFileId}?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          dispositionFile,
        ),

      getLastUpdatedByApi: (dispositionFileId: number) =>
        api.get<Api_LastUpdatedBy>(`/dispositionfiles/${dispositionFileId}/updateInfo`),
      getDispositionFileChecklist: (dispositionFileId: number) =>
        api.get<[]>(`/dispositionfiles/${dispositionFileId}/checklist`),
      putDispositionFileChecklist: (dispositionFile: ApiGen_Concepts_FileWithChecklist) =>
        api.put<ApiGen_Concepts_DispositionFile>(
          `/dispositionfiles/${dispositionFile?.id}/checklist`,
          dispositionFile.fileChecklistItems,
        ),
      getDispositionFileProperties: (dispositionFileId: number) =>
        api.get<ApiGen_Concepts_DispositionFileProperty[]>(
          `/dispositionfiles/${dispositionFileId}/properties`,
        ),
      getAllDispositionFileTeamMembers: () =>
        api.get<ApiGen_Concepts_DispositionFileTeam[]>(`/dispositionfiles/team-members`),
      getDispositionFileAppraisal: (dispositionFileId: number) =>
        api.get<ApiGen_Concepts_DispositionFileAppraisal>(
          `/dispositionfiles/${dispositionFileId}/appraisal`,
        ),
      postDispositionFileAppraisal: (
        dispositionFileId: number,
        appraisal: ApiGen_Concepts_DispositionFileAppraisal,
      ) =>
        api.post<ApiGen_Concepts_DispositionFileAppraisal>(
          `/dispositionfiles/${dispositionFileId}/appraisal`,
          appraisal,
        ),
      putDispositionFileAppraisal: (
        dispositionFileId: number,
        dispositionAppraisalId: number,
        appraisal: ApiGen_Concepts_DispositionFileAppraisal,
      ) =>
        api.put<ApiGen_Concepts_DispositionFileAppraisal>(
          `/dispositionfiles/${dispositionFileId}/appraisal/${dispositionAppraisalId}`,
          appraisal,
        ),

      getDispositionFileOffers: (dispositionFileId: number) =>
        api.get<ApiGen_Concepts_DispositionFileOffer[]>(
          `/dispositionfiles/${dispositionFileId}/offers`,
        ),
      postDispositionFileOffer: (
        dispositionFileId: number,
        offer: ApiGen_Concepts_DispositionFileOffer,
      ) =>
        api.post<ApiGen_Concepts_DispositionFileOffer>(
          `/dispositionfiles/${dispositionFileId}/offers`,
          offer,
        ),
      getDispositionFileSale: (dispositionFileId: number) =>
        api.get<ApiGen_Concepts_DispositionFileSale>(`/dispositionfiles/${dispositionFileId}/sale`),
      postDispositionFileSale: (
        dispositionFileId: number,
        sale: ApiGen_Concepts_DispositionFileSale,
      ) =>
        api.post<ApiGen_Concepts_DispositionFileSale>(
          `/dispositionfiles/${dispositionFileId}/sale`,
          sale,
        ),
      putDispositionFileSale: (
        dispositionFileId: number,
        saleId: number,
        sale: ApiGen_Concepts_DispositionFileSale,
      ) =>
        api.put<ApiGen_Concepts_DispositionFileSale>(
          `/dispositionfiles/${dispositionFileId}/sale/${saleId}`,
          sale,
        ),
      getDispositionFileOffer: (dispositionFileId: number, offferId: number) =>
        api.get<ApiGen_Concepts_DispositionFileOffer>(
          `/dispositionfiles/${dispositionFileId}/offers/${offferId}`,
        ),
      putDispositionFileOffer: (
        dispositionFileId: number,
        offferId: number,
        offer: ApiGen_Concepts_DispositionFileOffer,
      ) =>
        api.put<ApiGen_Concepts_DispositionFileOffer>(
          `/dispositionfiles/${dispositionFileId}/offers/${offferId}`,
          offer,
        ),
      deleteDispositionFileOffer: (dispositionFileId: number, offferId: number) =>
        api.delete<boolean>(`/dispositionfiles/${dispositionFileId}/offers/${offferId}`),
      exportDispositionFiles: (filter: IPaginateDisposition) =>
        api.get<Blob>(
          `/reports/disposition?${filter ? queryString.stringify({ ...filter, all: true }) : ''}`,
          {
            responseType: 'blob',
            headers: {
              Accept: 'application/vnd.ms-excel',
            },
          },
        ),
    }),
    [api],
  );
};
