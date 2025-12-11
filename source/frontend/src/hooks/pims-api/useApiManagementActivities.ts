import queryString from 'query-string';
import React from 'react';

import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { Api_ManagementActivityFilter } from '@/models/api/ManagementActivityFilter';

import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

export type IPaginateManagementActivities = IPaginateRequest<any>;

/**
 * PIMS API wrapper to centralize all AJAX requests to the management activities endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiManagementActivities = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getPropertyActivityApi: (managementActivityId: number) =>
        api.get<ApiGen_Concepts_ManagementActivity>(
          `/management-activities/${managementActivityId}`,
        ),

      postActivityApi: (managementFileId: number, activity: ApiGen_Concepts_ManagementActivity) =>
        api.post<ApiGen_Concepts_ManagementActivity>(
          `/managementfiles/${managementFileId}/management-activities`,
          activity,
        ),

      putActivityApi: (managementFileId: number, activity: ApiGen_Concepts_ManagementActivity) =>
        api.put<ApiGen_Concepts_ManagementActivity>(
          `/managementfiles/${managementFileId}/management-activities/${activity.id}`,
          activity,
        ),

      getManagementActivitiesPagedApi: (params: IPaginateManagementActivities | null) =>
        api.get<ApiGen_Base_Page<ApiGen_Concepts_ManagementActivity>>(
          `/management-activities/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getActivityApi: (managementFileId: number, managementActivityId: number) =>
        api.get<ApiGen_Concepts_ManagementActivity>(
          `/managementfiles/${managementFileId}/management-activities/${managementActivityId}`,
        ),

      getActivitiesApi: (managementFileId: number) =>
        api.get<ApiGen_Concepts_ManagementActivity[]>(
          `/managementfiles/${managementFileId}/management-activities/`,
        ),
      getFileActivitiesApi: (managementFileId: number) =>
        api.get<ApiGen_Concepts_ManagementActivity[]>(
          `/managementfiles/${managementFileId}/properties/management-activities/`,
        ),
      deleteActivityApi: (managementFileId: number, managementActivityId: number) =>
        api.delete<boolean>(
          `/managementfiles/${managementFileId}/management-activities/${managementActivityId}`,
        ),

      generateManagementActivitiesOverviewReportApi: (filter: Api_ManagementActivityFilter) =>
        api.post<Blob>(`/reports/management-activities/overview`, filter, {
          responseType: 'blob',
          headers: {
            Accept: 'application/vnd.ms-excel',
          },
        }),

      generateManagementActivitiesInvoiceReportApi: (filter: Api_ManagementActivityFilter) =>
        api.post<Blob>(`/reports/management-activities/invoices`, filter, {
          responseType: 'blob',
          headers: {
            Accept: 'application/vnd.ms-excel',
          },
        }),
    }),
    [api],
  );
};
