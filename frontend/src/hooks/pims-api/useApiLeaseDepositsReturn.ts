import { ILease } from 'interfaces';
import { IParentConcurrencyGuard } from 'interfaces/IParentConcurrencyGuard';
import { Api_SecurityDepositReturn } from 'models/api/SecurityDeposit';
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
      deleteLeaseDepositReturn: (request: IParentConcurrencyGuard<Api_SecurityDepositReturn>) =>
        api.delete<ILease>(
          `/leases/${request.parentId}/deposits/${request.payload.parentDepositId}/returns`,
          { data: request },
        ),
      putLeaseDepositReturn: (request: IParentConcurrencyGuard<Api_SecurityDepositReturn>) =>
        api.put<ILease>(
          `/leases/${request.parentId}/deposits/${request.payload.parentDepositId}/returns/${request.payload.id}`,
          request,
        ),
      postLeaseDepositReturn: (request: IParentConcurrencyGuard<Api_SecurityDepositReturn>) =>
        api.post<ILease>(
          `/leases/${request.parentId}/deposits/${request.payload.parentDepositId}/returns`,
          request,
        ),
    }),
    [api],
  );
};
