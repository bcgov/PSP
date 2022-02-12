import axios, { AxiosError } from 'axios';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { IEditableOrganization, IEditablePerson } from 'interfaces/editable-contact';
import { IApiError } from 'interfaces/IApiError';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

/**
 * hook that updates a contact.
 */
export const useUpdateContact = () => {
  const { putPerson, putOrganization } = useApiContacts();
  const dispatch = useDispatch();

  const updatePerson = async (person: IEditablePerson) => {
    try {
      dispatch(showLoading());
      const response = await putPerson(person);
      toast.success('Contact saved');
      return response?.data;
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Unable to save. Please try again.');
        }
        dispatch(
          logError({
            name: 'UpdatePerson',
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  const updateOrganization = async (organization: IEditableOrganization) => {
    try {
      dispatch(showLoading());
      const response = await putOrganization(organization);
      toast.success('Contact saved');
      return response?.data;
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Unable to save. Please try again.');
        }
        dispatch(
          logError({
            name: 'UpdateOrganization',
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
      }
    } finally {
      dispatch(hideLoading());
    }
  };

  return { updatePerson, updateOrganization };
};

export default useUpdateContact;
