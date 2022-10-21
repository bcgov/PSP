import { useApiLeaseDeposits } from 'hooks/pims-api/useApiLeaseDeposits';
import { IParentConcurrencyGuard } from 'interfaces/IParentConcurrencyGuard';
import { Api_SecurityDeposit } from 'models/api/SecurityDeposit';
import { useDispatch } from 'react-redux';
import { toast } from 'react-toastify';
import { handleAxiosResponse } from 'utils';

/**
 * hook providing lease deposits methods
 */
export const useLeaseDeposits = () => {
  const { putLeaseDeposit, putLeaseDepositNote, postLeaseDeposit, deleteLeaseDeposit } =
    useApiLeaseDeposits();
  const dispatch = useDispatch();

  const updateLeaseDeposit = async (request: IParentConcurrencyGuard<Api_SecurityDeposit>) => {
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
    }
  };

  const updateLeaseDepositNote = async (request: IParentConcurrencyGuard<{ note: string }>) => {
    try {
      const axiosPromise = putLeaseDepositNote(request);
      const response = await handleAxiosResponse(dispatch, 'UpdateLeaseDepositNote', axiosPromise);
      toast.success('Lease deposit note saved');
      return response;
    } catch (axiosError) {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response?.data.error);
      } else {
        toast.error('Error saving lease deposit note, refresh your page and try again');
      }
    }
  };

  const removeLeaseDeposit = async (request: IParentConcurrencyGuard<Api_SecurityDeposit>) => {
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
    }
  };

  return { updateLeaseDeposit, removeLeaseDeposit, updateLeaseDepositNote };
};
