import { AxiosResponse } from 'axios';
import { getFormDocumentTypesApi } from 'hooks/pims-api/useApiFormDocument';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_FormDocumentType } from 'models/api/formDocument';
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
    requestName: 'GetFormDocumentTypes',
    onSuccess: useAxiosSuccessHandler(),
    invoke: true,
    onError: useAxiosErrorHandler(
      'Failed to load activity. Either refresh the page to try again or try and load a different activity.',
    ),
  });

  return useMemo(
    () => ({
      getFormDocumentTypes: getFormDocumentTypes,
    }),
    [getFormDocumentTypes],
  );
};
