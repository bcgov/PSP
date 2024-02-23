import React from 'react';

import { IResearchFilter } from '@/features/research/interfaces';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { ApiGen_Concepts_PropertyActivitySubtype } from '@/models/api/generated/ApiGen_Concepts_PropertyActivitySubtype';

import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the property activitiesendpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiPropertyActivities = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getActivitySubtypesApi: () =>
        api.get<ApiGen_Concepts_PropertyActivitySubtype[]>(
          `/properties/management-activities/subtypes`,
        ),

      getActivitiesApi: (propertyId: number) =>
        api.get<ApiGen_Concepts_PropertyActivity[]>(
          `/properties/${propertyId}/management-activities`,
        ),

      getActivityApi: (propertyId: number, activityId: number) =>
        api.get<ApiGen_Concepts_PropertyActivity>(
          `/properties/${propertyId}/management-activities/${activityId}`,
        ),

      postActivityApi: (propertyId: number, activity: ApiGen_Concepts_PropertyActivity) =>
        api.post<ApiGen_Concepts_PropertyActivity>(
          `/properties/${propertyId}/management-activities`,
          activity,
        ),

      putActivityApi: (propertyId: number, activity: ApiGen_Concepts_PropertyActivity) =>
        api.put<ApiGen_Concepts_PropertyActivity>(
          `/properties/${propertyId}/management-activities/${activity.id}`,
          activity,
        ),

      deleteActivityApi: (propertyId: number, activityId: number) =>
        api.delete<boolean>(`/properties/${propertyId}/management-activities/${activityId}`),
    }),
    [api],
  );
};

export type IPaginateResearch = IPaginateRequest<IResearchFilter>;
