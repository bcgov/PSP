import { useApiProperties } from 'hooks/pims-api';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useCallback } from 'react';
import { useAxiosErrorHandler } from 'utils';

/**
 * hook that retrieves a property from the inventory.
 */
export const useGetProperty = () => {
  const { getPropertyConceptWithId } = useApiProperties();

  const getApiPropertyWrapper = useApiRequestWrapper({
    requestFunction: useCallback(
      async (id: number) => await getPropertyConceptWithId(id),
      [getPropertyConceptWithId],
    ),
    requestName: 'getPropertyApiById',
    onError: useAxiosErrorHandler('Failed to retrieve property information from PIMS'),
  });

  return getApiPropertyWrapper;
};
