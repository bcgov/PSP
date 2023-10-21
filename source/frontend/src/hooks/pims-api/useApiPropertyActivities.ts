import React from 'react';

import { IResearchFilter } from '@/features/research/interfaces';
import { Api_PropertyActivity, Api_PropertyActivitySubtype } from '@/models/api/PropertyActivity';

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
        api.get<Api_PropertyActivitySubtype[]>(`/properties/management-activities/subtypes`),
      getActivityApi: (propertyId: number, activityId: number) =>
        api.get<Api_PropertyActivity>(
          `/properties/${propertyId}/management-activities/${activityId}`,
        ),
      postActivityApi: (propertyId: number, activity: Api_PropertyActivity) =>
        api.post<Api_PropertyActivity>(`/properties/${propertyId}/management-activities`, activity),
      putActivityApi: (propertyId: number, activity: Api_PropertyActivity) =>
        api.put<Api_PropertyActivity>(
          `/properties/${propertyId}/management-activities/${activity.id}`,
          activity,
        ),
    }),
    [api],
  );
};

export type IPaginateResearch = IPaginateRequest<IResearchFilter>;
