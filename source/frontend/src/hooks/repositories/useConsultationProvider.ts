import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';
import { useAxiosErrorHandler } from '@/utils';

import { useApiConsultations } from '../pims-api/useApiConsultations';

/**
 * hook that interacts with the Consultations API.
 */
export const useConsultationProvider = () => {
  const {
    getLeaseConsultationsApi,
    getLeaseConsultationByIdApi,
    postLeaseConsultationApi,
    putLeaseConsultationApi,
    deleteLeaseConsultationApi,
  } = useApiConsultations();

  const getLeaseConsultations = useApiRequestWrapper<
    (leaseFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_ConsultationLease[], any>>
  >({
    requestFunction: useCallback(
      async (leaseFileId: number) => await getLeaseConsultationsApi(leaseFileId),
      [getLeaseConsultationsApi],
    ),
    requestName: 'getLeaseConsultations',
    onError: useAxiosErrorHandler('Failed to load Lease File Consultations'),
  });

  const getLeaseConsultationById = useApiRequestWrapper<
    (
      leaseFileId: number,
      consultationId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ConsultationLease, any>>
  >({
    requestFunction: useCallback(
      async (leaseFileId: number, consultationId: number) =>
        await getLeaseConsultationByIdApi(leaseFileId, consultationId),
      [getLeaseConsultationByIdApi],
    ),
    requestName: 'getLeaseConsultationById',
    onError: useAxiosErrorHandler('Failed to load Lease File Consultation'),
  });

  const addLeaseConsultation = useApiRequestWrapper<
    (
      acqFileId: number,
      agreements: ApiGen_Concepts_ConsultationLease,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ConsultationLease, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, agreement: ApiGen_Concepts_ConsultationLease) =>
        await postLeaseConsultationApi(acqFileId, agreement),
      [postLeaseConsultationApi],
    ),
    requestName: 'addLeaseConsultation',
    onError: useAxiosErrorHandler('Failed to create Lease File Consultation'),
  });

  const updateLeaseConsultation = useApiRequestWrapper<
    (
      leaseFileId: number,
      consultationId: number,
      consultation: ApiGen_Concepts_ConsultationLease,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ConsultationLease, any>>
  >({
    requestFunction: useCallback(
      async (
        leaseFileId: number,
        consultationId: number,
        consultation: ApiGen_Concepts_ConsultationLease,
      ) => await putLeaseConsultationApi(leaseFileId, consultationId, consultation),
      [putLeaseConsultationApi],
    ),
    requestName: 'updateLeaseConsultation',
    throwError: true,
  });

  const deleteLeaseConsultation = useApiRequestWrapper<
    (leaseFileId: number, consultationId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (leaseFileId: number, consultationId: number) =>
        await deleteLeaseConsultationApi(leaseFileId, consultationId),
      [deleteLeaseConsultationApi],
    ),
    requestName: 'DeleteLeaseConsultation',
    onError: useAxiosErrorHandler('Failed to Delete Lease File Consultation'),
  });

  return useMemo(
    () => ({
      getLeaseConsultations,
      getLeaseConsultationById,
      addLeaseConsultation,
      updateLeaseConsultation,
      deleteLeaseConsultation,
    }),
    [
      getLeaseConsultations,
      getLeaseConsultationById,
      addLeaseConsultation,
      updateLeaseConsultation,
      deleteLeaseConsultation,
    ],
  );
};
