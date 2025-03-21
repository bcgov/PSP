import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { useAxiosSuccessHandler } from '@/utils/axiosUtils';

/**
 * hook that updates a contact.
 */
export const useUpdateContact = () => {
  const { putPerson, putOrganization } = useApiContacts();

  const onError = useCallback((axiosError: AxiosError<IApiError>) => {
    if (axiosError?.response?.status === 400) {
      toast.error(axiosError?.response.data.error);
      return Promise.resolve();
    } else {
      toast.error('Unable to save. Please try again.');
      return Promise.reject(axiosError);
    }
  }, []);

  const { execute: updatePerson } = useApiRequestWrapper<
    (person: ApiGen_Concepts_Person) => Promise<AxiosResponse<ApiGen_Concepts_Person, any>>
  >({
    requestFunction: useCallback(
      async (person: ApiGen_Concepts_Person) => await putPerson(person),
      [putPerson],
    ),
    requestName: 'UpdatePerson',
    onSuccess: useAxiosSuccessHandler(),
    onError: onError,
  });

  const { execute: updateOrganization } = useApiRequestWrapper<
    (
      person: ApiGen_Concepts_Organization,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Organization, any>>
  >({
    requestFunction: useCallback(
      async (organization: ApiGen_Concepts_Organization) => await putOrganization(organization),
      [putOrganization],
    ),
    requestName: 'UpdateOrganization',
    onSuccess: useAxiosSuccessHandler(),
    onError: onError,
  });

  return { updatePerson, updateOrganization };
};

export default useUpdateContact;
