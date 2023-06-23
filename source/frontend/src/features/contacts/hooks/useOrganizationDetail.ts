import axios, { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { IEditableOrganization } from '@/interfaces/editable-contact';
import { IApiError } from '@/interfaces/IApiError';
import { logError } from '@/store/slices/network/networkSlice';

/**
 * hook that fetches an organization with the supplied id.
 * @param organizationId
 */
export const useOrganizationDetail = (organizationId?: number) => {
  const [organization, setOrganization] = useState<IEditableOrganization>();
  const { getOrganization } = useApiContacts();
  const dispatch = useDispatch();

  useEffect(() => {
    const getOrganizationById = async (id: number) => {
      try {
        dispatch(showLoading());
        const { data } = await getOrganization(id);
        setOrganization(data);
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          toast.error('Failed to get organization details, reload this page to try again.');
          dispatch(
            logError({
              name: 'GetOrganizationById',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        }
      } finally {
        dispatch(hideLoading());
      }
    };

    if (organizationId) {
      getOrganizationById(organizationId);
    } else {
      toast.error(
        'No valid organization id provided, go back to the contact list and select a valid contact.',
      );
    }
  }, [getOrganization, organizationId, dispatch]);

  return { organization };
};
