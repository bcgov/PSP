import React from 'react';

import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the PropertyOperations endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiPropertyOperation = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getPropertyOperationsApi: (id: number) =>
        api.get<ApiGen_Concepts_PropertyOperation[]>(`/properties/${id}/propertyOperations`),
    }),
    [api],
  );
};
