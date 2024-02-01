import React from 'react';

import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';

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
        api.get<ApiGen_Concepts_SecurityDeposit[]>(`/leases/${leaseId}/deposits`),
      deleteLeaseDeposit: (leaseId: number, securityDeposit: ApiGen_Concepts_SecurityDeposit) =>
        api.delete<void>(`/leases/${leaseId}/deposits/${securityDeposit.id}`, {
          data: securityDeposit,
        }),
      putLeaseDeposit: (leaseId: number, securityDeposit: ApiGen_Concepts_SecurityDeposit) =>
        api.put<ApiGen_Concepts_SecurityDeposit>(
          `/leases/${leaseId}/deposits/${securityDeposit.id}`,
          securityDeposit,
        ),
      putLeaseDepositNote: (leaseId: number, note: string) =>
        api.put<void>(`/leases/${leaseId}/deposits/note`, { note: note }),
      postLeaseDeposit: (leaseId: number, securityDeposit: ApiGen_Concepts_SecurityDeposit) =>
        api.post<ApiGen_Concepts_SecurityDeposit>(`/leases/${leaseId}/deposits`, securityDeposit),
    }),
    [api],
  );
};
