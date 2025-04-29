import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiAcquisitionFile } from '@/hooks/pims-api/useApiAcquisitionFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_ExpropriationPayment } from '@/models/api/generated/ApiGen_Concepts_ExpropriationPayment';
import { ApiGen_Concepts_FileChecklistItem } from '@/models/api/generated/ApiGen_Concepts_FileChecklistItem';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import {
  useAxiosErrorHandler,
  useAxiosErrorHandlerWithAuthorization,
  useAxiosSuccessHandler,
} from '@/utils';

/**
 * hook that interacts with the Acquisition File API.
 */
export const useAcquisitionProvider = () => {
  const {
    getAcquisitionFile,
    postAcquisitionFile,
    putAcquisitionFile,
    putAcquisitionFileProperties,
    getAcquisitionFileProperties,
    getAcquisitionFileOwners,
    getAcquisitionFileProject,
    getAcquisitionFileProduct,
    getAcquisitionFileChecklist,
    putAcquisitionFileChecklist,
    postFileForm8,
    getAcquisitionFileForm8s,
    getAllAcquisitionFileTeamMembers,
    getAgreementReport,
    getCompensationReport,
    getLastUpdatedByApi,
    getAcquisitionSubFiles,
    getAcquisitionAtTime,
  } = useApiAcquisitionFile();

  const addAcquisitionFileApi = useApiRequestWrapper<
    (
      acqFile: ApiGen_Concepts_AcquisitionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: ApiGen_Concepts_AcquisitionFile, useOverride: UserOverrideCode[] = []) =>
        await postAcquisitionFile(acqFile, useOverride),
      [postAcquisitionFile],
    ),
    requestName: 'AddAcquisitionFile',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  const getAcquisitionFileApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFile(acqFileId),
      [getAcquisitionFile],
    ),
    requestName: 'RetrieveAcquisitionFile',
    onError: useAxiosErrorHandlerWithAuthorization('Failed to load Acquisition File'),
  });

  const getLastUpdatedBy = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_LastUpdatedBy, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getLastUpdatedByApi(acqFileId),
      [getLastUpdatedByApi],
    ),
    requestName: 'getLastUpdatedBy',
    onError: useAxiosErrorHandler('Failed to load Acquisition File last-updated-by'),
  });

  const getAgreementsReportApi = useApiRequestWrapper<
    (filter: Api_ExportProjectFilter) => Promise<AxiosResponse<Blob, any>>
  >({
    requestFunction: useCallback(
      async (filter: Api_ExportProjectFilter) => await getAgreementReport(filter),
      [getAgreementReport],
    ),
    requestName: 'GetAgreementsReport',
    onError: useAxiosErrorHandler('Failed to load Agreements Report'),
  });

  const getCompensationReportApi = useApiRequestWrapper<
    (filter: Api_ExportProjectFilter) => Promise<AxiosResponse<Blob, any>>
  >({
    requestFunction: useCallback(
      async (filter: Api_ExportProjectFilter) => await getCompensationReport(filter),
      [getCompensationReport],
    ),
    requestName: 'GetCompensationRequisitionsReport',
    onError: useAxiosErrorHandler('Failed to load Compensation Requisitions Report'),
  });

  const updateAcquisitionFileApi = useApiRequestWrapper<
    (
      acqFile: ApiGen_Concepts_AcquisitionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (
        acqFile: ApiGen_Concepts_AcquisitionFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) => await putAcquisitionFile(acqFile, userOverrideCodes),
      [putAcquisitionFile],
    ),
    requestName: 'UpdateAcquisitionFile',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  const updateAcquisitionPropertiesApi = useApiRequestWrapper<
    (
      acqFile: ApiGen_Concepts_AcquisitionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: ApiGen_Concepts_AcquisitionFile, userOverrideCodes: UserOverrideCode[]) =>
        await putAcquisitionFileProperties(acqFile, userOverrideCodes),
      [putAcquisitionFileProperties],
    ),
    requestName: 'UpdateAcquisitionFileProperties',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  const getAcquisitionPropertiesApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFileProperty[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileProperties(acqFileId),
      [getAcquisitionFileProperties],
    ),
    requestName: 'GetAcquisitionFileProperties',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Properties'),
  });

  const getAcquisitionOwnersApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFileOwner[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileOwners(acqFileId),
      [getAcquisitionFileOwners],
    ),
    requestName: 'GetAcquisitionFileOwners',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Owners'),
  });

  const getAllAcquisitionTeamMembersApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFileTeam[], any>>
  >({
    requestFunction: useCallback(
      async () => await getAllAcquisitionFileTeamMembers(),
      [getAllAcquisitionFileTeamMembers],
    ),
    requestName: 'getAllAcquisitionFileTeamMembers',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Team Members'),
  });

  const getAcquisitionProjectApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_Project, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileProject(acqFileId),
      [getAcquisitionFileProject],
    ),
    requestName: 'GetAcquisitionFileProject',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Project'),
  });

  const getAcquisitionProductApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_Product, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileProduct(acqFileId),
      [getAcquisitionFileProduct],
    ),
    requestName: 'GetAcquisitionFileProduct',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Product'),
  });

  const getAcquisitionChecklistApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_FileChecklistItem[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileChecklist(acqFileId),
      [getAcquisitionFileChecklist],
    ),
    requestName: 'GetAcquisitionFileChecklist',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Checklist'),
  });

  const updateAcquisitionChecklistApi = useApiRequestWrapper<
    (
      acqFile: ApiGen_Concepts_FileWithChecklist,
    ) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: ApiGen_Concepts_FileWithChecklist) =>
        await putAcquisitionFileChecklist(acqFile),
      [putAcquisitionFileChecklist],
    ),
    requestName: 'UpdateAcquisitionFileChecklist',
    onError: useAxiosErrorHandler('Failed to update Acquisition File Checklist'),
    throwError: true,
  });

  const postFileForm8Api = useApiRequestWrapper<
    (
      acqFileId: number,
      form8: ApiGen_Concepts_ExpropriationPayment,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ExpropriationPayment, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, form8: ApiGen_Concepts_ExpropriationPayment) =>
        await postFileForm8(acqFileId, form8),
      [postFileForm8],
    ),
    requestName: 'postFileForm8',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to save Form 8'),
  });

  const getAcquisitionForm8sApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_ExpropriationPayment[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileForm8s(acqFileId),
      [getAcquisitionFileForm8s],
    ),
    requestName: 'getAcquisitionFileForm8s',
    onError: useAxiosErrorHandler(
      'Failed to load acquisition file form 8s. Refresh the page to try again.',
    ),
  });

  const getAcquisitionSubFilesApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFile[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionSubFiles(acqFileId),
      [getAcquisitionSubFiles],
    ),
    requestName: 'getAcquisitionSubFiles',
    onError: useAxiosErrorHandler(
      'Failed to load acquisition file subfiles. Refresh the page to try again.',
    ),
  });

  const getAcquisitionAtTimeApi = useApiRequestWrapper<
    (
      acqFileId: number,
      time: string,
    ) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, time: string) => await getAcquisitionAtTime(acqFileId, time),
      [getAcquisitionAtTime],
    ),
    requestName: 'getAcquisitionAtTime',
    onError: useAxiosErrorHandler(
      'Failed to load historical acquisition file. Refresh the page to try again.',
    ),
  });

  return useMemo(
    () => ({
      addAcquisitionFile: addAcquisitionFileApi,
      getAcquisitionFile: getAcquisitionFileApi,
      getLastUpdatedBy,
      updateAcquisitionFile: updateAcquisitionFileApi,
      updateAcquisitionProperties: updateAcquisitionPropertiesApi,
      getAcquisitionProperties: getAcquisitionPropertiesApi,
      getAcquisitionOwners: getAcquisitionOwnersApi,
      getAllAcquisitionFileTeamMembers: getAllAcquisitionTeamMembersApi,
      getAcquisitionProject: getAcquisitionProjectApi,
      getAcquisitionProduct: getAcquisitionProductApi,
      getAcquisitionFileChecklist: getAcquisitionChecklistApi,
      updateAcquisitionChecklist: updateAcquisitionChecklistApi,
      postAcquisitionForm8: postFileForm8Api,
      getAcquisitionFileForm8s: getAcquisitionForm8sApi,
      getAgreementsReport: getAgreementsReportApi,
      getCompensationReport: getCompensationReportApi,
      getAcquisitionSubFiles: getAcquisitionSubFilesApi,
      getAcquisitionAtTime: getAcquisitionAtTimeApi,
    }),
    [
      addAcquisitionFileApi,
      getAcquisitionFileApi,
      getLastUpdatedBy,
      updateAcquisitionFileApi,
      updateAcquisitionPropertiesApi,
      getAcquisitionPropertiesApi,
      getAcquisitionOwnersApi,
      getAllAcquisitionTeamMembersApi,
      getAcquisitionProjectApi,
      getAcquisitionProductApi,
      getAcquisitionChecklistApi,
      updateAcquisitionChecklistApi,
      postFileForm8Api,
      getAcquisitionForm8sApi,
      getAgreementsReportApi,
      getCompensationReportApi,
      getAcquisitionSubFilesApi,
      getAcquisitionAtTimeApi,
    ],
  );
};
