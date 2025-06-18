import queryString from 'query-string';
import React from 'react';

import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';

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
      getPropertyActivityApi: (propertyActivityId: number) =>
        api.get<ApiGen_Concepts_PropertyActivity>(`/management-activities/${propertyActivityId}`),

      postActivityApi: (managementFileId: number, activity: ApiGen_Concepts_PropertyActivity) =>
        api.post<ApiGen_Concepts_PropertyActivity>(
          `/managementfiles/${managementFileId}/management-activities`,
          activity,
        ),

      putActivityApi: (managementFileId: number, activity: ApiGen_Concepts_PropertyActivity) =>
        api.put<ApiGen_Concepts_PropertyActivity>(
          `/managementfiles/${managementFileId}/management-activities/${activity.id}`,
          activity,
        ),

      getManagementActivitiesPagedApi: (params: IPaginateManagementActivities | null) =>
        api.get<ApiGen_Base_Page<ApiGen_Concepts_PropertyActivity>>(
          `/management-activities/search?${params ? queryString.stringify(params) : ''}`,
        ),

      exportManagementActivitiesApi: (
        filter: IPaginateManagementActivities,
        outputFormat: 'csv' | 'excel' = 'excel',
      ) =>
        api.get<Blob>(
          `/reports/management-activities?${
            filter ? queryString.stringify({ ...filter, all: true }) : ''
          }`,
          {
            responseType: 'blob',
            headers: {
              Accept: outputFormat === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
            },
          },
        ),
      getActivityApi: (managementFileId: number, propertyActivityId: number) =>
        api.get<ApiGen_Concepts_PropertyActivity>(
          `/managementfiles/${managementFileId}/management-activities/${propertyActivityId}`,
        ),

      getActivitiesApi: (managementFileId: number) =>
        api.get<ApiGen_Concepts_PropertyActivity[]>(
          `/managementfiles/${managementFileId}/management-activities/`,
        ),
      getFileActivitiesApi: (managementFileId: number) =>
        api.get<ApiGen_Concepts_PropertyActivity[]>(
          `/managementfiles/${managementFileId}/properties/management-activities/`,
        ),
      deleteActivityApi: (managementFileId: number, propertyActivityId: number) =>
        api.delete<boolean>(
          `/managementfiles/${managementFileId}/management-activities/${propertyActivityId}`,
        ),
    }),
    [api],
  );
};
