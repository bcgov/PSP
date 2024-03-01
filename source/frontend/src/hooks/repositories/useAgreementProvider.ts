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
  } = useApiAgreements();

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
      getAcquisitionAgreements,
      getAcquisitionAgreementById,
      addAcquisitionAgreement,
      updateAcquisitionAgreement,
      deleteAcquisitionAgreement,
    }),
    [
      getAcquisitionAgreements,
      getAcquisitionAgreementById,
      addAcquisitionAgreement,
      updateAcquisitionAgreement,
      deleteAcquisitionAgreement,
    ],
  );
};
