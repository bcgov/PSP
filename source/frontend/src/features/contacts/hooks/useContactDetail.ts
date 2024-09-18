import axios, { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { logError } from '@/store/slices/network/networkSlice';

export interface IContact {
  person: ApiGen_Concepts_Person | null;
  organization: ApiGen_Concepts_Organization | null;
}

/**
 * hook that fetches the lease given the lease id.
 * @param leaseId
 */
export const useContactDetail = (contactId?: string) => {
  const [contact, setContact] = useState<IContact>();
  const { getPersonConcept, getOrganizationConcept } = useApiContacts();
  const dispatch = useDispatch();

  useEffect(() => {
    const getContactById = async (id: string) => {
      try {
        const contact: IContact = { person: null, organization: null };
        const idNumber = +id.substring(1);
        dispatch(showLoading());
        if (id.startsWith('P')) {
          contact.person = (await getPersonConcept(idNumber))?.data;
        } else {
          contact.organization = (await getOrganizationConcept(idNumber))?.data;
        }

        setContact(contact);
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
  }, [contactId, dispatch, getPersonConcept, getOrganizationConcept]);

  return { contact };
};
