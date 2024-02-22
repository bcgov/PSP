import React from 'react';

import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the interest holder endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiInterestHolders = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getAcquisitionInterestHolderApi: (acqFileId: number) =>
        api.get<ApiGen_Concepts_InterestHolder[]>(`/acquisitionfiles/${acqFileId}/interestholders`),
      postAcquisitionholderApi: (acqFileId: number, agreements: ApiGen_Concepts_InterestHolder[]) =>
        api.put<ApiGen_Concepts_InterestHolder[]>(
          `/acquisitionfiles/${acqFileId}/interestholders`,
          agreements,
        ),
    }),
    [api],
  );
};
