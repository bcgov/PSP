import { AxiosResponse } from 'axios';
import { getPropertyLeases } from 'hooks/pims-api/useApiPropertyLeases';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_PropertyLease } from 'models/api/PropertyLease';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that interacts with the insurances API.
 */
export const usePropertyLeaseRepository = () => {
  const getPropertyLeasesApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<Api_PropertyLease[], any>>
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
