import React from 'react';

import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

export type IPaginateDisposition = IPaginateRequest<Api_DispositionFilter>;

/**
 * PIMS API wrapper to centralize all AJAX requests to the PropertyOperations endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiPropertyOperation = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      postPropertyOperationApi: (
        propertyOperations: ApiGen_Concepts_PropertyOperation[],
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.post<ApiGen_Concepts_PropertyOperation[]>(
          `/property/operations?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          propertyOperations,
        ),
      getPropertyOperationsApi: (id: number) =>
        api.get<ApiGen_Concepts_PropertyOperation[]>(`/properties/${id}/propertyOperations`),
    }),
    [api],
  );
};
