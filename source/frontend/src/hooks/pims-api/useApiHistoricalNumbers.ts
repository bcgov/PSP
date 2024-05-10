import React from 'react';

import { ApiGen_Concepts_PropertyFileNumber } from '@/models/api/generated/ApiGen_Concepts_PropertyFileNumber';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the interest holder endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiHistoricalNumbers = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getByPropertyId: (propertyId: number) =>
        api.get<ApiGen_Concepts_PropertyFileNumber[]>(
          `/properties/${propertyId}/historicalNumbers`,
        ),
    }),
    [api],
  );
};
