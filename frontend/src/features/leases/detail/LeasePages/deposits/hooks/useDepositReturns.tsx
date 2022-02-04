import { useApiLeaseDepositReturns } from 'hooks/pims-api/useApiLeaseDepositsReturn';
import { ILeaseSecurityDepositReturn } from 'interfaces';
import { IParentConcurrencyGuard } from 'interfaces/IParentConcurrencyGuard';
import { useDispatch } from 'react-redux';
import { hideLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { handleAxiosResponse } from 'utils';

/**
 * hook providing lease return deposits methods
 */
export const useLeaseDepositReturns = () => {
  const {
    putLeaseDepositReturn,
    postLeaseDepositReturn,
    deleteLeaseDepositReturn,
  } = useApiLeaseDepositReturns();
  const dispatch = useDispatch();

  const updateLeaseDepositReturn = async (
    request: IParentConcurrencyGuard<ILeaseSecurityDepositReturn>,
  ) => {
    try {
      const axiosPromise = request.payload.id
        ? putLeaseDepositReturn(request)
        : postLeaseDepositReturn(request);
      const response = await handleAxiosResponse(
        dispatch,
        'UpdateLeaseReturnDeposit',
        axiosPromise,
      );
      toast.success('Lease return deposit saved');
      return response;
    } catch (axiosError) {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response?.data.error);
      } else {
        toast.error('Error saving lease return deposit, refresh your page and try again');
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  const removeLeaseDepositReturn = async (
    request: IParentConcurrencyGuard<ILeaseSecurityDepositReturn>,
  ) => {
    try {
      const axiosPromise = deleteLeaseDepositReturn(request);
      const response = await handleAxiosResponse(
        dispatch,
        'DeleteLeaseReturnDeposit',
        axiosPromise,
      );
      toast.success('Lease return deposit deleted');
      return response;
    } catch (axiosError) {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response?.data.error);
      } else {
        toast.error('Error deleting lease return  deposit, refresh your page and try again');
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  return { updateLeaseDepositReturn, removeLeaseDepositReturn };
};
