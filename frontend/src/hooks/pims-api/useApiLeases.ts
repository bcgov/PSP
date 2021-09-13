import { ILease, IPagedItems } from 'interfaces';
import queryString from 'query-string';
import React from 'react';

import { ILeaseAndLicenseFilter } from './../../features/leases/interfaces';
import { useApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeases = () => {
  const api = useApi();

  return React.useMemo(
    () => ({
      getLeases: (params: ILeaseAndLicenseFilter | null) =>
        api.get<IPagedItems<ILease>>(
          `/leases/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getLease: (id: number) => api.get<ILease>(`/leases/${id}`),
    }),
    [api],
  );
};
