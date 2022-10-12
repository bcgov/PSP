import { useApiProperties } from 'hooks/pims-api';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useCallback } from 'react';
import { toast } from 'react-toastify';
import { useAxiosErrorHandler } from 'utils';

/**
 * hook that retrieves a property from the inventory.
 */
export const useGetProperty = () => {
  const { getPropertyConceptWithId } = useApiProperties();

  const getApiPropertyWrapper = useApiRequestWrapper({
    requestFunction: useCallback(async (id: number) => await getPropertyConceptWithId(id), [
      getPropertyConceptWithId,
    ]),
    requestName: 'getPropertyApiById',
    onSuccess: useCallback(() => toast.success('Property information retrieved'), []),
    onError: useAxiosErrorHandler('Failed to retrieve property information from PIMS'),
  });

  return getApiPropertyWrapper;
};
