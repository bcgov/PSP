import { useApiLeaseTerms } from 'hooks/pims-api/useApiLeaseTerms';
import { ILeaseTerm } from 'interfaces/ILeaseTerm';
import { useDispatch } from 'react-redux';
import { hideLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { handleAxiosResponse } from 'utils';

/**
 * hook providing lease modification methods
 */
export const useLeaseTerms = () => {
  const { putLeaseTerm, postLeaseTerm, deleteLeaseTerm } = useApiLeaseTerms();
  const dispatch = useDispatch();

  const updateLeaseTerm = async (term: ILeaseTerm) => {
    term = { ...term, payments: [] }; // Do not send payment information, this api only updates terms.
    try {
      const axiosPromise = term.id ? putLeaseTerm(term) : postLeaseTerm(term);
      const response = await handleAxiosResponse(dispatch, 'UpdateLeaseTerm', axiosPromise);
      toast.success('Lease term saved');
      return response;
    } catch (axiosError) {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response?.data.error);
      } else {
        toast.error('Error saving lease term, refresh your page and try again');
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  const removeLeaseTerm = async (leaseTerm: ILeaseTerm) => {
    try {
      const axiosPromise = deleteLeaseTerm(leaseTerm);
      const response = await handleAxiosResponse(dispatch, 'DeleteLeaseTerm', axiosPromise);
      toast.success('Lease term deleted');
      return response;
    } catch (axiosError) {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response?.data.error);
      } else {
        toast.error('Error deleting lease term, refresh your page and try again');
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  return { updateLeaseTerm, removeLeaseTerm };
};
