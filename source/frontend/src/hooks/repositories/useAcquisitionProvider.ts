import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiAcquisitionFile } from '@/hooks/pims-api/useApiAcquisitionFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import {
  Api_AcquisitionFile,
  Api_AcquisitionFileChecklistItem,
  Api_AcquisitionFileOwner,
  Api_AcquisitionFileProperty,
} from '@/models/api/AcquisitionFile';
import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_ExpropriationPayment } from '@/models/api/ExpropriationPayment';
import { Api_Person } from '@/models/api/Person';
import { Api_Product, Api_Project } from '@/models/api/Project';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

const ignoreErrorCodes = [409];

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
    getFileCompensationRequisitions,
    postFileCompensationRequisition,
    getFileCompReqH120s,
    postFileForm8,
    getAcquisitionFileForm8s,
    getAllAcquisitionFileTeamMembers,
    getAgreementReport,
    getCompensationReport,
  } = useApiAcquisitionFile();

  const addAcquisitionFileApi = useApiRequestWrapper<
    (
      acqFile: Api_AcquisitionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_AcquisitionFile, useOverride: UserOverrideCode[] = []) =>
        await postAcquisitionFile(acqFile, useOverride),
      [postAcquisitionFile],
    ),
    requestName: 'AddAcquisitionFile',
    onSuccess: useAxiosSuccessHandler('Acquisition File saved'),
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const getAcquisitionFileApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFile(acqFileId),
      [getAcquisitionFile],
    ),
    requestName: 'RetrieveAcquisitionFile',
    onError: useAxiosErrorHandler('Failed to load Acquisition File'),
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
      acqFile: Api_AcquisitionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_AcquisitionFile, userOverrideCodes: UserOverrideCode[] = []) =>
        await putAcquisitionFile(acqFile, userOverrideCodes),
      [putAcquisitionFile],
    ),
    requestName: 'UpdateAcquisitionFile',
    onSuccess: useAxiosSuccessHandler('Acquisition File updated'),
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const updateAcquisitionPropertiesApi = useApiRequestWrapper<
    (
      acqFile: Api_AcquisitionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_AcquisitionFile, userOverrideCodes: UserOverrideCode[]) =>
        await putAcquisitionFileProperties(acqFile, userOverrideCodes),
      [putAcquisitionFileProperties],
    ),
    requestName: 'UpdateAcquisitionFileProperties',
    onSuccess: useAxiosSuccessHandler('Acquisition File Properties updated'),
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const getAcquisitionPropertiesApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_AcquisitionFileProperty[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileProperties(acqFileId),
      [getAcquisitionFileProperties],
    ),
    requestName: 'GetAcquisitionFileProperties',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Properties'),
  });

  const getAcquisitionOwnersApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_AcquisitionFileOwner[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileOwners(acqFileId),
      [getAcquisitionFileOwners],
    ),
    requestName: 'GetAcquisitionFileOwners',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Owners'),
  });

  const getAllAcquisitionTeamMembersApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_Person[], any>>
  >({
    requestFunction: useCallback(
      async () => await getAllAcquisitionFileTeamMembers(),
      [getAllAcquisitionFileTeamMembers],
    ),
    requestName: 'getAllAcquisitionFileTeamMembers',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Team Members'),
  });

  const getAcquisitionProjectApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_Project, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileProject(acqFileId),
      [getAcquisitionFileProject],
    ),
    requestName: 'GetAcquisitionFileProject',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Project'),
  });

  const getAcquisitionProductApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_Product, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileProduct(acqFileId),
      [getAcquisitionFileProduct],
    ),
    requestName: 'GetAcquisitionFileProduct',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Product'),
  });

  const getAcquisitionChecklistApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_AcquisitionFileChecklistItem[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionFileChecklist(acqFileId),
      [getAcquisitionFileChecklist],
    ),
    requestName: 'GetAcquisitionFileChecklist',
    onError: useAxiosErrorHandler('Failed to retrieve Acquisition File Checklist'),
  });

  const updateAcquisitionChecklistApi = useApiRequestWrapper<
    (acqFile: Api_AcquisitionFile) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_AcquisitionFile) => await putAcquisitionFileChecklist(acqFile),
      [putAcquisitionFileChecklist],
    ),
    requestName: 'UpdateAcquisitionFileChecklist',
    onError: useAxiosErrorHandler('Failed to update Acquisition File Checklist'),
    throwError: true,
  });

  const getAcquisitionCompensationRequisitionsApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_CompensationRequisition[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getFileCompensationRequisitions(acqFileId),
      [getFileCompensationRequisitions],
    ),
    requestName: 'GetAcquisitionCompensationRequisitions',
    onError: useAxiosErrorHandler(
      'Failed to load requisition compensations. Refresh the page to try again.',
    ),
  });

  const getAcquisitionCompReqH120sApi = useApiRequestWrapper<
    (
      acqFileId: number,
      finalOnly: boolean,
    ) => Promise<AxiosResponse<Api_CompensationFinancial[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, finalOnly: boolean) =>
        await getFileCompReqH120s(acqFileId, finalOnly),
      [getFileCompReqH120s],
    ),
    requestName: 'getAcquisitionCompReqH120s',
    onError: useAxiosErrorHandler(
      'Failed to load requisition compensation financials. Refresh the page to try again.',
    ),
  });

  const postFileCompensationRequisitionApi = useApiRequestWrapper<
    (
      acqFileId: number,
      compRequisition: Api_CompensationRequisition,
    ) => Promise<AxiosResponse<Api_CompensationRequisition, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, compRequisition: Api_CompensationRequisition) =>
        await postFileCompensationRequisition(acqFileId, compRequisition),
      [postFileCompensationRequisition],
    ),
    requestName: 'PostFileCompensationRequisition',
    onSuccess: useAxiosSuccessHandler('Compensation requisition saved'),
    onError: useAxiosErrorHandler('Failed to save Compensation requisition'),
  });

  const postFileForm8Api = useApiRequestWrapper<
    (
      acqFileId: number,
      form8: Api_ExpropriationPayment,
    ) => Promise<AxiosResponse<Api_ExpropriationPayment, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, form8: Api_ExpropriationPayment) =>
        await postFileForm8(acqFileId, form8),
      [postFileForm8],
    ),
    requestName: 'postFileForm8',
    onSuccess: useAxiosSuccessHandler('Form 8 saved'),
    onError: useAxiosErrorHandler('Failed to save Form 8'),
  });

  const getAcquisitionForm8sApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_ExpropriationPayment[], any>>
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

  return useMemo(
    () => ({
      addAcquisitionFile: addAcquisitionFileApi,
      getAcquisitionFile: getAcquisitionFileApi,
      updateAcquisitionFile: updateAcquisitionFileApi,
      updateAcquisitionProperties: updateAcquisitionPropertiesApi,
      getAcquisitionProperties: getAcquisitionPropertiesApi,
      getAcquisitionOwners: getAcquisitionOwnersApi,
      getAllAcquisitionFileTeamMembers: getAllAcquisitionTeamMembersApi,
      getAcquisitionProject: getAcquisitionProjectApi,
      getAcquisitionProduct: getAcquisitionProductApi,
      getAcquisitionFileChecklist: getAcquisitionChecklistApi,
      updateAcquisitionChecklist: updateAcquisitionChecklistApi,
      getAcquisitionCompensationRequisitions: getAcquisitionCompensationRequisitionsApi,
      postAcquisitionCompensationRequisition: postFileCompensationRequisitionApi,
      getAcquisitionCompReqH120s: getAcquisitionCompReqH120sApi,
      postAcquisitionForm8: postFileForm8Api,
      getAcquisitionFileForm8s: getAcquisitionForm8sApi,
      getAgreementsReport: getAgreementsReportApi,
      getCompensationReport: getCompensationReportApi,
    }),
    [
      addAcquisitionFileApi,
      getAcquisitionFileApi,
      updateAcquisitionFileApi,
      updateAcquisitionPropertiesApi,
      getAcquisitionPropertiesApi,
      getAcquisitionOwnersApi,
      getAllAcquisitionTeamMembersApi,
      getAcquisitionProjectApi,
      getAcquisitionProductApi,
      getAcquisitionChecklistApi,
      updateAcquisitionChecklistApi,
      getAcquisitionCompensationRequisitionsApi,
      postFileCompensationRequisitionApi,
      getAcquisitionCompReqH120sApi,
      postFileForm8Api,
      getAcquisitionForm8sApi,
      getAgreementsReportApi,
      getCompensationReportApi,
    ],
  );
};
