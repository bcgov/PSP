import { AxiosError } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { useApiRequestWrapper } from '@/hooks/pims-api/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { Api_Property } from '@/models/api/Property';
import { useAxiosErrorHandler } from '@/utils';

/**
 * hook that retrieves a property from the inventory.
 */
export const usePimsPropertyRepository = () => {
  const { getPropertyConceptWithIdApi, putPropertyConceptApi } = useApiProperties();

  const getPropertyWrapper = useApiRequestWrapper({
    requestFunction: useCallback(
      async (id: number) => await getPropertyConceptWithIdApi(id),
      [getPropertyConceptWithIdApi],
    ),
    requestName: 'getPropertyApiById',
    onError: useAxiosErrorHandler('Failed to retrieve property information from PIMS'),
  });

  const updatePropertyWrapper = useApiRequestWrapper({
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

  return { getPropertyWrapper, updatePropertyWrapper };
};
