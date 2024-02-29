import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import {
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
      async (propertyId: number, propertyManagement: ApiGen_Concepts_PropertyManagement) =>
        await putPropertyManagementApi(propertyId, propertyManagement),
      [],
    ),
    requestName: 'updatePropertyManagement',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update property management.'),
  });

  return useMemo(
    () => ({
      getPropertyManagement: getPropertyManagementWrapper,
      updatePropertyManagement: updatePropertyManagementWrapper,
    }),
    [getPropertyManagementWrapper, updatePropertyManagementWrapper],
  );
};
