import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileContact } from '@/models/api/generated/ApiGen_Concepts_ManagementFileContact';
import { ApiGen_Concepts_ManagementFileProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementFileProperty';
import { ApiGen_Concepts_ManagementFileTeam } from '@/models/api/generated/ApiGen_Concepts_ManagementFileTeam';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import {
  useAxiosErrorHandler,
  useAxiosErrorHandlerWithAuthorization,
  useAxiosSuccessHandler,
} from '@/utils';

import { useApiManagementFile } from '../pims-api/useApiManagementFile';
import {
  deleteManagementFileContactsApi,
  getManagementFileContactApi,
  getManagementFileContactsApi,
  postManagementFileContactsApi,
  putManagementFileContactsApi,
} from '../pims-api/useApiManagementFileContact';

/**
 * hook that interacts with the Management File API.
 */
export const useManagementFileRepository = () => {
  const {
    postManagementFileApi,
    getManagementFile,
    putManagementFileApi,
    putManagementFileProperties,
    getManagementFileProperties,
    getLastUpdatedByApi,
    getAllManagementFileTeamMembers,
  } = useApiManagementFile();

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
      ) => await postManagementFileApi(managementFile, useOverride),
      [postManagementFileApi],
    ),
    requestName: 'AddManagementFile',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  const getManagementFileApi = useApiRequestWrapper<
    (managementFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFile, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number) => await getManagementFile(managementFileId),
      [getManagementFile],
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
      ) => await putManagementFileApi(managementFileId, managementFile, useOverride),
      [putManagementFileApi],
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

  const getManagementContactsApi = useApiRequestWrapper<
    (
      managementFileId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFileContact[], any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number) => await getManagementFileContactsApi(managementFileId),
      [],
    ),
    requestName: 'GetManagementFileContacts',
    onError: useAxiosErrorHandler('Failed to retrieve Management File Contacts'),
  });

  const getManagementContactApi = useApiRequestWrapper<
    (
      managementFileId: number,
      managementFileContactId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFileContact, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, managementFileContactId: number) =>
        await getManagementFileContactApi(managementFileId, managementFileContactId),
      [],
    ),
    requestName: 'GetManagementFileContact',
    onError: useAxiosErrorHandler('Failed to retrieve Management File Contact'),
  });

  const postManagementContactApi = useApiRequestWrapper<
    (
      managementFileId: number,
      managementFileContact: ApiGen_Concepts_ManagementFileContact,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFileContact, any>>
  >({
    requestFunction: useCallback(
      async (
        managementFileId: number,
        managementFileContact: ApiGen_Concepts_ManagementFileContact,
      ) => await postManagementFileContactsApi(managementFileId, managementFileContact),
      [],
    ),
    requestName: 'PostManagementFileContact',
    onError: useAxiosErrorHandler('Failed to create Management File Contact'),
  });

  const putManagementContactApi = useApiRequestWrapper<
    (
      managementFileId: number,
      managementFileContactId: number,
      managementFileContact: ApiGen_Concepts_ManagementFileContact,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementFileContact, any>>
  >({
    requestFunction: useCallback(
      async (
        managementFileId: number,
        managementFileContactId: number,
        managementFileContact: ApiGen_Concepts_ManagementFileContact,
      ) =>
        await putManagementFileContactsApi(
          managementFileId,
          managementFileContactId,
          managementFileContact,
        ),
      [],
    ),
    requestName: 'PutManagementFileContact',
    onError: useAxiosErrorHandler('Failed to update Management File Contact'),
  });

  const deleteManagementContactApi = useApiRequestWrapper<
    (
      managementFileId: number,
      managementFileContactId: number,
    ) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, managementFileContactId: number) =>
        await deleteManagementFileContactsApi(managementFileId, managementFileContactId),
      [],
    ),
    requestName: 'DeleteManagementFileContact',
    onError: useAxiosErrorHandler('Failed to delete Management File Contact'),
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
      getAllManagementFileContacts: getManagementContactsApi,
      getManagementFileContact: getManagementContactApi,
      postManagementFileContact: postManagementContactApi,
      putManagementContact: putManagementContactApi,
      deleteManagementContact: deleteManagementContactApi,
    }),
    [
      addManagementFileApi,
      getManagementFileApi,
      updateManagementFileApi,
      getLastUpdatedBy,
      updateManagementPropertiesApi,
      getManagementPropertiesApi,
      getAllManagementTeamMembersApi,
      getManagementContactsApi,
      getManagementContactApi,
      postManagementContactApi,
      putManagementContactApi,
      deleteManagementContactApi,
    ],
  );
};
