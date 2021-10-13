import { AxiosResponse } from 'axios';
import { ILease, IPagedItems } from 'interfaces';
// import queryString from 'query-string';
import React from 'react';

import { ILeaseAndLicenseFilter } from './../../features/leases/interfaces';
import { useAxiosApi } from '.';

// TODO: remove mocked API when backend is functional
const mockGetLeases = (params: ILeaseAndLicenseFilter | null) => {
  const testJson = {
    page: 1,
    pageIndex: 0,
    quantity: 0,
    total: 0,
    items: [] as ILease[],
  } as IPagedItems<ILease>;
  const axiosResponse: AxiosResponse<IPagedItems<ILease>> = {
    data: testJson,
    status: 200,
    statusText: 'OK',
    config: {},
    headers: {},
  };
  return Promise.resolve(axiosResponse);
};

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeases = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getLeases: mockGetLeases,
      // getLeases: (params: ILeaseAndLicenseFilter | null) =>
      // api.get<IPagedItems<ILease>>(
      //   `/leases/search?${params ? queryString.stringify(params) : ''}`,
      // ),
      getLease: (id: number) => api.get<ILease>(`/leases/${id}`),
    }),
    [api],
  );
};
