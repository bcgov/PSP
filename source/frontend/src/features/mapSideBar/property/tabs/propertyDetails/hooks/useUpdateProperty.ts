import { AxiosError } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiProperties } from '@/hooks/pims-api';
import { useApiRequestWrapper } from '@/hooks/pims-api/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { Api_Property } from '@/models/api/Property';

/**
 * hook that updates a property from the inventory.
 */
export const useUpdateProperty = () => {
  const { putPropertyConceptApi } = useApiProperties();

  const { execute, loading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (property: Api_Property) => await putPropertyConceptApi(property),
      [putPropertyConceptApi],
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
  return { updateProperty: execute, updatePropertyLoading: loading };
};
