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
  const { getAcquisitionAgreementsApi, postAcquisitionAgreementsApi } = useApiAgreements();

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

  const updateAcquisitionAgreements = useApiRequestWrapper<
    (
      acqFileId: number,
      agreements: ApiGen_Concepts_Agreement[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_Agreement[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, agreement: ApiGen_Concepts_Agreement[]) =>
        await postAcquisitionAgreementsApi(acqFileId, agreement),
      [postAcquisitionAgreementsApi],
    ),
    requestName: 'updateAcquisitionAgreements',
    onError: useAxiosErrorHandler('Failed to update Acquisition File Agreements'),
  });

  return useMemo(
    () => ({
      getAcquisitionAgreements,
      updateAcquisitionAgreements,
    }),
    [getAcquisitionAgreements, updateAcquisitionAgreements],
  );
};
