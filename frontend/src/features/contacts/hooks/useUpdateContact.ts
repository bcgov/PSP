import { AxiosError } from 'axios';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IEditableOrganization, IEditablePerson } from 'interfaces/editable-contact';
import { IApiError } from 'interfaces/IApiError';
import { toast } from 'react-toastify';

/**
 * hook that updates a contact.
 */
export const useUpdateContact = () => {
  const { putPerson, putOrganization } = useApiContacts();

  const onSuccess = () => toast.success('Contact saved');
  const onError = (axiosError: AxiosError<IApiError>) => {
    if (axiosError?.response?.status === 400) {
      toast.error(axiosError?.response.data.error);
    } else {
      toast.error('Unable to save. Please try again.');
    }
  };

  const { refresh: updatePerson } = useApiRequestWrapper({
    requestFunction: async (person: IEditablePerson) => await putPerson(person),
    requestName: 'UpdatePerson',
    onSuccess: onSuccess,
    onError: onError,
  });

  const { refresh: updateOrganization } = useApiRequestWrapper({
    requestFunction: async (organization: IEditableOrganization) =>
      await putOrganization(organization),
    requestName: 'UpdateOrganization',
    onSuccess: onSuccess,
    onError: onError,
  });

  return { updatePerson, updateOrganization };
};

export default useUpdateContact;
