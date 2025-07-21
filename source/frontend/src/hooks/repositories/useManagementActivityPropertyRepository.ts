import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiPropertyActivities } from '../pims-api/useApiPropertyActivities';

/**
 * hook that interacts with the Property Activity API.
 */
export const useManagementActivityPropertyRepository = () => {
  const { getActivitiesApi, getActivityApi, postActivityApi, putActivityApi, deleteActivityApi } =
    useApiPropertyActivities();

  const getActivities = useApiRequestWrapper<
    (propertyId: number) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity[], any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number) => await getActivitiesApi(propertyId),
      [getActivitiesApi],
    ),
    requestName: 'getActivities',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load property management activities list.'),
  });

  const getActivity = useApiRequestWrapper<
    (
      propertyId: number,
      activityId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, activityId: number) =>
        await getActivityApi(propertyId, activityId),
      [getActivityApi],
    ),
    requestName: 'getActivityApi',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retrive property activity.'),
  });

  const createActivity = useApiRequestWrapper<
    (
      propertyId: number,
      activity: ApiGen_Concepts_ManagementActivity,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, activity: ApiGen_Concepts_ManagementActivity) =>
        await postActivityApi(propertyId, activity),
      [postActivityApi],
    ),
    requestName: 'createActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to create a property activity.'),
  });

  const updateActivity = useApiRequestWrapper<
    (
      propertyId: number,
      activity: ApiGen_Concepts_ManagementActivity,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, activity: ApiGen_Concepts_ManagementActivity) =>
        await putActivityApi(propertyId, activity),
      [putActivityApi],
    ),
    requestName: 'updateActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update a property activity.'),
  });

  const deleteActivity = useApiRequestWrapper<
    (propertyId: number, activityId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, activityId: number) =>
        await deleteActivityApi(propertyId, activityId),
      [deleteActivityApi],
    ),
    requestName: 'deleteActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to delete property management activity. Refresh the page to try again.',
    ),
  });

  return useMemo(
    () => ({
      getActivities,
      getActivity,
      createActivity,
      updateActivity,
      deleteActivity,
    }),
    [getActivities, getActivity, createActivity, updateActivity, deleteActivity],
  );
};
