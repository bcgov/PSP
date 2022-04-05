import { ILease } from 'interfaces';
import { IParentConcurrencyGuard } from 'interfaces/IParentConcurrencyGuard';
import { Api_SecurityDeposit } from 'models/api/SecurityDeposit';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease deposits endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeaseDeposits = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      deleteLeaseDeposit: (request: IParentConcurrencyGuard<Api_SecurityDeposit>) =>
        api.delete<ILease>(`/leases/${request.parentId}/deposits`, { data: request }),
      putLeaseDeposit: (request: IParentConcurrencyGuard<Api_SecurityDeposit>) =>
        api.put<ILease>(`/leases/${request.parentId}/deposits/${request.payload.id}`, request),
      putLeaseDepositNote: (request: IParentConcurrencyGuard<{ note: string }>) =>
        api.put<ILease>(`/leases/${request.parentId}/deposits/note`, request),
      postLeaseDeposit: (request: IParentConcurrencyGuard<Api_SecurityDeposit>) =>
        api.post<ILease>(`/leases/${request.parentId}/deposits`, request),
    }),
    [api],
  );
};
