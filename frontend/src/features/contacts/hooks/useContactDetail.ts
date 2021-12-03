import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { IContact } from 'interfaces/IContact';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

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
        toast.error('Failed to load contact, reload this page to try again.');
        dispatch(
          logError({
            name: 'ContactLoad',
            status: e?.response?.status,
            error: e,
          }),
        );
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
