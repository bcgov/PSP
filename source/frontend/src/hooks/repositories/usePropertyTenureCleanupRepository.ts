import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_PropertyTenureCleanup } from '@/models/api/generated/ApiGen_Concepts_PropertyTenureCleanup';
import { useAxiosErrorHandler } from '@/utils';

import { useApiPropertyTenureCleanup } from '../pims-api/useApiPropertyTenureCleanup';

/**
 * hook that interacts with the Historical Number API.
 */
export const usePropertyTenureCleanupRepository = () => {
  const { getByPropertyId: getByPropertyIdApi } = useApiPropertyTenureCleanup();

  const getPropertyTenureCleanups = useApiRequestWrapper<
    (propertyId: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyTenureCleanup[], any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number) => await getByPropertyIdApi(propertyId),
      [getByPropertyIdApi],
    ),
    requestName: 'getPropertyTenureCleanups',
    onError: useAxiosErrorHandler('Failed to load property tenure cleanups'),
  });

  return useMemo(
    () => ({
      getPropertyTenureCleanups,
    }),
    [getPropertyTenureCleanups],
  );
};
