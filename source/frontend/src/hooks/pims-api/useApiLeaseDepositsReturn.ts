import React from 'react';

import { ApiGen_Concepts_SecurityDepositReturn } from '@/models/api/generated/ApiGen_Concepts_SecurityDepositReturn';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease return deposits endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeaseDepositReturns = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      deleteLeaseDepositReturn: (leaseId: number, request: ApiGen_Concepts_SecurityDepositReturn) =>
        api.delete<void>(`/leases/${leaseId}/deposits/${request.parentDepositId}/returns`, {
          data: request,
        }),
      putLeaseDepositReturn: (leaseId: number, request: ApiGen_Concepts_SecurityDepositReturn) =>
        api.put<ApiGen_Concepts_SecurityDepositReturn>(
          `/leases/${leaseId}/deposits/${request.parentDepositId}/returns/${request.id}`,
          request,
        ),
      postLeaseDepositReturn: (leaseId: number, request: ApiGen_Concepts_SecurityDepositReturn) =>
        api.post<ApiGen_Concepts_SecurityDepositReturn>(
          `/leases/${leaseId}/deposits/${request.parentDepositId}/returns`,
          request,
        ),
    }),
    [api],
  );
};
