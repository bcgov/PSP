import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiInterestHolders } from '@/hooks/pims-api/useApiInterestHolders';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { useAxiosErrorHandler } from '@/utils';

const ignoreErrorCodes = [409];

/**
 * hook that interacts with the InterestHolde API.
 */
export const useInterestHolderRepository = () => {
  const { getAcquisitionInterestHolderApi, postAcquisitionholderApi } = useApiInterestHolders();

  const getAcquisitionInterestHolders = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_InterestHolder[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionInterestHolderApi(acqFileId),
      [getAcquisitionInterestHolderApi],
    ),
    requestName: 'getAcquisitionInterestHolder',
    onError: useAxiosErrorHandler('Failed to load Acquisition File InterestHolder'),
  });

  const updateAcquisitionInterestHolders = useApiRequestWrapper<
    (
      acqFileId: number,
      agreements: ApiGen_Concepts_InterestHolder[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_InterestHolder[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, stakeholder: ApiGen_Concepts_InterestHolder[]) =>
        await postAcquisitionholderApi(acqFileId, stakeholder),
      [postAcquisitionholderApi],
    ),
    requestName: 'updateAcquisitionInterestHolder',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  return useMemo(
    () => ({
      getAcquisitionInterestHolders,
      updateAcquisitionInterestHolders,
    }),
    [getAcquisitionInterestHolders, updateAcquisitionInterestHolders],
  );
};
