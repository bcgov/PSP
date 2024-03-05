import queryString from 'query-string';
import React from 'react';

import { IPagedItems } from '@/interfaces';
import {
  Api_DispositionFile,
  Api_DispositionFileAppraisal,
  Api_DispositionFileOffer,
  Api_DispositionFileProperty,
  Api_DispositionFileTeam,
} from '@/models/api/DispositionFile';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { Api_FileWithChecklist, Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';
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
        api.get<IPagedItems<Api_DispositionFile>>(
          `/dispositionfiles/search?${params ? queryString.stringify(params) : ''}`,
        ),
      putDispositionFileProperties: (
        dspFile: Api_DispositionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<Api_DispositionFile>(
          `/dispositionfiles/${dspFile?.id}/properties?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          dspFile,
        ),
      postDispositionFileApi: (
        dispositionFile: Api_DispositionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.post<Api_DispositionFile>(
          `/dispositionfiles?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          dispositionFile,
        ),
      getDispositionFile: (dispositionFileId: number) =>
        api.get<Api_DispositionFile>(`/dispositionfiles/${dispositionFileId}`),
      putDispositionFileApi: (
        dispositionFileId: number,
        dispositionFile: Api_DispositionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<Api_DispositionFile>(
          `/dispositionfiles/${dispositionFileId}?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          dispositionFile,
        ),
      getLastUpdatedByApi: (dispositionFileId: number) =>
        api.get<Api_LastUpdatedBy>(`/dispositionfiles/${dispositionFileId}/updateInfo`),
      getDispositionFileChecklist: (acqFileId: number) =>
        api.get<[]>(`/dispositionfiles/${acqFileId}/checklist`),
      putDispositionFileChecklist: (acqFile: Api_FileWithChecklist) =>
        api.put<Api_DispositionFile>(`/dispositionfiles/${acqFile?.id}/checklist`, acqFile),
      getDispositionFileProperties: (dispositionFileId: number) =>
        api.get<Api_DispositionFileProperty[]>(`/dispositionfiles/${dispositionFileId}/properties`),
      getAllDispositionFileTeamMembers: () =>
        api.get<Api_DispositionFileTeam[]>(`/dispositionfiles/team-members`),
      getDispositionFileAppraisal: (dispositionFileId: number) =>
        api.get<Api_DispositionFileAppraisal>(`/dispositionfiles/${dispositionFileId}/appraisal`),
      postDispositionFileAppraisal: (
        dispositionFileId: number,
        appraisal: Api_DispositionFileAppraisal,
      ) =>
        api.post<Api_DispositionFileAppraisal>(
          `/dispositionfiles/${dispositionFileId}/appraisal`,
          appraisal,
        ),
      putDispositionFileAppraisal: (
        dispositionFileId: number,
        dispositionAppraisalId: number,
        appraisal: Api_DispositionFileAppraisal,
      ) =>
        api.put<Api_DispositionFileAppraisal>(
          `/dispositionfiles/${dispositionFileId}/appraisal/${dispositionAppraisalId}`,
          appraisal,
        ),
      getDispositionFileOffers: (dispositionFileId: number) =>
        api.get<Api_DispositionFileOffer[]>(`/dispositionfiles/${dispositionFileId}/offers`),
      postDispositionFileOffer: (dispositionFileId: number, offer: Api_DispositionFileOffer) =>
        api.post<Api_DispositionFileOffer>(`/dispositionfiles/${dispositionFileId}/offers`, offer),
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
        api.get<Api_DispositionFileOffer>(
          `/dispositionfiles/${dispositionFileId}/offers/${offferId}`,
        ),
      putDispositionFileOffer: (
        dispositionFileId: number,
        offferId: number,
        offer: Api_DispositionFileOffer,
      ) =>
        api.put<Api_DispositionFileOffer>(
          `/dispositionfiles/${dispositionFileId}/offers/${offferId}`,
          offer,
        ),
      deleteDispositionFileOffer: (dispositionFileId: number, offferId: number) =>
        api.delete<boolean>(`/dispositionfiles/${dispositionFileId}/offers/${offferId}`),
      exportDispositionFiles: (filter: IPaginateDisposition, outputFormat: 'excel' = 'excel') =>
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
