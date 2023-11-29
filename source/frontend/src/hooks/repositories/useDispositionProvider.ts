import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiDispositionFile } from '@/hooks/pims-api/useApiDispositionFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import {
  Api_DispositionFile,
  Api_DispositionFileProperty,
  Api_DispositionFileTeam,
} from '@/models/api/DispositionFile';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { useAxiosErrorHandler, useAxiosErrorHandlerWithAuthorization } from '@/utils';

/**
 * hook that interacts with the Disposition File API.
 */
export const useDispositionProvider = () => {
  const {
    getDispositionFile,
    getDispositionFileProperties,
    getLastUpdatedByApi,
    getAllDispositionFileTeamMembers,
  } = useApiDispositionFile();

  const getDispositionFileApi = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<Api_DispositionFile, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFile(dispositionFileId),
      [getDispositionFile],
    ),
    requestName: 'RetrieveDispositionFile',
    onError: useAxiosErrorHandlerWithAuthorization('Failed to load Disposition File'),
  });

  const getLastUpdatedBy = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<Api_LastUpdatedBy, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getLastUpdatedByApi(dispositionFileId),
      [getLastUpdatedByApi],
    ),
    requestName: 'getLastUpdatedBy',
    onError: useAxiosErrorHandler('Failed to load Disposition File last-updated-by'),
  });

  const getDispositionPropertiesApi = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<Api_DispositionFileProperty[], any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFileProperties(dispositionFileId),
      [getDispositionFileProperties],
    ),
    requestName: 'GetDispositionFileProperties',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Properties'),
  });

  const getAllDispositionTeamMembersApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_DispositionFileTeam[], any>>
  >({
    requestFunction: useCallback(
      async () => await getAllDispositionFileTeamMembers(),
      [getAllDispositionFileTeamMembers],
    ),
    requestName: 'GetAllDispositionTeamMembers',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Team Members'),
  });

  return useMemo(
    () => ({
      getDispositionFile: getDispositionFileApi,
      getLastUpdatedBy,
      getDispositionProperties: getDispositionPropertiesApi,
      getAllDispositionTeamMembers: getAllDispositionTeamMembersApi,
    }),
    [
      getDispositionFileApi,
      getLastUpdatedBy,
      getDispositionPropertiesApi,
      getAllDispositionTeamMembersApi,
    ],
  );
};
