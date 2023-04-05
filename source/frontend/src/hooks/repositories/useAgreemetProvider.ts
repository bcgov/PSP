import { AxiosResponse } from 'axios';
import { useApiAgreements } from 'hooks/pims-api/useApiAgreements';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_Agreement } from 'models/api/Agreement';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler } from 'utils';

/**
 * hook that interacts with the Acquisition File API.
 */
export const useAgreementProvider = () => {
  const { getAcquisitionAgreementsApi } = useApiAgreements();

  const getAcquisitionAgreements = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_Agreement[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionAgreementsApi(acqFileId),
      [getAcquisitionAgreementsApi],
    ),
    requestName: 'getAcquisitionAgreements',
    onError: useAxiosErrorHandler('Failed to load Acquisition File Agreements'),
  });

  return useMemo(
    () => ({
      getAcquisitionAgreements,
    }),
    [getAcquisitionAgreements],
  );
};
