import axios, { AxiosError } from 'axios';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { IApiError } from 'interfaces/IApiError';
import { Api_Lease } from 'models/api/Lease';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

/**
 * hook that adds a lease.
 */
export const useAddLease = () => {
  const { postLease } = useApiLeases();
  const dispatch = useDispatch();

  const addLease = async (
    lease: Api_Lease,
    setUserOverrideMessage?: (message?: string) => void,
    userOverride: boolean = false,
  ) => {
    try {
      dispatch(showLoading());
      const response = await postLease(lease, userOverride);
      toast.success('Lease/License saved');
      return response?.data;
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        if (axiosError?.response?.status === 409) {
          setUserOverrideMessage && setUserOverrideMessage(axiosError?.response.data.error);
        } else {
          if (axiosError?.response?.status === 400) {
            toast.error(axiosError?.response.data.error);
          } else {
            toast.error('Save error. Check responses and try again.');
          }
          dispatch(
            logError({
              name: 'AddLease',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        }
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  return { addLease };
};
