import { ILeaseFilter } from 'features/leases/interfaces';
import { ILease, ILeaseSearchResult, IPagedItems } from 'interfaces';
import queryString from 'query-string';
import React from 'react';

import { IPaginateRequest, useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeases = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getLeases: (params: IPaginateLeases | null) =>
        api.get<IPagedItems<ILeaseSearchResult>>(
          `/leases/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getLease: (id: number) => api.get<ILease>(`/leases/${id}`),
    }),
    [api],
  );
};

export type IPaginateLeases = IPaginateRequest<ILeaseFilter>;
