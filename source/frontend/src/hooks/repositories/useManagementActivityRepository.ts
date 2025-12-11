import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiManagementActivities } from '../pims-api/useApiManagementActivities';

/**
 * hook that interacts with the Management File Activity API.
 */
export const useManagementActivityRepository = () => {
  const {
    postActivityApi,
    getPropertyActivityApi,
    getActivityApi,
    getActivitiesApi,
    getFileActivitiesApi,
    deleteActivityApi,
    putActivityApi,
  } = useApiManagementActivities();

  const addManagementActivity = useApiRequestWrapper<
    (
      managementFileId: number,
      activity: ApiGen_Concepts_ManagementActivity,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, activity: ApiGen_Concepts_ManagementActivity) =>
        await postActivityApi(managementFileId, activity),
      [postActivityApi],
    ),
    requestName: 'AddManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to create a management file activity.'),
  });

  const getPropertyManagementActivity = useApiRequestWrapper<
    (
      managementActivityId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity, any>>
  >({
    requestFunction: useCallback(
      async (managementActivityId: number) => await getPropertyActivityApi(managementActivityId),
      [getPropertyActivityApi],
    ),
    requestName: 'GetManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retrieve a management file activity.'),
  });

  const updateManagementActivity = useApiRequestWrapper<
    (
      managementFileId: number,
      activity: ApiGen_Concepts_ManagementActivity,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, activity: ApiGen_Concepts_ManagementActivity) =>
        await putActivityApi(managementFileId, activity),
      [putActivityApi],
    ),
    requestName: 'UpdateManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update a management file activity.'),
  });

  const getManagementActivity = useApiRequestWrapper<
    (
      managementFileId: number,
      managementActivityId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, managementActivityId: number) =>
        await getActivityApi(managementFileId, managementActivityId),
      [getActivityApi],
    ),
    requestName: 'GetManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retrieve a management file activity.'),
  });

  const getManagementActivities = useApiRequestWrapper<
    (managementFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity[], any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number) => await getActivitiesApi(managementFileId),
      [getActivitiesApi],
    ),
    requestName: 'getManagementActivities',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load property management activities list.'),
  });

  const getManagementFileActivities = useApiRequestWrapper<
    (managementFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_ManagementActivity[], any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number) => await getFileActivitiesApi(managementFileId),
      [getFileActivitiesApi],
    ),
    requestName: 'getManagementFileActivities',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load property management file activities list.'),
  });

  const deleteManagementActivity = useApiRequestWrapper<
    (managementFileId: number, managementActivityId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, managementActivityId: number) =>
        await deleteActivityApi(managementFileId, managementActivityId),
      [deleteActivityApi],
    ),
    requestName: 'deleteManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to delete management file activity. Refresh the page to try again.',
    ),
  });

  return useMemo(
    () => ({
      getPropertyManagementActivity,
      addManagementActivity,
      updateManagementActivity,
      getManagementActivity,
      getManagementActivities,
      getManagementFileActivities,
      deleteManagementActivity,
    }),
    [
      getPropertyManagementActivity,
      addManagementActivity,
      updateManagementActivity,
      getManagementActivity,
      getManagementActivities,
      getManagementFileActivities,
      deleteManagementActivity,
    ],
  );
};
