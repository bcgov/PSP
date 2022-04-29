import { AxiosError } from 'axios';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import { IApiError } from 'interfaces/IApiError';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { toast } from 'react-toastify';

/**
 * hook that updates a research file.
 */
export const useUpdateResearch = () => {
  const { putResearchFile } = useApiResearchFile();

  const { refresh } = useApiRequestWrapper({
    requestFunction: async (researchFile: Api_ResearchFile) => await putResearchFile(researchFile),
    requestName: 'UpdateResearchFile',
    onSuccess: () => toast.success('Research File updated'),
    onError: (axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Save error. Check responses and try again.');
      }
    },
  });

  return { updateResearchFile: refresh };
};
