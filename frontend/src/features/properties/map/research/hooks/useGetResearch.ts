import { AxiosError } from 'axios';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import { IApiError } from 'interfaces/IApiError';
import { toast } from 'react-toastify';

import { useApiRequestWrapper } from './../../../../../hooks/pims-api/useApiRequestWrapper';

/**
 * hook that retrieves a research file.
 */
export const useGetResearch = () => {
  const { getResearchFile } = useApiResearchFile();
  const { refresh } = useApiRequestWrapper({
    requestFunction: async (researchFileId: number) => await getResearchFile(researchFileId),
    requestName: 'retrieveResearchFile',
    onSuccess: () => toast.success('Research File retrieved'),
    onError: (axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Retrieve research file error. Check responses and try again.');
      }
    },
  });
  return { retrieveResearchFile: refresh };
};
