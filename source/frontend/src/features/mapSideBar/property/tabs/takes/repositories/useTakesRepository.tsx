import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiTakes } from '@/hooks/pims-api/useApiTakes';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that interacts with the Takes API.
 */
export const useTakesRepository = () => {
  const {
    getTakesByAcqFileId,
    getTakesCountByPropertyId,
    addTakeByFilePropertyId,
    getTakeById,
    updateTakeByFilePropertyId,
    deleteTakeByFilePropertyId,
    getTakesByPropertyId,
  } = useApiTakes();

  const getTakeByIdApi = useApiRequestWrapper<
    (
      acquisitionFilePropertyId: number,
      takeId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Take, any>>
  >({
    requestFunction: useCallback(
      async (acquisitionFilePropertyId: number, takeId: number) =>
        await getTakeById(acquisitionFilePropertyId, takeId),
      [getTakeById],
    ),
    requestName: 'GetTakeByIdApi',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

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

  const addTakeByAcquisitionPropertyIdApi = useApiRequestWrapper<
    (
      acquisitionFilePropertyId: number,
      take: ApiGen_Concepts_Take,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Take, any>>
  >({
    requestFunction: useCallback(
      async (acquisitionFilePropertyId: number, take: ApiGen_Concepts_Take) =>
        await addTakeByFilePropertyId(acquisitionFilePropertyId, take),
      [addTakeByFilePropertyId],
    ),
    requestName: 'AddTakeByAcquisitionPropertyIdApi',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
    throwError: true,
  });

  const updateTakeByAcquisitionPropertyIdApi = useApiRequestWrapper<
    (
      acquisitionFilePropertyId: number,
      take: ApiGen_Concepts_Take,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Take, any>>
  >({
    requestFunction: useCallback(
      async (acquisitionFilePropertyId: number, take: ApiGen_Concepts_Take) =>
        await updateTakeByFilePropertyId(acquisitionFilePropertyId, take),
      [updateTakeByFilePropertyId],
    ),
    requestName: 'UpdateTakesByAcquisitionPropertyId',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
    throwError: true,
  });

  const deleteTakeByAcquisitionPropertyIdApi = useApiRequestWrapper<
    (
      acquisitionFilePropertyId: number,
      takeId: number,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<number, any>>
  >({
    requestFunction: useCallback(
      async (
        acquisitionFilePropertyId: number,
        takeId: number,
        userOverrideCodes: UserOverrideCode[],
      ) => await deleteTakeByFilePropertyId(acquisitionFilePropertyId, takeId, userOverrideCodes),
      [deleteTakeByFilePropertyId],
    ),
    requestName: 'deleteTakeByAcquisitionPropertyIdApi',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  return useMemo(
    () => ({
      getTakeById: getTakeByIdApi,
      getTakesByFileId: getTakesByFileIdApi,
      getTakesByPropertyId: getTakesByPropertyApi,
      getTakesCountByPropertyId: getTakesCountByPropertyIdApi,
      addTakeByAcquisitionPropertyId: addTakeByAcquisitionPropertyIdApi,
      updateTakeByAcquisitionPropertyId: updateTakeByAcquisitionPropertyIdApi,
      deleteTakeByAcquisitionPropertyId: deleteTakeByAcquisitionPropertyIdApi,
    }),
    [
      getTakeByIdApi,
      getTakesByFileIdApi,
      getTakesByPropertyApi,
      getTakesCountByPropertyIdApi,
      addTakeByAcquisitionPropertyIdApi,
      updateTakeByAcquisitionPropertyIdApi,
      deleteTakeByAcquisitionPropertyIdApi,
    ],
  );
};
