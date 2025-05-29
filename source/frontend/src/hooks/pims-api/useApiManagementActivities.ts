import React from 'react';

import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { ApiGen_Concepts_PropertyActivitySubtype } from '@/models/api/generated/ApiGen_Concepts_PropertyActivitySubtype';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the management activities endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiManagementActivities = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getActivitySubtypesApi: () =>
        api.get<ApiGen_Concepts_PropertyActivitySubtype[]>(
          `/properties/management-activities/subtypes`,
        ),

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
