import queryString from 'query-string';
import React from 'react';

import { IPagedItems } from '@/interfaces';
import {
  Api_DispositionFile,
  Api_DispositionFileProperty,
  Api_DispositionFileTeam,
} from '@/models/api/DispositionFile';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { Api_LastUpdatedBy } from '@/models/api/File';

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
      getDispositionFile: (dispositionFileId: number) =>
        api.get<Api_DispositionFile>(`/dispositionfiles/${dispositionFileId}`),
      getLastUpdatedByApi: (dispositionFileId: number) =>
        api.get<Api_LastUpdatedBy>(`/dispositionfiles/${dispositionFileId}/updateInfo`),
      getDispositionFileProperties: (dispositionFileId: number) =>
        api.get<Api_DispositionFileProperty[]>(`/dispositionfiles/${dispositionFileId}/properties`),
      getAllDispositionFileTeamMembers: () =>
        api.get<Api_DispositionFileTeam[]>(`/dispositionfiles/team-members`),
    }),
    [api],
  );
};
