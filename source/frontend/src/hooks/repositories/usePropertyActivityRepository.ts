import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_PropertyActivity, Api_PropertyActivitySubtype } from '@/models/api/PropertyActivity';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiPropertyActivities } from '../pims-api/useApiPropertyActivities';

/**
 * hook that interacts with the Property Activity API.
 */
export const usePropertyActivityRepository = () => {
  const { getActivitySubtypesApi, getActivityApi, postActivityApi, putActivityApi } =
    useApiPropertyActivities();

  const getActivitySubtypes = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_PropertyActivitySubtype[], any>>
  >({
    requestFunction: useCallback(
      async () => await getActivitySubtypesApi(),
      [getActivitySubtypesApi],
    ),
    requestName: 'getActivitySubtypes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retreive property activity subtypes.'),
  });

  const getActivity = useApiRequestWrapper<
    (propertyId: number, activityId: number) => Promise<AxiosResponse<Api_PropertyActivity, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, activityId: number) =>
        await getActivityApi(propertyId, activityId),
      [getActivityApi],
    ),
    requestName: 'getActivityApi',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retreive property activity.'),
  });

  const createActivity = useApiRequestWrapper<
    (
      propertyId: number,
      activity: Api_PropertyActivity,
    ) => Promise<AxiosResponse<Api_PropertyActivity, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, activity: Api_PropertyActivity) =>
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
      activity: Api_PropertyActivity,
    ) => Promise<AxiosResponse<Api_PropertyActivity, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, activity: Api_PropertyActivity) =>
        await putActivityApi(propertyId, activity),
      [putActivityApi],
    ),
    requestName: 'updateActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update a property activity.'),
  });

  return useMemo(
    () => ({
      getActivitySubtypes,
      getActivity,
      createActivity,
      updateActivity,
    }),
    [getActivitySubtypes, getActivity, createActivity, updateActivity],
  );
};
