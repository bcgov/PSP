import React from 'react';

import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the takes endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiTakes = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getTakesByAcqFileId: (fileId: number) =>
        api.get<ApiGen_Concepts_Take[]>(`/takes/acquisition/${fileId}`),
      getTakesByPropertyId: (fileId: number, propertyId: number) =>
        api.get<ApiGen_Concepts_Take[]>(`/takes/acquisition/${fileId}/property/${propertyId}`),
      getTakesCountByPropertyId: (propertyId: number) =>
        api.get<number>(`/takes/property/${propertyId}/count`),
      updateTakesCountByPropertyId: (
        acquisitionFilePropertyId: number,
        takes: ApiGen_Concepts_Take[],
      ) => api.put<number>(`/takes/acquisition/property/${acquisitionFilePropertyId}`, takes),
    }),
    [api],
  );
};
