import React from 'react';

import { Api_SecurityDepositReturn } from '@/models/api/SecurityDeposit';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease return deposits endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeaseDepositReturns = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      deleteLeaseDepositReturn: (leaseId: number, request: Api_SecurityDepositReturn) =>
        api.delete<void>(`/leases/${leaseId}/deposits/${request.parentDepositId}/returns`, {
          data: request,
        }),
      putLeaseDepositReturn: (leaseId: number, request: Api_SecurityDepositReturn) =>
        api.put<Api_SecurityDepositReturn>(
          `/leases/${leaseId}/deposits/${request.parentDepositId}/returns/${request.id}`,
          request,
        ),
      postLeaseDepositReturn: (leaseId: number, request: Api_SecurityDepositReturn) =>
        api.post<Api_SecurityDepositReturn>(
          `/leases/${leaseId}/deposits/${request.parentDepositId}/returns`,
          request,
        ),
    }),
    [api],
  );
};
