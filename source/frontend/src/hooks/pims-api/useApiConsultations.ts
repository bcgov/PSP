import React from 'react';

import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the consultations endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiConsultations = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getLeaseConsultationsApi: (leaseFileId: number) =>
        api.get<ApiGen_Concepts_ConsultationLease[]>(`/leases/${leaseFileId}/consultations`),
      getLeaseConsultationByIdApi: (leaseFileId: number, consultationId: number) =>
        api.get<ApiGen_Concepts_ConsultationLease>(
          `/leases/${leaseFileId}/consultations/${consultationId}`,
        ),
      postLeaseConsultationApi: (
        leaseFileId: number,
        consultation: ApiGen_Concepts_ConsultationLease,
      ) =>
        api.post<ApiGen_Concepts_ConsultationLease>(
          `/leases/${leaseFileId}/consultations`,
          consultation,
        ),
      putLeaseConsultationApi: (
        leaseFileId: number,
        agreementId: number,
        consultation: ApiGen_Concepts_ConsultationLease,
      ) =>
        api.put<ApiGen_Concepts_ConsultationLease>(
          `/leases/${leaseFileId}/consultations/${agreementId}`,
          consultation,
        ),
      deleteLeaseConsultationApi: (leaseFileId: number, agreementId: number) =>
        api.delete<boolean>(`/leases/${leaseFileId}/consultations/${agreementId}`),
    }),
    [api],
  );
};
