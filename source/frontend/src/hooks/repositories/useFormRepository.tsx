import { AxiosResponse } from 'axios';
import { FileTypes } from 'constants/fileTypes';
import { postFileForm } from 'hooks/pims-api/useApiForm';
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

  return useMemo(
    () => ({
      addFilesForm: addFileFormApi,
    }),
    [addFileFormApi],
  );
};
