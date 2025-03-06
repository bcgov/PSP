import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiLeases } from '../pims-api/useApiLeases';
import { getPropertyLeases } from '../pims-api/useApiPropertyLeases';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the insurances API.
 */
export const usePropertyLeaseRepository = () => {
  const { putLeaseProperties } = useApiLeases();

  const getPropertyLeasesApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyLease[], any>>
  >({
    requestFunction: useCallback(async (leaseId: number) => await getPropertyLeases(leaseId), []),
    requestName: 'getPropertyLeases',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateLeasePropertiesApi = useApiRequestWrapper<
    (
      lease: ApiGen_Concepts_Lease,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_Lease, any>>
  >({
    requestFunction: useCallback(
      async (lease: ApiGen_Concepts_Lease, userOverrideCodes: UserOverrideCode[]) =>
        await putLeaseProperties(lease, userOverrideCodes),
      [putLeaseProperties],
    ),
    requestName: 'UpdateLeaseProperties',
    onSuccess: useAxiosSuccessHandler('Lease File Properties updated'),
    throwError: true,
  });

  return useMemo(
    () => ({
      getPropertyLeases: getPropertyLeasesApi,
      updateLeaseProperties: updateLeasePropertiesApi,
    }),
    [getPropertyLeasesApi, updateLeasePropertiesApi],
  );
};
