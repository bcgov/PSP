import { AxiosResponse } from 'axios';
import { FileTypes } from 'constants/fileTypes';
import { getFormDocumentTypesApi, postFileFormApi } from 'hooks/pims-api/useApiFormDocument';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_FormDocumentFile, Api_FormDocumentType } from 'models/api/FormDocument';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that interacts with the Form documents API.
 */
export const useFormDocumentRepository = () => {
  const getFormDocumentTypes = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_FormDocumentType[], any>>
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
      activity: Api_FormDocumentFile,
    ) => Promise<AxiosResponse<Api_FormDocumentFile, any>>
  >({
    requestFunction: useCallback(
      async (fileType: FileTypes, fileForm: Api_FormDocumentFile) =>
        await postFileFormApi(fileType, fileForm),
      [],
    ),
    requestName: 'addFormDocumentFile',
    onSuccess: useAxiosSuccessHandler('Form Document added to file'),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      getFormDocumentTypes: getFormDocumentTypes,
      addFilesForm: addFormDocumentFile,
    }),
    [getFormDocumentTypes, addFormDocumentFile],
  );
};
