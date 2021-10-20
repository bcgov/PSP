import { AxiosResponse } from 'axios';
import { ILeaseFilter } from 'features/leases/interfaces';
import { ILease, IPagedItems } from 'interfaces';
// import queryString from 'query-string';
import React from 'react';

import { IPaginateRequest, useAxiosApi } from '.';

// TODO: remove mocked API when backend is functional
const mockGetLeases = (params: IPaginateLeases | null) => {
  const mockJson = {
    page: 1,
    pageIndex: 0,
    quantity: 0,
    total: 0,
    items: [] as ILease[],
  } as IPagedItems<ILease>;
  const mockResponse: AxiosResponse<IPagedItems<ILease>> = {
    data: mockJson,
    status: 200,
    statusText: 'OK',
    config: {},
    headers: {},
  };
  return Promise.resolve(mockResponse);
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
      // getLeases: (params: IPaginateLeases | null) =>
      // api.get<IPagedItems<ILease>>(
      //   `/leases/search?${params ? queryString.stringify(params) : ''}`,
      // ),
      getLease: (id: number) => api.get<ILease>(`/leases/${id}`),
    }),
    [api],
  );
};

export type IPaginateLeases = IPaginateRequest<ILeaseFilter>;
