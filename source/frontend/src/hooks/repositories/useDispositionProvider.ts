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
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import {
  useAxiosErrorHandler,
  useAxiosErrorHandlerWithAuthorization,
  useAxiosSuccessHandler,
} from '@/utils';

const ignoreErrorCodes = [409];

/**
 * hook that interacts with the Disposition File API.
 */
export const useDispositionProvider = () => {
  const {
    postDispositionFileApi,
    getDispositionFile,
    getDispositionFileProperties,
    getLastUpdatedByApi,
    getAllDispositionFileTeamMembers,
  } = useApiDispositionFile();

  const addDispositionFileApi = useApiRequestWrapper<
    (
      dispositionFile: Api_DispositionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<Api_DispositionFile, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFile: Api_DispositionFile, useOverride: UserOverrideCode[] = []) =>
        await postDispositionFileApi(dispositionFile, useOverride),
      [postDispositionFileApi],
    ),
    requestName: 'AddDispositionFile',
    onSuccess: useAxiosSuccessHandler('Disposition File saved'),
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

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
      addDispositionFileApi: addDispositionFileApi,
      getDispositionFile: getDispositionFileApi,
      getLastUpdatedBy,
      getDispositionProperties: getDispositionPropertiesApi,
      getAllDispositionTeamMembers: getAllDispositionTeamMembersApi,
    }),
    [
      addDispositionFileApi,
      getDispositionFileApi,
      getLastUpdatedBy,
      getDispositionPropertiesApi,
      getAllDispositionTeamMembersApi,
    ],
  );
};
