import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_PropertyManagement } from '@/models/api/Property';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import {
  deletePropertyManagementActivitiesApi,
  getPropertyManagementActivitiesApi,
  getPropertyManagementApi,
  putPropertyManagementApi,
} from '../pims-api/useApiPropertyManagement';

/**
 * hook that interacts with the property management API.
 */
export const usePropertyManagementRepository = () => {
  const getPropertyManagementWrapper = useApiRequestWrapper<typeof getPropertyManagementApi>({
    requestFunction: useCallback(
      async (propertyId: number) => await getPropertyManagementApi(propertyId),
      [],
    ),
    requestName: 'getPropertyManagement',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load property management.'),
  });

  const updatePropertyManagementWrapper = useApiRequestWrapper<typeof putPropertyManagementApi>({
    requestFunction: useCallback(
      async (propertyId: number, propertyManagement: Api_PropertyManagement) =>
        await putPropertyManagementApi(propertyId, propertyManagement),
      [],
    ),
    requestName: 'updatePropertyManagement',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update property management.'),
  });

  const getPropertyManagementActivitiesWrapper = useApiRequestWrapper<
    typeof getPropertyManagementActivitiesApi
  >({
    requestFunction: useCallback(
      async (propertyId: number) => await getPropertyManagementActivitiesApi(propertyId),
      [],
    ),
    requestName: 'getPropertyManagementActivities',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load property management activities list.'),
  });

  const deletePropertyManagementActivityWrapper = useApiRequestWrapper<
    (propertyId: number, managementActivityId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, managementActivityId: number) =>
        await deletePropertyManagementActivitiesApi(propertyId, managementActivityId),
      [],
    ),
    requestName: 'deletePropertyManagementActivity',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to delete property management activity. Refresh the page to try again.',
    ),
  });

  return useMemo(
    () => ({
      getPropertyManagement: getPropertyManagementWrapper,
      updatePropertyManagement: updatePropertyManagementWrapper,
      getPropertyManagementActivities: getPropertyManagementActivitiesWrapper,
      deletePropertyManagementActivity: deletePropertyManagementActivityWrapper,
    }),
    [
      deletePropertyManagementActivityWrapper,
      getPropertyManagementActivitiesWrapper,
      getPropertyManagementWrapper,
      updatePropertyManagementWrapper,
    ],
  );
};
