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

  const getAcquisitionExpropriationEvents = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_ExpropriationEvent[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getAcquisitionExpropriationEventsApi(acqFileId),
      [getAcquisitionExpropriationEventsApi],
    ),
    requestName: 'getAcquisitionExpropriationEvents',
    onError: useAxiosErrorHandler('Failed to load Acquisition Expropriation Events'),
  });

  const getAcquisitionExpropriationEventById = useApiRequestWrapper<
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

  const addAcquisitionExpropriationEvent = useApiRequestWrapper<
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

  const updateAcquisitionExpropriationEvent = useApiRequestWrapper<
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

  const deleteAcquisitionExpropriationEvent = useApiRequestWrapper<
    (acqFileId: number, agreementId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number, agreementId: number) =>
        await deleteAcquisitionExpropriationEventApi(acqFileId, agreementId),
      [deleteAcquisitionExpropriationEventApi],
    ),
    requestName: 'deleteAcquisitionExpropriationEvent',
    onError: useAxiosErrorHandler('Failed to Delete Acquisition File Agreement'),
  });

  return useMemo(
    () => ({
      getAcquisitionExpropriationEvents,
      getAcquisitionExpropriationEventById,
      addAcquisitionExpropriationEvent,
      updateAcquisitionExpropriationEvent,
      deleteAcquisitionExpropriationEvent,
    }),
    [
      getAcquisitionExpropriationEvents,
      getAcquisitionExpropriationEventById,
      addAcquisitionExpropriationEvent,
      updateAcquisitionExpropriationEvent,
      deleteAcquisitionExpropriationEvent,
    ],
  );
};
