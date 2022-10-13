import { useApiLeasePayments } from 'hooks/pims-api/useApiLeasePayments';
import { ILeasePayment } from 'interfaces/ILeasePayment';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { handleAxiosResponse } from 'utils';

/**
 * hook providing lease payment modification methods
 */
export const useLeasePayments = () => {
  const { putLeasePayment, postLeasePayment, deleteLeasePayment } = useApiLeasePayments();
  const dispatch = useDispatch();

  const updateLeasePayment = useCallback(
    async (payment: ILeasePayment) => {
      try {
        const axiosPromise = payment.id ? putLeasePayment(payment) : postLeasePayment(payment);
        const response = await handleAxiosResponse(dispatch, 'UpdateLeasePayment', axiosPromise);
        toast.success('Lease payment saved');
        return response;
      } catch (axiosError) {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response?.data.error);
        } else {
          toast.error('Error saving lease payment, refresh your page and try again');
        }
      } finally {
        dispatch(hideLoading());
      }
    },
    [dispatch, postLeasePayment, putLeasePayment],
  );

  const removeLeasePayment = useCallback(
    async (payment: ILeasePayment) => {
      try {
        const axiosPromise = deleteLeasePayment(payment);
        const response = await handleAxiosResponse(dispatch, 'DeleteLeasePayment', axiosPromise);
        toast.success('Lease payment deleted');
        return response;
      } catch (axiosError) {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response?.data.error);
        } else {
          toast.error('Error deleting lease payment, refresh your page and try again');
        }
      } finally {
        dispatch(hideLoading());
      }
    },
    [deleteLeasePayment, dispatch],
  );

  return { updateLeasePayment, removeLeasePayment };
};
