import { IPaginateParams } from 'constants/API';
import * as pimsToasts from 'constants/toasts';
import { LifecycleToasts } from 'customAxios';
import { IAgency, IAgencyDetail, IPagedItems } from 'interfaces';
import React from 'react';

import { useApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the agency endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

const agencyToasts: LifecycleToasts = {
  loadingToast: pimsToasts.agency.AGENCY_UPDATING,
  successToast: pimsToasts.agency.AGENCY_UPDATED,
  errorToast: pimsToasts.agency.AGENCY_ERROR,
};

export const useApiAgencies = () => {
  const api = useApi();
  const apiWithToasts = useApi({ lifecycleToasts: agencyToasts });

  return React.useMemo(
    () => ({
      getAgency: (id: number) => api.get<IAgency>(`/admin/agencies/${id}`),
      getAgenciesPaged: (params: IPaginateParams) =>
        api.post<IPagedItems<IAgency>>(`/admin/agencies/filter`, params),
      postAgency: (agency: IAgencyDetail) => apiWithToasts.post<IAgency>(`/admin/agencies`, agency),
      putAgency: (agency: IAgencyDetail) =>
        apiWithToasts.put<IAgency>(`/admin/agencies/${agency.id}`, agency),
      deleteAgency: (agency: IAgency) =>
        api.delete<IAgency>(`/admin/agencies/${agency.id}`, {
          data: agency,
        }),
    }),
    [api, apiWithToasts],
  );
};
