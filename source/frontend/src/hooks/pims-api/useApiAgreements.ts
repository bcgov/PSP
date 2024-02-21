import React from 'react';

import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the agreements endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiAgreements = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getAcquisitionAgreementsApi: (acqFileId: number) =>
        api.get<ApiGen_Concepts_Agreement[]>(`/acquisitionfiles/${acqFileId}/agreements`),
      postAcquisitionAgreementsApi: (acqFileId: number, agreements: ApiGen_Concepts_Agreement[]) =>
        api.post<ApiGen_Concepts_Agreement[]>(
          `/acquisitionfiles/${acqFileId}/agreements`,
          agreements,
        ),
    }),
    [api],
  );
};
