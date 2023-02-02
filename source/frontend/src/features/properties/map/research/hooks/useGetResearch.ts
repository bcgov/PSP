import { AxiosError, AxiosResponse } from 'axios';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import { IApiError } from 'interfaces/IApiError';
import { Api_ResearchFile, Api_ResearchFileProperty } from 'models/api/ResearchFile';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that retrieves a research file.
 */
export const useGetResearch = () => {
  const { getResearchFile } = useApiResearchFile();
  const retrieveResearchFile = useApiRequestWrapper<
    (researchFileId: number) => Promise<AxiosResponse<Api_ResearchFile, any>>
  >({
    requestFunction: useCallback(
      async (researchFileId: number) => await getResearchFile(researchFileId),
      [getResearchFile],
    ),
    requestName: 'retrieveResearchFile',
    onSuccess: useCallback(() => toast.success('Research File retrieved'), []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Retrieve research file error. Check responses and try again.');
      }
    }, []),
  });

  const { getResearchFileProperties } = useApiResearchFile();
  const retrieveResearchFileProperties = useApiRequestWrapper<
    (researchFileId: number) => Promise<AxiosResponse<Api_ResearchFileProperty[], any>>
  >({
    requestFunction: useCallback(
      async (researchFileId: number) => await getResearchFileProperties(researchFileId),
      [getResearchFileProperties],
    ),
    requestName: 'retrieveResearchFileProperties',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Retrieve research file properties error. Check responses and try again.');
      }
    }, []),
  });
  return { retrieveResearchFile, retrieveResearchFileProperties };
};
