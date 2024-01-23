import queryString from 'query-string';
import React from 'react';

import { IPaginateAccessRequests } from '@/constants/API';
import { IPagedItems } from '@/interfaces';
import { ApiGen_Concepts_AccessRequest } from '@/models/api/generated/ApiGen_Concepts_AccessRequest';
import { isValidId } from '@/utils';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the access requests endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiAccessRequests = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getAccessRequest: () => api.get<ApiGen_Concepts_AccessRequest>(`/access/requests`),
      getAccessRequestById: (accessRequestId: number) =>
        api.get<ApiGen_Concepts_AccessRequest>(`/admin/access/requests/${accessRequestId}`),
      getAccessRequestsPaged: (params: IPaginateAccessRequests) =>
        api.get<IPagedItems<ApiGen_Concepts_AccessRequest>>(
          `/admin/access/requests?${queryString.stringify(params)}`,
        ),
      postAccessRequest: (accessRequest: ApiGen_Concepts_AccessRequest) => {
        return api.request<ApiGen_Concepts_AccessRequest>({
          url: `/access/requests${!isValidId(accessRequest.id) ? '' : `/${accessRequest.id}`}`,
          method: !isValidId(accessRequest.id) ? 'post' : 'put',
          data: accessRequest,
        });
      },
      putAccessRequest: (accessRequest: ApiGen_Concepts_AccessRequest) =>
        api.put<ApiGen_Concepts_AccessRequest>(`/keycloak/access/requests`, accessRequest),
      deleteAccessRequest: (accessRequest: ApiGen_Concepts_AccessRequest) =>
        api.delete<ApiGen_Concepts_AccessRequest>(`/admin/access/requests/${accessRequest.id}`, {
          data: accessRequest,
        }),
    }),
    [api],
  );
};
