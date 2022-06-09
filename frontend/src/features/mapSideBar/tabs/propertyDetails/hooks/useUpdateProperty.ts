import { AxiosError } from 'axios';
import { useApiProperties } from 'hooks/pims-api';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import { Api_Property } from 'models/api/Property';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that updates a property from the inventory.
 */
export const useUpdateProperty = () => {
  const { putPropertyConcept } = useApiProperties();

  const { execute } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (property: Api_Property) => await putPropertyConcept(property),
      [putPropertyConcept],
    ),
    requestName: 'updatePropertyConcept',
    onSuccess: useCallback(() => toast.success('Property information updated'), []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Save error. Check responses and try again.');
      }
    }, []),
  });
  return { updateProperty: execute };
};
