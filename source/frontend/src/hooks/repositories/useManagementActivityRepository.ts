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
  const { getActivitySubtypesApi, getActivityApi, postActivityApi, putActivityApi } =
    useApiManagementActivities();

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

  const getManagementActivity = useApiRequestWrapper<
    (
      managementFileId: number,
      activityId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyActivity, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, activityId: number) =>
        await getActivityApi(managementFileId, activityId),
      [getActivityApi],
    ),
    requestName: 'GetManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retrieve a management file activity.'),
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

  const updateManagementActivity = useApiRequestWrapper<
    (
      managementFileId: number,
      activity: ApiGen_Concepts_PropertyActivity,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyActivity, any>>
  >({
    requestFunction: useCallback(
      async (managementFileId: number, activity: ApiGen_Concepts_PropertyActivity) =>
        await putActivityApi(managementFileId, activity),
      [putActivityApi],
    ),
    requestName: 'UpdateManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update a management file activity.'),
  });

  return useMemo(
    () => ({
      getActivitySubtypes,
      getManagementActivity,
      addManagementActivity,
      updateManagementActivity,
    }),
    [getActivitySubtypes, getManagementActivity, addManagementActivity, updateManagementActivity],
  );
};
