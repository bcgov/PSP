import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { Api_LeaseTenant } from '@/models/api/LeaseTenant';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils/axiosUtils';

import { getLeaseTenants, updateLeaseTenants } from '../pims-api/useApiLeaseTenants';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the property improvements API.
 */
export const useLeaseTenantRepository = () => {
  const getLeaseTenantsApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<Api_LeaseTenant[], any>>
  >({
    requestFunction: useCallback(async (leaseId: number) => await getLeaseTenants(leaseId), []),
    requestName: 'getLeaseTenants',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateLeaseTenantsApi = useApiRequestWrapper<
    (
      leaseId: number,
      improvements: Api_LeaseTenant[],
    ) => Promise<AxiosResponse<Api_LeaseTenant[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, improvements: Api_LeaseTenant[]) =>
        await updateLeaseTenants(leaseId, improvements),
      [],
    ),
    requestName: 'updateLeaseTenants',
    onSuccess: useAxiosSuccessHandler('Tenants saved successfully.'),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      getLeaseTenants: getLeaseTenantsApi,
      updateLeaseTenants: updateLeaseTenantsApi,
    }),
    [getLeaseTenantsApi, updateLeaseTenantsApi],
  );
};
