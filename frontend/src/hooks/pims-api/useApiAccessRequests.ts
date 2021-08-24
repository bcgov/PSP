import { IPaginateAccessRequests } from 'constants/API';
import { IAccessRequest, IPagedItems } from 'interfaces';
import queryString from 'query-string';
import React from 'react';

import { useApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the access requests endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiAccessRequests = () => {
  const api = useApi();

  return React.useMemo(
    () => ({
      getAccessRequest: () => api.get<IAccessRequest>(`/access/requests`),
      getAccessRequestsPaged: (params: IPaginateAccessRequests) =>
        api.get<IPagedItems<IAccessRequest>>(
          `/admin/access/requests?${queryString.stringify(params)}`,
        ),
      postAccessRequest: (accessRequest: IAccessRequest) => {
        return api.request<IAccessRequest>({
          url: `/access/requests${accessRequest.id === undefined ? '' : `/${accessRequest.id}`}`,
          method: accessRequest.id === undefined ? 'post' : 'put',
          data: accessRequest,
        });
      },
      putAccessRequest: (accessRequest: IAccessRequest) =>
        api.put<IAccessRequest>(`/keycloak/access/requests`, accessRequest),
      deleteAccessRequest: (accessRequest: IAccessRequest) =>
        api.delete<IAccessRequest>(`/admin/access/requests/${accessRequest.id}`, {
          data: accessRequest,
        }),
    }),
    [api],
  );
};
