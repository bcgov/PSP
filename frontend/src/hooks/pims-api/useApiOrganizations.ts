import { IPaginateParams } from 'constants/API';
import * as pimsToasts from 'constants/toasts';
import { LifecycleToasts } from 'customAxios';
import { IOrganization, IPagedItems } from 'interfaces';
import React from 'react';

import { useApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the organization endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

const organizationToasts: LifecycleToasts = {
  loadingToast: pimsToasts.organization.ORGANIZATION_UPDATING,
  successToast: pimsToasts.organization.ORGANIZATION_UPDATED,
  errorToast: pimsToasts.organization.ORGANIZATION_ERROR,
};

export const useApiOrganizations = () => {
  const api = useApi();
  const apiWithToasts = useApi({ lifecycleToasts: organizationToasts });

  return React.useMemo(
    () => ({
      getOrganization: (id: number) => api.get<IOrganization>(`/admin/organizations/${id}`),
      getOrganizationsPaged: (params: IPaginateParams) =>
        api.post<IPagedItems<IOrganization>>(`/admin/organizations/filter`, params),
      postOrganization: (organization: IOrganization) =>
        apiWithToasts.post<IOrganization>(`/admin/organizations`, organization),
      putOrganization: (organization: IOrganization) =>
        apiWithToasts.put<IOrganization>(`/admin/organizations/${organization.id}`, organization),
      deleteOrganization: (organization: IOrganization) =>
        api.delete<IOrganization>(`/admin/organizations/${organization.id}`, {
          data: organization,
        }),
    }),
    [api, apiWithToasts],
  );
};
