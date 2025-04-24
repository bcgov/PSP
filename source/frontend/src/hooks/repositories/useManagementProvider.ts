import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { AxiosError, AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';
import { useDispatch } from 'react-redux';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementFileProperty';
import { ApiGen_Concepts_ManagementFileTeam } from '@/models/api/generated/ApiGen_Concepts_ManagementFileTeam';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiManagementFile } from '../pims-api/useApiManagementFile';
import { handleAxiosResponseRaw } from './../../utils/utils';

/**
 * hook that interacts with the Management File API.
 */
export const useManagementProvider = ({ managementFileId }: { managementFileId?: number }) => {
  const {
    postManagementFileApi,
    getManagementFile,
    putManagementFileApi,
    putManagementFileProperties,
    getManagementFileProperties,
    getLastUpdatedByApi,
    getAllManagementFileTeamMembers,
  } = useApiManagementFile();
  const dispatch = useDispatch();
  const queryClient = useQueryClient();

  const addManagementFileMutation = useMutation<
    ApiGen_Concepts_ManagementFile,
    AxiosError,
    {
      managementFile: ApiGen_Concepts_ManagementFile;
      userOverrideCodes: UserOverrideCode[];
    }
  >({
    mutationFn: async ({ managementFile, userOverrideCodes }) =>
      await handleAxiosResponseRaw(
        dispatch,
        'AddManagementFile',
        postManagementFileApi(managementFile, userOverrideCodes ?? []),
      ),
    mutationKey: ['AddManagementFile'],
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['GetManagementFiles'] }); // invalidate the list of management files as a new one has been added.
    },
  });

  const getManagementFileQuery = useQuery({
    queryKey: ['RetrieveManagementFile', managementFileId],
    queryFn: async () =>
      await handleAxiosResponseRaw(
        dispatch,
        'RetrieveManagementFile',
        getManagementFile(managementFileId),
      ),
  });

  const updateManagementFileMutation = useMutation<
    ApiGen_Concepts_ManagementFile,
    AxiosError,
    {
      managementFileId: number;
      managementFile: ApiGen_Concepts_ManagementFile;
      userOverrideCodes: UserOverrideCode[];
    }
  >({
    mutationFn: async ({ managementFileId, managementFile, userOverrideCodes }) =>
      await handleAxiosResponseRaw(
        dispatch,
        'UpdateManagementFile',
        putManagementFileApi(managementFileId, managementFile, userOverrideCodes ?? []),
      ),
    mutationKey: ['UpdateManagementFile'],
    onSuccess: (data: ApiGen_Concepts_ManagementFile) => {
      queryClient.invalidateQueries({ queryKey: ['RetrieveManagementFile', data?.id] }); // invalidate the management file.
    },
  });

  const updateManagementPropertiesApi = useApiRequestWrapper<
    (
      managementFile: ApiGen_Concepts_ManagementFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFile, any>>
  >({
    requestFunction: useCallback(
      async (
        managementFile: ApiGen_Concepts_ManagementFile,
        userOverrideCodes: UserOverrideCode[],
      ) => await putManagementFileProperties(managementFile, userOverrideCodes),
      [putManagementFileProperties],
    ),
    requestName: 'UpdateManagementFileProperties',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  const getLastUpdatedBy = useApiRequestWrapper<
    (managementFileId: number) => Promise<AxiosResponse<Api_LastUpdatedBy, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number) => await getLastUpdatedByApi(managementFileId),
      [getLastUpdatedByApi],
    ),
    requestName: 'getLastUpdatedBy',
    onError: useAxiosErrorHandler('Failed to load Management File last-updated-by'),
  });

  const getManagementPropertiesApi = useApiRequestWrapper<
    (
      managementFileId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFileProperty[], any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number) => await getManagementFileProperties(managementFileId),
      [getManagementFileProperties],
    ),
    requestName: 'GetManagementFileProperties',
    onError: useAxiosErrorHandler('Failed to retrieve Management File Properties'),
  });

  const getAllManagementTeamMembersApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_ManagementFileTeam[], any>>
  >({
    requestFunction: useCallback(
      async () => await getAllManagementFileTeamMembers(),
      [getAllManagementFileTeamMembers],
    ),
    requestName: 'GetAllManagementTeamMembers',
    onError: useAxiosErrorHandler('Failed to retrieve Management File Team Members'),
  });

  return useMemo(
    () => ({
      getLastUpdatedBy,
      updateManagementProperties: updateManagementPropertiesApi,
      getManagementProperties: getManagementPropertiesApi,
      getAllManagementTeamMembers: getAllManagementTeamMembersApi,
      addManagementFileMutation,
      updateManagementFileMutation,
      getManagementFileQuery,
    }),
    [
      getLastUpdatedBy,
      updateManagementPropertiesApi,
      getManagementPropertiesApi,
      getAllManagementTeamMembersApi,
      addManagementFileMutation,
      updateManagementFileMutation,
      getManagementFileQuery,
    ],
  );
};
