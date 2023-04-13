import { ILeaseFilter } from 'features/leases';
import { ILease, ILeaseSearchResult, IPagedItems } from 'interfaces';
import { Api_Lease } from 'models/api/Lease';
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
      getApiLease: (id: number) => api.get<Api_Lease>(`/leases/concept/${id}`),
      postLease: (lease: Api_Lease, userOverride: boolean = false) =>
        api.post<Api_Lease>(`/leases?userOverride=${userOverride}`, lease),
      putApiLease: (lease: Api_Lease, userOverride?: boolean) =>
        api.put<Api_Lease>(`/leases/${lease.id}?userOverride=${userOverride}`, lease),
      putLease: (lease: ILease, subRoute: string, userOverride?: boolean) =>
        api.put<ILease>(
          `/leases/${lease.id}/${subRoute ?? ''}?userOverride=${userOverride}`,
          lease,
        ),
      exportLeases: (filter: IPaginateLeases, outputFormat: 'csv' | 'excel' = 'excel') =>
        api.get<Blob>(
          `/reports/leases?${filter ? queryString.stringify({ ...filter, all: true }) : ''}`,
          {
            responseType: 'blob',
            headers: {
              Accept: outputFormat === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
            },
          },
        ),
      exportAggregatedLeases: (fiscalYearStart: number) =>
        api.get<Blob>(`/reports/leases/aggregated?fiscalYearStart=${fiscalYearStart}`, {
          responseType: 'blob',
          headers: {
            Accept: 'application/vnd.ms-excel',
          },
        }),
    }),
    [api],
  );
};

export type IPaginateLeases = IPaginateRequest<ILeaseFilter>;
