import axios, { AxiosError } from 'axios';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { logError } from '@/store/slices/network/networkSlice';

/**
 * hook that adds a contact.
 */
const useAddContact = () => {
  const { postPerson, postOrganization } = useApiContacts();
  const dispatch = useDispatch();

  const addPerson = async (
    person: ApiGen_Concepts_Person,
    needsUserAction: (needsAction: boolean) => void,
    userOverride = false,
  ) => {
    try {
      dispatch(showLoading());
      const response = await postPerson(person, userOverride);
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
            toast.error('Unable to save. Please try again.');
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

  const addOrganization = async (
    organization: ApiGen_Concepts_Organization,
    needsUserAction: (needsAction: boolean) => void,
    userOverride = false,
  ) => {
    try {
      dispatch(showLoading());
      const response = await postOrganization(organization, userOverride);
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
              name: 'AddOrganization',
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

  return { addPerson, addOrganization };
};

export default useAddContact;
