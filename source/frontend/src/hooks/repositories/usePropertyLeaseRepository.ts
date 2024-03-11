import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { getPropertyLeases } from '../pims-api/useApiPropertyLeases';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the insurances API.
 */
export const usePropertyLeaseRepository = () => {
  const getPropertyLeasesApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyLease[], any>>
  >({
    requestFunction: useCallback(async (leaseId: number) => await getPropertyLeases(leaseId), []),
    requestName: 'getPropertyLeases',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      getPropertyLeases: getPropertyLeasesApi,
    }),
    [getPropertyLeasesApi],
  );
};
