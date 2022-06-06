import { AxiosError } from 'axios';
import { useApiProperties } from 'hooks/pims-api';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that retrieves a property from the inventory.
 */
export const useGetProperty = () => {
  const { getPropertyConceptWithPid } = useApiProperties();

  const { execute, loading } = useApiRequestWrapper({
    requestFunction: useCallback(async (pid: string) => await getPropertyConceptWithPid(pid), [
      getPropertyConceptWithPid,
    ]),
    requestName: 'retrievePropertyConcept',
    onSuccess: useCallback(() => toast.success('Property information retrieved'), []),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Retrieve property information error. Check responses and try again.');
      }
    }, []),
  });
  return { retrieveProperty: execute, retrievePropertyLoading: loading };
};
