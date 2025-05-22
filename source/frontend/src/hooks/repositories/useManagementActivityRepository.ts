import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { ApiGen_Concepts_PropertyActivitySubtype } from '@/models/api/generated/ApiGen_Concepts_PropertyActivitySubtype';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiManagementActivities } from '../pims-api/useApiManagementActivities';

/**
 * hook that interacts with the Management File Activity API.
 */
export const useManagementActivityRepository = () => {
  const {
    getActivitySubtypesApi,
    postActivityApi,
    getActivityApi,
    getActivitiesApi,
    getFileActivitiesApi,
    deleteActivityApi,
  } = useApiManagementActivities();

  const getActivitySubtypes = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_PropertyActivitySubtype[], any>>
  >({
    requestFunction: useCallback(
      async () => await getActivitySubtypesApi(),
      [getActivitySubtypesApi],
    ),
    requestName: 'RetrieveActivitySubtypes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retrieve management activity subtypes.'),
  });

  const addManagementActivity = useApiRequestWrapper<
    (
      managementFileId: number,
      activity: ApiGen_Concepts_PropertyActivity,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyActivity, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, activity: ApiGen_Concepts_PropertyActivity) =>
        await postActivityApi(managementFileId, activity),
      [postActivityApi],
    ),
    requestName: 'AddManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to create a management file activity.'),
  });

  const getManagementActivity = useApiRequestWrapper<
    (
      managementFileId: number,
      propertyActivityId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyActivity, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, propertyActivityId: number) =>
        await getActivityApi(managementFileId, propertyActivityId),
      [getActivityApi],
    ),
    requestName: 'GetManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retrieve a management file activity.'),
  });

  const getManagementActivities = useApiRequestWrapper<
    (managementFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyActivity[], any>>
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
    (managementFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyActivity[], any>>
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
    (managementFileId: number, propertyActivityId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, propertyActivityId: number) =>
        await deleteActivityApi(managementFileId, propertyActivityId),
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
      getActivitySubtypes,
      addManagementActivity,
      getManagementActivity,
      getManagementActivities,
      getManagementFileActivities,
      deleteManagementActivity,
    }),
    [
      getActivitySubtypes,
      addManagementActivity,
      getManagementActivity,
      getManagementActivities,
      getManagementFileActivities,
      deleteManagementActivity,
    ],
  );
};
