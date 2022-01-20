import axios, { AxiosError } from 'axios';
import { useApiInsurances } from 'hooks/pims-api/useApiInsurances';
import { IInsurance } from 'interfaces';
import { IBatchUpdateRequest } from 'interfaces/batchUpdate';
import { IApiError } from 'interfaces/IApiError';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

/**
 * hook that creates or updates insurances.
 */
export const useUpdateInsurance = () => {
  const { postInsuranceBatch } = useApiInsurances();
  const dispatch = useDispatch();

  const batchUpdateInsurances = async (
    leaseId: number,
    updateRequest: IBatchUpdateRequest<IInsurance>,
  ) => {
    try {
      dispatch(showLoading());
      const response = await postInsuranceBatch(leaseId, updateRequest);
      toast.success('Insurance(s) saved');
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
            name: 'BatchUpdateInsurances',
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  return { batchUpdateInsurances };
};
