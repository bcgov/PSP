import axios, { AxiosError } from 'axios';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { ILease } from 'interfaces';
import { IApiError } from 'interfaces/IApiError';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

/**
 * hook that updates a lease.
 * @param leaseId
 */
export const useUpdateLease = () => {
  const { putLease } = useApiLeases();
  const dispatch = useDispatch();

  const updateLease = async (
    lease: ILease,
    setUserOverride?: (userOverride?: string) => void,
    userOverride: boolean = false,
    subRoute?: string,
  ) => {
    if (lease.id === undefined) {
      throw Error('Cannot update a lease with no id.');
    }
    try {
      dispatch(showLoading());
      const response = await putLease(lease, subRoute, userOverride);
      toast.success('Lease/License saved');
      return response?.data;
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        if (axiosError?.response?.status === 409) {
          setUserOverride && setUserOverride(axiosError?.response.data.error);
        } else if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Save error. Check responses and try again.');
        }

        dispatch(
          logError({
            name: 'UpdateLease',
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  return { updateLease };
};
