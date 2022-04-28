import axios, { AxiosError } from 'axios';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import { IApiError } from 'interfaces/IApiError';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

/**
 * hook that updates a research file.
 */
export const useUpdateResearch = () => {
  const { putResearchFile } = useApiResearchFile();
  const dispatch = useDispatch();

  const updateResearchFile = async (researchFile: Api_ResearchFile) => {
    try {
      dispatch(showLoading());
      const response = await putResearchFile(researchFile);
      toast.success('Research File updated');
      return response?.data;
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;

        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Save error. Check responses and try again.');
        }
        dispatch(
          logError({
            name: 'updateResearchFile',
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  return { updateResearchFile };
};
