import { AxiosResponse } from 'axios';
import { FileTypes } from 'constants/fileTypes';
import {
  deleteActivity,
  getActivity,
  getActivityTemplates,
  getFileActivities,
  postActivity,
  postFileActivity,
  putActivity,
} from 'hooks/pims-api/useApiActivities';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_Activity, Api_ActivityTemplate, Api_FileActivity } from 'models/api/Activity';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that interacts with the Activitys API.
 */
export const useActivityRepository = () => {
  const addActivityApi = useApiRequestWrapper<
    (activity: Api_Activity) => Promise<AxiosResponse<Api_Activity, any>>
  >({
    requestFunction: useCallback(
      async (activity: Api_Activity) => await postActivity(activity),
      [],
    ),
    requestName: 'AddActivity',
    onSuccess: useAxiosSuccessHandler('Activity saved'),
    onError: useAxiosErrorHandler(),
  });

  const addFileActivityApi = useApiRequestWrapper<
    (fileType: FileTypes, activity: Api_FileActivity) => Promise<AxiosResponse<Api_Activity, any>>
  >({
    requestFunction: useCallback(
      async (fileType: FileTypes, activity: Api_FileActivity) =>
        await postFileActivity(fileType, activity),
      [],
    ),
    requestName: 'AddFileActivity',
    onSuccess: useAxiosSuccessHandler('Activity saved'),
    onError: useAxiosErrorHandler(),
  });

  const getActivityApi = useApiRequestWrapper<
    (activityId: number) => Promise<AxiosResponse<Api_Activity, any>>
  >({
    requestFunction: useCallback(async (activityId: number) => await getActivity(activityId), []),
    requestName: 'GetActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to load activity. Either refresh the page to try again or try and load a different activity.',
    ),
  });

  const getFileActivitiesApi = useApiRequestWrapper<
    (fileType: FileTypes, fileId: number) => Promise<AxiosResponse<Api_Activity[], any>>
  >({
    requestFunction: useCallback(
      async (fileType: FileTypes, fileId: number) => await getFileActivities(fileType, fileId),
      [],
    ),
    requestName: 'GetFileActivities',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load activities. Refresh the page to try again.'),
  });

  const getActivityTemplatesApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_ActivityTemplate[], any>>
  >({
    requestFunction: useCallback(async () => await getActivityTemplates(), []),
    requestName: 'GetActivityTemplates',
    onSuccess: useAxiosSuccessHandler(),
    invoke: true,
    onError: useAxiosErrorHandler(
      'Failed to load activity. Either refresh the page to try again or try and load a different activity.',
    ),
  });

  const updateActivityApi = useApiRequestWrapper<
    (activity: Api_Activity) => Promise<AxiosResponse<Api_Activity, any>>
  >({
    requestFunction: useCallback(async (activity: Api_Activity) => await putActivity(activity), []),
    requestName: 'UpdateActivity',
    onSuccess: useAxiosSuccessHandler('Activity saved'),
    onError: useAxiosErrorHandler(),
  });

  const deleteActivityApi = useApiRequestWrapper<
    (activityId: number) => Promise<AxiosResponse<boolean, any>>
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
      addFileActivity: addFileActivityApi,
      getActivityTemplates: getActivityTemplatesApi,
      getActivity: getActivityApi,
      getFileActivities: getFileActivitiesApi,
      updateActivity: updateActivityApi,
      deleteActivity: deleteActivityApi,
    }),
    [
      addActivityApi,
      addFileActivityApi,
      getActivityApi,
      getFileActivitiesApi,
      updateActivityApi,
      deleteActivityApi,
      getActivityTemplatesApi,
    ],
  );
};
