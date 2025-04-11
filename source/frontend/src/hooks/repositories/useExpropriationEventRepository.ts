import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiExpropriationEvents } from '@/hooks/pims-api/useApiExpropriationEvents';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_ExpropriationEvent } from '@/models/api/generated/ApiGen_Concepts_ExpropriationEvent';
import { useAxiosErrorHandler } from '@/utils';

/**
 * hook that interacts with the Acquisition Expropriation Events API.
 */
export const useExpropriationEventRepository = () => {
  const {
    getAcquisitionExpropriationEventsApi,
    getAcquisitionExpropriationEventByIdApi,
    postAcquisitionExpropriationEventApi,
    putAcquisitionExpropriationEventApi,
    deleteAcquisitionExpropriationEventApi,
  } = useApiExpropriationEvents();

  const getExpropriationEvents = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_ExpropriationEvent[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionExpropriationEventsApi(acqFileId),
      [getAcquisitionExpropriationEventsApi],
    ),
    requestName: 'getAcquisitionExpropriationEvents',
    onError: useAxiosErrorHandler('Failed to load Acquisition Expropriation Events'),
  });

  const getExpropriationEventById = useApiRequestWrapper<
    (
      acqFileId: number,
      eventId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ExpropriationEvent, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, eventId: number) =>
        await getAcquisitionExpropriationEventByIdApi(acqFileId, eventId),
      [getAcquisitionExpropriationEventByIdApi],
    ),
    requestName: 'getAcquisitionExpropriationEventById',
    onError: useAxiosErrorHandler('Failed to load Acquisition Expropriation Event'),
  });

  const addExpropriationEvent = useApiRequestWrapper<
    (
      acqFileId: number,
      expropriationEvent: ApiGen_Concepts_ExpropriationEvent,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ExpropriationEvent, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, expropriationEvent: ApiGen_Concepts_ExpropriationEvent) =>
        await postAcquisitionExpropriationEventApi(acqFileId, expropriationEvent),
      [postAcquisitionExpropriationEventApi],
    ),
    requestName: 'addAcquisitionExpropriationEvent',
    onError: useAxiosErrorHandler('Failed to create Acquisition Expropriation Event'),
  });

  const updateExpropriationEvent = useApiRequestWrapper<
    (
      acqFileId: number,
      expropriationEvent: ApiGen_Concepts_ExpropriationEvent,
    ) => Promise<AxiosResponse<ApiGen_Concepts_ExpropriationEvent, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, expropriationEvent: ApiGen_Concepts_ExpropriationEvent) =>
        await putAcquisitionExpropriationEventApi(acqFileId, expropriationEvent),
      [putAcquisitionExpropriationEventApi],
    ),
    requestName: 'updateAcquisitionExpropriationEvent',
    onError: useAxiosErrorHandler('Failed to update Acquisition File Agreement'),
  });

  const deleteExpropriationEvent = useApiRequestWrapper<
    (acqFileId: number, eventId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, eventId: number) =>
        await deleteAcquisitionExpropriationEventApi(acqFileId, eventId),
      [deleteAcquisitionExpropriationEventApi],
    ),
    requestName: 'deleteAcquisitionExpropriationEvent',
    onError: useAxiosErrorHandler('Failed to Delete Acquisition File Agreement'),
  });

  return useMemo(
    () => ({
      getExpropriationEvents,
      getExpropriationEventById,
      addExpropriationEvent,
      updateExpropriationEvent,
      deleteExpropriationEvent,
    }),
    [
      getExpropriationEvents,
      getExpropriationEventById,
      addExpropriationEvent,
      updateExpropriationEvent,
      deleteExpropriationEvent,
    ],
  );
};
