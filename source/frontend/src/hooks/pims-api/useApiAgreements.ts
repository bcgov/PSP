import React from 'react';

import { Api_Agreement } from '@/models/api/Agreement';

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
        api.get<Api_Agreement[]>(`/acquisitionfiles/${acqFileId}/agreements`),
      postAcquisitionAgreementsApi: (acqFileId: number, agreements: Api_Agreement[]) =>
        api.post<Api_Agreement[]>(`/acquisitionfiles/${acqFileId}/agreements`, agreements),
    }),
    [api],
  );
};
