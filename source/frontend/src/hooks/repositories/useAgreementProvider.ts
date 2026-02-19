import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiAgreements } from '@/hooks/pims-api/useApiAgreements';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { useAxiosErrorHandler } from '@/utils';

/**
 * hook that interacts with the Agreements API.
 */
export const useAgreementProvider = () => {
  const {
    getAcquisitionAgreementsApi,
    getAcquisitionAgreementByIdApi,
    postAcquisitionAgreementApi,
    putAcquisitionAgreementApi,
    deleteAcquisitionAgreementApi,
    getDispositionAgreementsApi,
    getDispositionAgreementByIdApi,
    postDispositionAgreementApi,
    putDispositionAgreementApi,
    deleteDispositionAgreementApi,
  } = useApiAgreements();
  // Disposition agreement operations
  const getDispositionFileAgreements = useApiRequestWrapper<
    (dspFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_Agreement[], any>>
  >({
    requestFunction: useCallback(
      async (dspFileId: number) => await getDispositionAgreementsApi(dspFileId),
      [getDispositionAgreementsApi],
    ),
    requestName: 'getDispositionFileAgreements',
    onError: useAxiosErrorHandler('Failed to load Disposition File Agreements'),
  });

  const getDispositionAgreementById = useApiRequestWrapper<
    (
      dspFileId: number,
      agreementId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Agreement, any>>
  >({
    requestFunction: useCallback(
      async (dspFileId: number, agreementId: number) =>
        await getDispositionAgreementByIdApi(dspFileId, agreementId),
      [getDispositionAgreementByIdApi],
    ),
    requestName: 'getDispositionAgreementById',
    onError: useAxiosErrorHandler('Failed to load Disposition File Agreement'),
  });

  const addDispositionAgreement = useApiRequestWrapper<
    (
      dspFileId: number,
      agreement: ApiGen_Concepts_Agreement,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Agreement, any>>
  >({
    requestFunction: useCallback(
      async (dspFileId: number, agreement: ApiGen_Concepts_Agreement) =>
        await postDispositionAgreementApi(dspFileId, agreement),
      [postDispositionAgreementApi],
    ),
    requestName: 'addDispositionAgreement',
    onError: useAxiosErrorHandler('Failed to create Disposition File Agreement'),
  });

  const updateDispositionAgreement = useApiRequestWrapper<
    (
      dspFileId: number,
      agreementId: number,
      agreement: ApiGen_Concepts_Agreement,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Agreement, any>>
  >({
    requestFunction: useCallback(
      async (dspFileId: number, agreementId: number, agreement: ApiGen_Concepts_Agreement) =>
        await putDispositionAgreementApi(dspFileId, agreementId, agreement),
      [putDispositionAgreementApi],
    ),
    requestName: 'updateDispositionAgreement',
    onError: useAxiosErrorHandler('Failed to update Disposition File Agreement'),
  });

  const deleteDispositionAgreement = useApiRequestWrapper<
    (dspFileId: number, agreementId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (dspFileId: number, agreementId: number) =>
        await deleteDispositionAgreementApi(dspFileId, agreementId),
      [deleteDispositionAgreementApi],
    ),
    requestName: 'DeleteDispositionAgreement',
    onError: useAxiosErrorHandler('Failed to Delete Disposition File Agreement'),
  });

  const getAcquisitionAgreements = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_Agreement[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionAgreementsApi(acqFileId),
      [getAcquisitionAgreementsApi],
    ),
    requestName: 'getAcquisitionAgreements',
    onError: useAxiosErrorHandler('Failed to load Acquisition File Agreements'),
  });

  const getAcquisitionAgreementById = useApiRequestWrapper<
    (
      acqFileId: number,
      agreementId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Agreement, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, agreementId: number) =>
        await getAcquisitionAgreementByIdApi(acqFileId, agreementId),
      [getAcquisitionAgreementByIdApi],
    ),
    requestName: 'getAcquisitionAgreementById',
    onError: useAxiosErrorHandler('Failed to load Acquisition File Agreement'),
  });

  const addAcquisitionAgreement = useApiRequestWrapper<
    (
      acqFileId: number,
      agreements: ApiGen_Concepts_Agreement,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Agreement, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, agreement: ApiGen_Concepts_Agreement) =>
        await postAcquisitionAgreementApi(acqFileId, agreement),
      [postAcquisitionAgreementApi],
    ),
    requestName: 'addAcquisitionAgreement',
    onError: useAxiosErrorHandler('Failed to create Acquisition File Agreement'),
  });

  const updateAcquisitionAgreement = useApiRequestWrapper<
    (
      acqFileId: number,
      agreementId: number,
      agreements: ApiGen_Concepts_Agreement,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Agreement, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, agreementId: number, agreement: ApiGen_Concepts_Agreement) =>
        await putAcquisitionAgreementApi(acqFileId, agreementId, agreement),
      [putAcquisitionAgreementApi],
    ),
    requestName: 'updateAcquisitionAgreement',
    onError: useAxiosErrorHandler('Failed to update Acquisition File Agreement'),
  });

  const deleteAcquisitionAgreement = useApiRequestWrapper<
    (acqFileId: number, agreementId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, agreementId: number) =>
        await deleteAcquisitionAgreementApi(acqFileId, agreementId),
      [deleteAcquisitionAgreementApi],
    ),
    requestName: 'DeleteAcquisitionAgreement',
    onError: useAxiosErrorHandler('Failed to Delete Acquisition File Agreement'),
  });

  return useMemo(
    () => ({
      // Acquisition
      getAcquisitionAgreements,
      getAcquisitionAgreementById,
      addAcquisitionAgreement,
      updateAcquisitionAgreement,
      deleteAcquisitionAgreement,
      // Disposition
      getDispositionFileAgreements,
      getDispositionAgreementById,
      addDispositionAgreement,
      updateDispositionAgreement,
      deleteDispositionAgreement,
    }),
    [
      getAcquisitionAgreements,
      getAcquisitionAgreementById,
      addAcquisitionAgreement,
      updateAcquisitionAgreement,
      deleteAcquisitionAgreement,
      getDispositionFileAgreements,
      getDispositionAgreementById,
      addDispositionAgreement,
      updateDispositionAgreement,
      deleteDispositionAgreement,
    ],
  );
};
