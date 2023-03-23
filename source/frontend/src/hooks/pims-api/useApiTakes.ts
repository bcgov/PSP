import React from 'react';

import { Api_Take } from './../../models/api/Take';
import { useAxiosApi } from './';

/**
 * PIMS API wrapper to centralize all AJAX requests to the takes endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiTakes = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getTakesByAcqFileId: (fileId: number) => api.get<Api_Take[]>(`/takes/acquisition/${fileId}`),
      getTakesCountByPropertyId: (propertyId: number) =>
        api.get<number>(`/takes/property/${propertyId}/count`),
      updateTakesCountByPropertyId: (acquisitionFilePropertyId: number, takes: Api_Take[]) =>
        api.put<number>(`/takes/acquisition/property/${acquisitionFilePropertyId}`, takes),
    }),
    [api],
  );
};
