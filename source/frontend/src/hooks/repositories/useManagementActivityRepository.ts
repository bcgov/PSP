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
  const { getActivitySubtypesApi, postActivityApi } = useApiManagementActivities();

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

  return useMemo(
    () => ({
      getActivitySubtypes,
      addManagementActivity,
    }),
    [getActivitySubtypes, addManagementActivity],
  );
};
