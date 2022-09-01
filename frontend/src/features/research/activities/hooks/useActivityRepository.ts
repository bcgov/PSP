import { AxiosResponse } from 'axios';
import {
  deleteActivity,
  getActivity,
  postActivity,
  putActivity,
} from 'hooks/pims-api/useApiActivities';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_Activity } from 'models/api/Activity';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that interacts with the Activitys API.
 */
export const useActivityRepository = () => {
  const addActivityApi = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_Activity, any>>
  >({
    requestFunction: useCallback(
      async (activity: Api_Activity) => await postActivity(activity),
      [],
    ),
    requestName: 'AddActivity',
    onSuccess: useAxiosSuccessHandler('Activity saved'),
    onError: useAxiosErrorHandler(),
  });

  const getActivityApi = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_Activity, any>>
  >({
    requestFunction: useCallback(async (activityId: number) => await getActivity(activityId), []),
    requestName: 'GetActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to load activity. Either refresh the page to try again or try and load a different activity.',
    ),
  });

  const updateActivityApi = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_Activity, any>>
  >({
    requestFunction: useCallback(async (activity: Api_Activity) => await putActivity(activity), []),
    requestName: 'UpdateActivity',
    onSuccess: useAxiosSuccessHandler('Activity saved'),
    onError: useAxiosErrorHandler(),
  });

  const deleteActivityApi = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (activityId: number) => await deleteActivity(activityId),
      [],
    ),
    throwError: true,
    requestName: 'DeleteActivity',
    onSuccess: useAxiosSuccessHandler('Activity deleted'),
    onError: useAxiosErrorHandler('Failed to delete activity. Refresh the page to try again.'),
  });

  return useMemo(
    () => ({
      addActivity: addActivityApi,
      getActivity: getActivityApi,
      updateActivity: updateActivityApi,
      deleteActivity: deleteActivityApi,
    }),
    [addActivityApi, getActivityApi, updateActivityApi, deleteActivityApi],
  );
};
