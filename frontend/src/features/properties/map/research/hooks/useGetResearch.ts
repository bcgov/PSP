import axios, { AxiosError } from 'axios';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import { IApiError } from 'interfaces/IApiError';
import { useMemo } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

/**
 * hook that retrieves a research file.
 */
export const useGetResearch = () => {
  const { getResearchFile } = useApiResearchFile();
  const dispatch = useDispatch();

  const retrieveResearchFile = useMemo(
    () => async (researchFileId: number) => {
      try {
        dispatch(showLoading());
        const response = await getResearchFile(researchFileId);
        toast.success('Research File retrieved');
        return response?.data;
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;

          if (axiosError?.response?.status === 400) {
            toast.error(axiosError?.response.data.error);
          } else {
            toast.error('Retrieve research file error. Check responses and try again.');
          }
          dispatch(
            logError({
              name: 'retrieveResearchFile',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        }
      } finally {
        dispatch(hideLoading());
      }
    },
    [dispatch, getResearchFile],
  );

  return { retrieveResearchFile };
};
