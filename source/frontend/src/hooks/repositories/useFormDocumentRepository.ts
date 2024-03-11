import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { FileTypes } from '@/constants/fileTypes';
import {
  deleteFileForm,
  getFileForm,
  getFileForms,
  getFormDocumentTypesApi,
  postFileFormApi,
} from '@/hooks/pims-api/useApiFormDocument';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_FormDocumentFile } from '@/models/api/generated/ApiGen_Concepts_FormDocumentFile';
import { ApiGen_Concepts_FormDocumentType } from '@/models/api/generated/ApiGen_Concepts_FormDocumentType';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that interacts with the Form documents API.
 */
export const useFormDocumentRepository = () => {
  const getFormDocumentTypes = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_FormDocumentType[], any>>
  >({
    requestFunction: useCallback(async () => await getFormDocumentTypesApi(), []),
    requestName: 'getFormDocumentTypes',
    onSuccess: useAxiosSuccessHandler(),
    invoke: true,
    onError: useAxiosErrorHandler(
      'Failed to load form document types. Either refresh the page to try again or try and load a different activity.',
    ),
  });

  const addFormDocumentFile = useApiRequestWrapper<
    (
      fileType: FileTypes,
      activity: ApiGen_Concepts_FormDocumentFile,
    ) => Promise<AxiosResponse<ApiGen_Concepts_FormDocumentFile, any>>
  >({
    requestFunction: useCallback(
      async (fileType: FileTypes, fileForm: ApiGen_Concepts_FormDocumentFile) =>
        await postFileFormApi(fileType, fileForm),
      [],
    ),
    requestName: 'addFormDocumentFile',
    onSuccess: useAxiosSuccessHandler('Form Document added to file'),
    onError: useAxiosErrorHandler(),
  });

  const getFileFormsApi = useApiRequestWrapper<
    (
      fileType: FileTypes,
      fileId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_FormDocumentFile[], any>>
  >({
    requestFunction: useCallback(
      async (fileType: FileTypes, fileId: number) => await getFileForms(fileType, fileId),
      [],
    ),
    requestName: 'getFileForms',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load form templates. Refresh the page to try again.'),
  });
  const getFileFormApi = useApiRequestWrapper<
    (
      fileType: FileTypes,
      formFileId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_FormDocumentFile, any>>
  >({
    requestFunction: useCallback(
      async (fileType: FileTypes, fileId: number) => await getFileForm(fileType, fileId),
      [],
    ),
    requestName: 'getFileForm',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load form. Refresh the page to try again.'),
  });
  const deleteFileFormApi = useApiRequestWrapper<
    (fileType: FileTypes, fileId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (fileType: FileTypes, fileId: number) => await deleteFileForm(fileType, fileId),
      [],
    ),
    requestName: 'deleteFileForm',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to delete form template. Refresh the page to try again.'),
  });

  return useMemo(
    () => ({
      getFormDocumentTypes: getFormDocumentTypes,
      addFilesForm: addFormDocumentFile,
      getFileForms: getFileFormsApi,
      getFileForm: getFileFormApi,
      deleteFileForm: deleteFileFormApi,
    }),
    [getFormDocumentTypes, addFormDocumentFile, getFileFormsApi, deleteFileFormApi, getFileFormApi],
  );
};
