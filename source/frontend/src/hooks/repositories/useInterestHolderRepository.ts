import { AxiosResponse } from 'axios';
import { useApiInterestHolders } from 'hooks/pims-api/useApiInterestHolders';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_InterestHolder } from 'models/api/InterestHolder';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler } from 'utils';

/**
 * hook that interacts with the InterestHolde API.
 */
export const useInterestHolderRepository = () => {
  const { getAcquisitionInterestHolderApi, postAcquisitionholderApi } = useApiInterestHolders();

  const getAcquisitionInterestHolders = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_InterestHolder[], any>>
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
      agreements: Api_InterestHolder[],
    ) => Promise<AxiosResponse<Api_InterestHolder[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, stakeholder: Api_InterestHolder[]) =>
        await postAcquisitionholderApi(acqFileId, stakeholder),
      [postAcquisitionholderApi],
    ),
    requestName: 'updateAcquisitionInterestHolder',
    onError: useAxiosErrorHandler('Failed to update Acquisition File InterestHolder'),
  });

  return useMemo(
    () => ({
      getAcquisitionInterestHolders,
      updateAcquisitionInterestHolders,
    }),
    [getAcquisitionInterestHolders, updateAcquisitionInterestHolders],
  );
};
