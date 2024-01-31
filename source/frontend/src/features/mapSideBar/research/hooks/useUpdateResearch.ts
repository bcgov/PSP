import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiResearchFile } from '@/hooks/pims-api/useApiResearchFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';

/**
 * hook that updates a research file.
 */
export const useUpdateResearch = () => {
  const { putResearchFile } = useApiResearchFile();

  const { execute } = useApiRequestWrapper<
    (
      researchFile: ApiGen_Concepts_ResearchFile,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ResearchFile, any>>
  >({
    requestFunction: useCallback(
      async (researchFile: ApiGen_Concepts_ResearchFile) => await putResearchFile(researchFile),
      [putResearchFile],
    ),
    requestName: 'UpdateResearchFile',
    onSuccess: useCallback(() => toast.success('Research File updated'), []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Save error. Check responses and try again.');
      }
    }, []),
  });

  return { updateResearchFile: execute };
};
