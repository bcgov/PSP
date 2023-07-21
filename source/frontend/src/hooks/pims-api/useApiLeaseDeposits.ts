import React from 'react';

import { Api_SecurityDeposit } from '@/models/api/SecurityDeposit';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease deposits endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeaseDeposits = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getLeaseDeposits: (leaseId: number) =>
        api.get<Api_SecurityDeposit[]>(`/leases/${leaseId}/deposits`),
      deleteLeaseDeposit: (leaseId: number, securityDeposit: Api_SecurityDeposit) =>
        api.delete<void>(`/leases/${leaseId}/deposits/${securityDeposit.id}`, {
          data: securityDeposit,
        }),
      putLeaseDeposit: (leaseId: number, securityDeposit: Api_SecurityDeposit) =>
        api.put<Api_SecurityDeposit>(
          `/leases/${leaseId}/deposits/${securityDeposit.id}`,
          securityDeposit,
        ),
      putLeaseDepositNote: (leaseId: number, note: string) =>
        api.put<void>(`/leases/${leaseId}/deposits/note`, { note: note }),
      postLeaseDeposit: (leaseId: number, securityDeposit: Api_SecurityDeposit) =>
        api.post<Api_SecurityDeposit>(`/leases/${leaseId}/deposits`, securityDeposit),
    }),
    [api],
  );
};
