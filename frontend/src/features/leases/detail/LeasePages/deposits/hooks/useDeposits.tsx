import { useApiLeaseDeposits } from 'hooks/pims-api/useApiLeaseDeposits';
import { ILeaseSecurityDeposit } from 'interfaces';
import { IParentConcurrencyGuard } from 'interfaces/IParentConcurrencyGuard';
import { useDispatch } from 'react-redux';
import { hideLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { handleAxiosResponse } from 'utils';

/**
 * hook providing lease deposits methods
 */
export const useLeaseDeposits = () => {
  const { putLeaseDeposit, postLeaseDeposit, deleteLeaseDeposit } = useApiLeaseDeposits();
  const dispatch = useDispatch();

  const updateLeaseDeposit = async (request: IParentConcurrencyGuard<ILeaseSecurityDeposit>) => {
    try {
      const axiosPromise = request.payload.id
        ? putLeaseDeposit(request)
        : postLeaseDeposit(request);
      const response = await handleAxiosResponse(dispatch, 'UpdateLeaseDeposit', axiosPromise);
      toast.success('Lease deposit saved');
      return response;
    } catch (axiosError) {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response?.data.error);
      } else {
        toast.error('Error saving lease deposit, refresh your page and try again');
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  const removeLeaseDeposit = async (request: IParentConcurrencyGuard<ILeaseSecurityDeposit>) => {
    try {
      const axiosPromise = deleteLeaseDeposit(request);
      const response = await handleAxiosResponse(dispatch, 'DeleteLeaseDeposit', axiosPromise);
      toast.success('Lease deposit deleted');
      return response;
    } catch (axiosError) {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response?.data.error);
      } else {
        toast.error('Error deleting lease deposit, refresh your page and try again');
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  return { updateLeaseDeposit, removeLeaseDeposit };
};
