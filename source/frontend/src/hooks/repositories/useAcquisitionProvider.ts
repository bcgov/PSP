import { AxiosResponse } from 'axios';
import { useApiAcquisitionFile } from 'hooks/pims-api/useApiAcquisitionFile';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import {
  Api_AcquisitionFile,
  Api_AcquisitionFileChecklistItem,
  Api_AcquisitionFileOwner,
  Api_AcquisitionFileProperty,
} from 'models/api/AcquisitionFile';
import { Api_Compensation } from 'models/api/Compensation';
import { Api_Product, Api_Project } from 'models/api/Project';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

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
  } = useApiAcquisitionFile();

  const addAcquisitionFileApi = useApiRequestWrapper<
    (acqFile: Api_AcquisitionFile) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_AcquisitionFile) => await postAcquisitionFile(acqFile),
      [postAcquisitionFile],
    ),
    requestName: 'AddAcquisitionFile',
    onSuccess: useAxiosSuccessHandler('Acquisition File saved'),
    onError: useAxiosErrorHandler('Failed to save Acquisition File'),
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

  const updateAcquisitionFileApi = useApiRequestWrapper<
    (
      acqFile: Api_AcquisitionFile,
      userOverride: boolean,
    ) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_AcquisitionFile, userOverride = false) =>
        await putAcquisitionFile(acqFile, userOverride),
      [putAcquisitionFile],
    ),
    requestName: 'UpdateAcquisitionFile',
    onSuccess: useAxiosSuccessHandler('Acquisition File updated'),
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const updateAcquisitionPropertiesApi = useApiRequestWrapper<
    (acqFile: Api_AcquisitionFile) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_AcquisitionFile) => await putAcquisitionFileProperties(acqFile),
      [putAcquisitionFileProperties],
    ),
    requestName: 'UpdateAcquisitionFileProperties',
    onSuccess: useAxiosSuccessHandler('Acquisition File Properties updated'),
    onError: useAxiosErrorHandler('Failed to update Acquisition File Properties'),
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
    (acqFileId: number) => Promise<AxiosResponse<Api_Compensation[], any>>
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

  const postFileCompensationRequisitionApi = useApiRequestWrapper<
    (
      acqFileId: number,
      compRequisition: Api_Compensation,
    ) => Promise<AxiosResponse<Api_Compensation, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, compRequisition: Api_Compensation) =>
        await postFileCompensationRequisition(acqFileId, compRequisition),
      [postFileCompensationRequisition],
    ),
    requestName: 'PostFileCompensationRequisition',
    onSuccess: useAxiosSuccessHandler('Compensation requisition saved'),
    onError: useAxiosErrorHandler('Failed to save Compensation requisition'),
  });

  return useMemo(
    () => ({
      addAcquisitionFile: addAcquisitionFileApi,
      getAcquisitionFile: getAcquisitionFileApi,
      updateAcquisitionFile: updateAcquisitionFileApi,
      updateAcquisitionProperties: updateAcquisitionPropertiesApi,
      getAcquisitionProperties: getAcquisitionPropertiesApi,
      getAcquisitionOwners: getAcquisitionOwnersApi,
      getAcquisitionProject: getAcquisitionProjectApi,
      getAcquisitionProduct: getAcquisitionProductApi,
      getAcquisitionFileChecklist: getAcquisitionChecklistApi,
      updateAcquisitionChecklist: updateAcquisitionChecklistApi,
      getAcquisitionCompensationRequisitions: getAcquisitionCompensationRequisitionsApi,
      postAcquisitionCompensationRequisition: postFileCompensationRequisitionApi,
    }),
    [
      addAcquisitionFileApi,
      getAcquisitionFileApi,
      updateAcquisitionFileApi,
      updateAcquisitionPropertiesApi,
      getAcquisitionPropertiesApi,
      getAcquisitionOwnersApi,
      getAcquisitionProjectApi,
      getAcquisitionProductApi,
      getAcquisitionChecklistApi,
      updateAcquisitionChecklistApi,
      getAcquisitionCompensationRequisitionsApi,
      postFileCompensationRequisitionApi,
    ],
  );
};
