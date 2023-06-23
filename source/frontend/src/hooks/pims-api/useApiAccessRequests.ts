import queryString from 'query-string';
import React from 'react';

import { IPaginateAccessRequests } from '@/constants/API';
import { IPagedItems } from '@/interfaces';

import { Api_AccessRequest } from './../../models/api/AccessRequest';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the access requests endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiAccessRequests = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getAccessRequest: () => api.get<Api_AccessRequest>(`/access/requests`),
      getAccessRequestById: (accessRequestId: number) =>
        api.get<Api_AccessRequest>(`/admin/access/requests/${accessRequestId}`),
      getAccessRequestsPaged: (params: IPaginateAccessRequests) =>
        api.get<IPagedItems<Api_AccessRequest>>(
          `/admin/access/requests?${queryString.stringify(params)}`,
        ),
      postAccessRequest: (accessRequest: Api_AccessRequest) => {
        return api.request<Api_AccessRequest>({
          url: `/access/requests${accessRequest.id === undefined ? '' : `/${accessRequest.id}`}`,
          method: accessRequest.id === undefined ? 'post' : 'put',
          data: accessRequest,
        });
      },
      putAccessRequest: (accessRequest: Api_AccessRequest) =>
        api.put<Api_AccessRequest>(`/keycloak/access/requests`, accessRequest),
      deleteAccessRequest: (accessRequest: Api_AccessRequest) =>
        api.delete<Api_AccessRequest>(`/admin/access/requests/${accessRequest.id}`, {
          data: accessRequest,
        }),
    }),
    [api],
  );
};
