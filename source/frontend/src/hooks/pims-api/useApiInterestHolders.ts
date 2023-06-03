import { Api_InterestHolder } from 'models/api/InterestHolder';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the interest holder endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiInterestHolders = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getAcquisitionInterestHolderApi: (acqFileId: number) =>
        api.get<Api_InterestHolder[]>(`/acquisitionfiles/${acqFileId}/interestholders`),
      postAcquisitionholderApi: (acqFileId: number, agreements: Api_InterestHolder[]) =>
        api.post<Api_InterestHolder[]>(
          `/acquisitionfiles/${acqFileId}/interestholders`,
          agreements,
        ),
    }),
    [api],
  );
};
