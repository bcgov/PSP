import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiTakes } from '@/hooks/pims-api/useApiTakes';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that interacts with the Takes API.
 */
export const useTakesRepository = () => {
  const {
    getTakesByAcqFileId,
    getTakesCountByPropertyId,
    updateTakesCountByPropertyId,
    getTakesByPropertyId,
  } = useApiTakes();

  const getTakesByFileIdApi = useApiRequestWrapper<
    (fileId: number) => Promise<AxiosResponse<ApiGen_Concepts_Take[], any>>
  >({
    requestFunction: useCallback(
      async (fileId: number) => await getTakesByAcqFileId(fileId),
      [getTakesByAcqFileId],
    ),
    requestName: 'GetTakesByAcqFileId',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const getTakesByPropertyApi = useApiRequestWrapper<
    (fileId: number, propertyId: number) => Promise<AxiosResponse<ApiGen_Concepts_Take[], any>>
  >({
    requestFunction: useCallback(
      async (fileId: number, propertyId: number) => await getTakesByPropertyId(fileId, propertyId),
      [getTakesByPropertyId],
    ),
    requestName: 'GetTakesByAcqPropertyId',
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
    (
      acquisitionFilePropertyId: number,
      takes: ApiGen_Concepts_Take[],
    ) => Promise<AxiosResponse<number, any>>
  >({
    requestFunction: useCallback(
      async (acquisitionFilePropertyId: number, takes: ApiGen_Concepts_Take[]) =>
        await updateTakesCountByPropertyId(acquisitionFilePropertyId, takes),
      [updateTakesCountByPropertyId],
    ),
    requestName: 'UpdateTakesByAcquisitionPropertyId',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
    throwError: true,
  });

  return useMemo(
    () => ({
      getTakesByFileId: getTakesByFileIdApi,
      getTakesByPropertyId: getTakesByPropertyApi,
      getTakesCountByPropertyId: getTakesCountByPropertyIdApi,
      updateTakesByAcquisitionPropertyId: updateTakesByAcquisitionPropertyIdApi,
    }),
    [
      getTakesByFileIdApi,
      getTakesByPropertyApi,
      getTakesCountByPropertyIdApi,
      updateTakesByAcquisitionPropertyIdApi,
    ],
  );
};
