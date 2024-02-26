import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiPropertyOperation } from '../pims-api/useApiPropertyOperation';

/**
 * hook that interacts with the Property Operations API.
 */
export const usePropertyOperationRepository = () => {
  const { getPropertyOperationsApi } = useApiPropertyOperation();

  const getPropertyOperations = useApiRequestWrapper<
    (propertyId: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyOperation[], any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number) => await getPropertyOperationsApi(propertyId),
      [getPropertyOperationsApi],
    ),
    requestName: 'GetPropertyOperations',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      getPropertyOperations,
    }),
    [getPropertyOperations],
  );
};
