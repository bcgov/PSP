import React from 'react';

import { ApiGen_Concepts_PropertyTenureCleanup } from '@/models/api/generated/ApiGen_Concepts_PropertyTenureCleanup';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the proeprty tenure cleanup endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiPropertyTenureCleanup = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getByPropertyId: (propertyId: number) =>
        api.get<ApiGen_Concepts_PropertyTenureCleanup[]>(
          `/properties/${propertyId}/tenureCleanups`,
        ),
    }),
    [api],
  );
};
