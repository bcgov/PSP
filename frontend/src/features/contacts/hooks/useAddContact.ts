import axios, { AxiosError } from 'axios';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { IApiError } from 'interfaces/IApiError';
import { ICreatePerson } from 'interfaces/ICreateContact';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

/**
 * hook that adds a contact.
 */
const useAddContact = () => {
  const { postPerson } = useApiContacts();
  const dispatch = useDispatch();

  const addPerson = async (
    person: ICreatePerson,
    needsUserAction: (needsAction: boolean) => void,
    userOverride: boolean = false,
  ) => {
    try {
      dispatch(showLoading());
      const response = await postPerson(person, userOverride);
      toast.success('Contact/Person saved');
      return response?.data;
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        if (axiosError?.response?.status === 409) {
          needsUserAction(true);
        } else {
          if (axiosError?.response?.status === 400) {
            toast.error(axiosError?.response.data.error);
          } else {
            toast.error('Save error. Check responses and try again.');
          }
          dispatch(
            logError({
              name: 'AddPerson',
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

  return { addPerson };
};

export default useAddContact;
