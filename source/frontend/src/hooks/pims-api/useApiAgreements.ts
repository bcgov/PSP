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
      getAcquisitionAgreementByIdApi: (acqFileId: number, agreementId: number) =>
        api.get<ApiGen_Concepts_Agreement>(
          `/acquisitionfiles/${acqFileId}/agreements/${agreementId}`,
        ),
      postAcquisitionAgreementApi: (acqFileId: number, agreement: ApiGen_Concepts_Agreement) =>
        api.post<ApiGen_Concepts_Agreement>(`/acquisitionfiles/${acqFileId}/agreements`, agreement),
      putAcquisitionAgreementApi: (
        acqFileId: number,
        agreementId: number,
        agreement: ApiGen_Concepts_Agreement,
      ) =>
        api.put<ApiGen_Concepts_Agreement>(
          `/acquisitionfiles/${acqFileId}/agreements/${agreementId}`,
          agreement,
        ),
      deleteAcquisitionAgreementApi: (acqFileId: number, agreementId: number) =>
        api.delete<boolean>(`/acquisitionfiles/${acqFileId}/agreements/${agreementId}`),
    }),
    [api],
  );
};
