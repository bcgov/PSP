import { AxiosResponse } from 'axios';
import { FileTypes } from 'constants/fileTypes';
import { deleteFileForm, getFileForm, getFileForms, postFileForm } from 'hooks/pims-api/useApiForm';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_FileForm } from 'models/api/Form';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that interacts with the forms api.
 */
export const useFormRepository = () => {
  const addFileFormApi = useApiRequestWrapper<
    (fileType: FileTypes, activity: Api_FileForm) => Promise<AxiosResponse<Api_FileForm, any>>
  >({
    requestFunction: useCallback(
      async (fileType: FileTypes, fileForm: Api_FileForm) => await postFileForm(fileType, fileForm),
      [],
    ),
    requestName: 'addFileForm',
    onSuccess: useAxiosSuccessHandler('Form added to file'),
    onError: useAxiosErrorHandler(),
  });
  const getFileFormsApi = useApiRequestWrapper<
    (fileType: FileTypes, fileId: number) => Promise<AxiosResponse<Api_FileForm[], any>>
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
    (fileType: FileTypes, formFileId: number) => Promise<AxiosResponse<Api_FileForm, any>>
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
      addFileForm: addFileFormApi,
      getFileForms: getFileFormsApi,
      getFileForm: getFileFormApi,
      deleteFileForm: deleteFileFormApi,
    }),
    [addFileFormApi, getFileFormsApi, deleteFileFormApi, getFileFormApi],
  );
};
