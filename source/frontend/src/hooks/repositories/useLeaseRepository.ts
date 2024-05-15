import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiLeases } from '../pims-api/useApiLeases';

/**
 * hook that interacts with the Lease API.
 */
export const useLeaseRepository = () => {
  const { getLastUpdatedByApi, getApiLease } = useApiLeases();

  const getLastUpdatedBy = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<Api_LastUpdatedBy, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number) => await getLastUpdatedByApi(leaseId),
      [getLastUpdatedByApi],
    ),
    requestName: 'getLastUpdatedBy',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retreive last-updated-by information for a lease.'),
  });

  const getLease = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_Lease, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number) => await getApiLease(leaseId),
      [getApiLease],
    ),
    requestName: 'getApiLease',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retreive lease.'),
  });

  return useMemo(
    () => ({
      getLastUpdatedBy: getLastUpdatedBy,
      getLease: getLease,
    }),
    [getLastUpdatedBy, getLease],
  );
};
