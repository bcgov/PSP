import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiPropertyOperation } from '../pims-api/useApiPropertyOperation';

/**
 * hook that interacts with the PropertyOperation File API.
 */
export const usePropertyOperationRepository = () => {
  const { postPropertyOperationApi, getPropertyOperationsApi } = useApiPropertyOperation();

  const addPropertyOperationApi = useApiRequestWrapper<
    (
      propertyOperations: ApiGen_Concepts_PropertyOperation[],
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyOperation[], any>>
  >({
    requestFunction: useCallback(
      async (
        propertyOperations: ApiGen_Concepts_PropertyOperation[],
        useOverride: UserOverrideCode[] = [],
      ) => await postPropertyOperationApi(propertyOperations, useOverride),
      [postPropertyOperationApi],
    ),
    requestName: 'AddPropertyOperation',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

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
      addPropertyOperationApi: addPropertyOperationApi,
      getPropertyOperations,
    }),
    [addPropertyOperationApi, getPropertyOperations],
  );
};
