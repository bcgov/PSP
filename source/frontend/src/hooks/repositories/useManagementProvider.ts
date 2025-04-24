import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementFileProperty';
import { ApiGen_Concepts_ManagementFileTeam } from '@/models/api/generated/ApiGen_Concepts_ManagementFileTeam';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { setManagementFile } from '@/store/slices/files/managementFileSlice';
import { RootState } from '@/store/store';
import {
  exists,
  useAxiosErrorHandler,
  useAxiosErrorHandlerWithAuthorization,
  useAxiosSuccessHandler,
} from '@/utils';

import { useApiManagementFile } from '../pims-api/useApiManagementFile';

/**
 * hook that interacts with the Management File API.
 */
export const useManagementProvider = () => {
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
  const cachedManagementFiles = useSelector((state: RootState) => state.managementFiles);

  const addManagementFileApi = useApiRequestWrapper<
    (
      managementFile: ApiGen_Concepts_ManagementFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFile, any>>
  >({
    requestFunction: useCallback(
      async (
        managementFile: ApiGen_Concepts_ManagementFile,
        useOverride: UserOverrideCode[] = [],
      ) => {
        const response = await postManagementFileApi(managementFile, useOverride);
        if (response.status === 201 && exists(response.data)) {
          dispatch(setManagementFile(response.data));
        }
        return response;
      },
      [dispatch, postManagementFileApi],
    ),
    requestName: 'AddManagementFile',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  const getManagementFileApi = useApiRequestWrapper<
    (managementFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFile, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number) => {
        if (exists(cachedManagementFiles[managementFileId])) {
          return { data: cachedManagementFiles[managementFileId] } as any as AxiosResponse<
            ApiGen_Concepts_ManagementFile,
            any
          >;
        }
        const response = await getManagementFile(managementFileId);
        if (response.status === 200 && exists(response.data)) {
          dispatch(setManagementFile(response.data));
        }
        return response;
      },
      [cachedManagementFiles, dispatch, getManagementFile],
    ),
    requestName: 'RetrieveManagementFile',
    onError: useAxiosErrorHandlerWithAuthorization('Failed to load Management File'),
  });

  const updateManagementFileApi = useApiRequestWrapper<
    (
      managementFileId: number,
      managementFile: ApiGen_Concepts_ManagementFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFile, any>>
  >({
    requestFunction: useCallback(
      async (
        managementFileId: number,
        managementFile: ApiGen_Concepts_ManagementFile,
        useOverride: UserOverrideCode[] = [],
      ) => {
        const response = await putManagementFileApi(managementFileId, managementFile, useOverride);
        if (response.status === 200 && exists(response.data)) {
          dispatch(setManagementFile(response.data));
        }
        return response;
      },
      [dispatch, putManagementFileApi],
    ),
    requestName: 'UpdateManagementFile',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
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
      addManagementFileApi: addManagementFileApi,
      getManagementFile: getManagementFileApi,
      putManagementFile: updateManagementFileApi,
      getLastUpdatedBy,
      updateManagementProperties: updateManagementPropertiesApi,
      getManagementProperties: getManagementPropertiesApi,
      getAllManagementTeamMembers: getAllManagementTeamMembersApi,
    }),
    [
      addManagementFileApi,
      getManagementFileApi,
      updateManagementFileApi,
      getLastUpdatedBy,
      getManagementPropertiesApi,
      getAllManagementTeamMembersApi,
      updateManagementPropertiesApi,
    ],
  );
};
