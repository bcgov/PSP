import { ILease, ILeaseSecurityDepositReturn } from 'interfaces';
import { IParentConcurrencyGuard } from 'interfaces/IParentConcurrencyGuard';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease return deposits endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeaseDepositReturns = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      deleteLeaseDepositReturn: (request: IParentConcurrencyGuard<ILeaseSecurityDepositReturn>) =>
        api.delete<ILease>(`/leases/${request.parentId}/deposit-returns`, { data: request }),
      putLeaseDepositReturn: (request: IParentConcurrencyGuard<ILeaseSecurityDepositReturn>) =>
        api.put<ILease>(
          `/leases/${request.parentId}/deposit-returns/${request.payload.id}`,
          request,
        ),
      postLeaseDepositReturn: (request: IParentConcurrencyGuard<ILeaseSecurityDepositReturn>) =>
        api.post<ILease>(`/leases/${request.parentId}/deposit-returns`, request),
    }),
    [api],
  );
};
