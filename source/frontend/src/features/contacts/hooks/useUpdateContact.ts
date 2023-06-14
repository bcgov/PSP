import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IEditableOrganization, IEditablePerson } from '@/interfaces/editable-contact';
import { IApiError } from '@/interfaces/IApiError';

/**
 * hook that updates a contact.
 */
export const useUpdateContact = () => {
  const { putPerson, putOrganization } = useApiContacts();

  const onSuccess = useCallback(() => toast.success('Contact saved'), []);
  const onError = useCallback((axiosError: AxiosError<IApiError>) => {
    if (axiosError?.response?.status === 400) {
      toast.error(axiosError?.response.data.error);
    } else {
      toast.error('Unable to save. Please try again.');
    }
  }, []);

  const { execute: updatePerson } = useApiRequestWrapper<
    (person: IEditablePerson) => Promise<AxiosResponse<IEditablePerson, any>>
  >({
    requestFunction: useCallback(
      async (person: IEditablePerson) => await putPerson(person),
      [putPerson],
    ),
    requestName: 'UpdatePerson',
    onSuccess: onSuccess,
    onError: onError,
  });

  const { execute: updateOrganization } = useApiRequestWrapper<
    (person: IEditableOrganization) => Promise<AxiosResponse<IEditableOrganization, any>>
  >({
    requestFunction: useCallback(
      async (organization: IEditableOrganization) => await putOrganization(organization),
      [putOrganization],
    ),
    requestName: 'UpdateOrganization',
    onSuccess: onSuccess,
    onError: onError,
  });

  return { updatePerson, updateOrganization };
};

export default useUpdateContact;
