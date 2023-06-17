import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiResearchFile } from '@/hooks/pims-api/useApiResearchFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { Api_ResearchFile, Api_ResearchFileProperty } from '@/models/api/ResearchFile';

/**
 * hook that updates a property research file.
 */
export const useUpdatePropertyResearch = () => {
  const { putPropertyResearchFile } = useApiResearchFile();

  const { execute } = useApiRequestWrapper<
    (researchFile: Api_ResearchFileProperty) => Promise<AxiosResponse<Api_ResearchFile, any>>
  >({
    requestFunction: useCallback(
      async (researchFile: Api_ResearchFileProperty) => await putPropertyResearchFile(researchFile),
      [putPropertyResearchFile],
    ),
    requestName: 'UpdatePropertyResearchFile',
    onSuccess: useCallback(() => toast.success('Property Research File updated'), []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Save error. Check responses and try again.');
      }
    }, []),
  });

  return { updatePropertyResearchFile: execute };
};
