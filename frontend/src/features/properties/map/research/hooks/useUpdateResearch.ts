import { AxiosError } from 'axios';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import { IApiError } from 'interfaces/IApiError';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that updates a research file.
 */
export const useUpdateResearch = () => {
  const { putResearchFile } = useApiResearchFile();

  const { refresh } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (researchFile: Api_ResearchFile) => await putResearchFile(researchFile),
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

  return { updateResearchFile: refresh };
};
