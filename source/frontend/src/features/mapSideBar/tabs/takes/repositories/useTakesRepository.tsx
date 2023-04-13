import { AxiosResponse } from 'axios';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useApiTakes } from 'hooks/pims-api/useApiTakes';
import { Api_Take } from 'models/api/Take';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that interacts with the Takes API.
 */
export const useTakesRepository = () => {
  const { getTakesByAcqFileId, getTakesCountByPropertyId, updateTakesCountByPropertyId } =
    useApiTakes();

  const getTakesByFileIdApi = useApiRequestWrapper<
    (fileId: number) => Promise<AxiosResponse<Api_Take[], any>>
  >({
    requestFunction: useCallback(
      async (fileId: number) => await getTakesByAcqFileId(fileId),
      [getTakesByAcqFileId],
    ),
    requestName: 'GetTakesByAcqFileId',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const getTakesCountByPropertyIdApi = useApiRequestWrapper<
    (propertyId: number) => Promise<AxiosResponse<number, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number) => await getTakesCountByPropertyId(propertyId),
      [getTakesCountByPropertyId],
    ),
    requestName: 'GetTakesCountByPropertyId',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateTakesByAcquisitionPropertyIdApi = useApiRequestWrapper<
    (acquisitionFilePropertyId: number, takes: Api_Take[]) => Promise<AxiosResponse<number, any>>
  >({
    requestFunction: useCallback(
      async (acquisitionFilePropertyId: number, takes: Api_Take[]) =>
        await updateTakesCountByPropertyId(acquisitionFilePropertyId, takes),
      [updateTakesCountByPropertyId],
    ),
    requestName: 'UpdateTakesByAcquisitionPropertyId',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      getTakesByFileId: getTakesByFileIdApi,
      getTakesCountByPropertyId: getTakesCountByPropertyIdApi,
      updateTakesByAcquisitionPropertyId: updateTakesByAcquisitionPropertyIdApi,
    }),
    [getTakesByFileIdApi, getTakesCountByPropertyIdApi, updateTakesByAcquisitionPropertyIdApi],
  );
};
