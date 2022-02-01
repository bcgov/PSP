import { ILease, ILeaseSecurityDeposit } from 'interfaces';
import { IParentConcurrencyGuard } from 'interfaces/IParentConcurrencyGuard';
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
      deleteLeaseDeposit: (request: IParentConcurrencyGuard<ILeaseSecurityDeposit>) =>
        api.delete<ILease>(`/leases/${request.parentId}/deposits`, { data: request }),
      putLeaseDeposit: (request: IParentConcurrencyGuard<ILeaseSecurityDeposit>) =>
        api.put<ILease>(`/leases/${request.parentId}/deposits/${request.payload.id}`, request),
      postLeaseDeposit: (request: IParentConcurrencyGuard<ILeaseSecurityDeposit>) =>
        api.post<ILease>(`/leases/${request.parentId}/deposits`, request),
    }),
    [api],
  );
};
