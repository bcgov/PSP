import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosSuccessHandler } from '@/utils';

import { useApiPropertyOperation } from '../pims-api/useApiPropertyOperation';

const ignoreErrorCodes = [409];

/**
 * hook that interacts with the PropertyOperation File API.
 */
export const usePropertyOperationRepository = () => {
  const { postPropertyOperationApi } = useApiPropertyOperation();

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
    onSuccess: useAxiosSuccessHandler('Property operation saved'),
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  return useMemo(
    () => ({
      addPropertyOperationApi: addPropertyOperationApi,
    }),
    [addPropertyOperationApi],
  );
};
