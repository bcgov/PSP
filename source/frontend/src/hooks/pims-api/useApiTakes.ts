import React from 'react';

import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

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
      getTakeById: (acquisitionFilePropertyId: number, takeId: number) =>
        api.get<ApiGen_Concepts_Take>(
          `/takes/acquisition/property/${acquisitionFilePropertyId}/takes/${takeId}`,
        ),
      addTakeByFilePropertyId: (acquisitionFilePropertyId: number, take: ApiGen_Concepts_Take) =>
        api.post<ApiGen_Concepts_Take>(
          `/takes/acquisition/property/${acquisitionFilePropertyId}/takes`,
          take,
        ),
      updateTakeByFilePropertyId: (acquisitionFilePropertyId: number, take: ApiGen_Concepts_Take) =>
        api.put<ApiGen_Concepts_Take>(
          `/takes/acquisition/property/${acquisitionFilePropertyId}/takes/${take.id}`,
          take,
        ),
      deleteTakeByFilePropertyId: (
        acquisitionFilePropertyId: number,
        takeId: number,
        userOverrideCodes: UserOverrideCode[],
      ) =>
        api.delete(
          `/takes/acquisition/property/${acquisitionFilePropertyId}/takes/${takeId}?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
        ),
    }),
    [api],
  );
};
