import axios, { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { IApiError } from '@/interfaces/IApiError';
import { IContact } from '@/interfaces/IContact';
import { logError } from '@/store/slices/network/networkSlice';

/**
 * hook that fetches the lease given the lease id.
 * @param leaseId
 */
export const useContactDetail = (contactId?: string) => {
  const [contact, setContact] = useState<IContact>();
  const { getContact } = useApiContacts();
  const dispatch = useDispatch();

  useEffect(() => {
    const getContactById = async (id: string) => {
      try {
        dispatch(showLoading());
        const { data } = await getContact(id);
        setContact(data);
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          toast.error('Failed to load contact, reload this page to try again.');
          dispatch(
            logError({
              name: 'GetContactById',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        }
      } finally {
        dispatch(hideLoading());
      }
    };
    if (contactId) {
      getContactById(contactId);
    } else {
      toast.error(
        'No valid contact id provided, go back to the contact list and select a valid contact.',
      );
    }
  }, [getContact, contactId, dispatch]);

  return { contact };
};
