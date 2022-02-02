import { ILease } from 'interfaces';
import React from 'react';

import { ILeaseTerm } from './../../interfaces/ILeaseTerm';
import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease term endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeaseTerms = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      deleteLeaseTerm: (term: ILeaseTerm) =>
        api.delete<ILease>(`/leases/${term.leaseId}/term`, { data: term }),
      putLeaseTerm: (term: ILeaseTerm) =>
        api.put<ILease>(`/leases/${term.leaseId}/term/${term.id}`, term),
      postLeaseTerm: (term: ILeaseTerm) => api.post<ILease>(`/leases/${term.leaseId}/term`, term),
    }),
    [api],
  );
};
