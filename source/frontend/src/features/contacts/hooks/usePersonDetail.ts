import axios, { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { IEditablePerson } from '@/interfaces/editable-contact';
import { IApiError } from '@/interfaces/IApiError';
import { logError } from '@/store/slices/network/networkSlice';

/**
 * hook that fetches the person given the person's id.
 * @param personId
 */
export const usePersonDetail = (personId?: number | null) => {
  const [person, setPerson] = useState<IEditablePerson>();
  const { getPerson } = useApiContacts();
  const dispatch = useDispatch();

  useEffect(() => {
    const getPersonById = async (id: number) => {
      try {
        dispatch(showLoading());
        const { data } = await getPerson(id);
        setPerson(data);
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          toast.error('Failed to get person details, reload this page to try again.');
          dispatch(
            logError({
              name: 'GetPerson',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        }
      } finally {
        dispatch(hideLoading());
      }
    };

    if (personId) {
      getPersonById(personId);
    } else {
      toast.error(
        'No valid person id provided, go back to the contact list and select a valid contact.',
      );
    }
  }, [getPerson, personId, dispatch]);

  return { person };
};
