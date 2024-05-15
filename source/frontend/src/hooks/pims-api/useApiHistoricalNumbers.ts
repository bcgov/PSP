import React from 'react';

import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the historic number endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiHistoricalNumbers = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getByPropertyId: (propertyId: number) =>
        api.get<ApiGen_Concepts_HistoricalFileNumber[]>(
          `/properties/${propertyId}/historicalNumbers`,
        ),
      putHistoricalNumbers: (
        propertyId: number,
        historicalNumbers: ApiGen_Concepts_HistoricalFileNumber[],
      ) =>
        api.put<ApiGen_Concepts_HistoricalFileNumber[]>(
          `/properties/${propertyId}/historicalNumbers`,
          historicalNumbers,
        ),
    }),
    [api],
  );
};
