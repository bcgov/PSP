import React from 'react';

import { ApiGen_Concepts_ExpropriationEvent } from '@/models/api/generated/ApiGen_Concepts_ExpropriationEvent';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the acquisition expropriation event endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiExpropriationEvents = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getAcquisitionExpropriationEventsApi: (acqFileId: number) =>
        api.get<ApiGen_Concepts_ExpropriationEvent[]>(
          `/acquisitionfiles/${acqFileId}/expropriation-events`,
        ),
      getAcquisitionExpropriationEventByIdApi: (acqFileId: number, eventId: number) =>
        api.get<ApiGen_Concepts_ExpropriationEvent>(
          `/acquisitionfiles/${acqFileId}/expropriation-events/${eventId}`,
        ),
      postAcquisitionExpropriationEventApi: (
        acqFileId: number,
        expropriationEvent: ApiGen_Concepts_ExpropriationEvent,
      ) =>
        api.post<ApiGen_Concepts_ExpropriationEvent>(
          `/acquisitionfiles/${acqFileId}/expropriation-events`,
          expropriationEvent,
        ),
      putAcquisitionExpropriationEventApi: (
        acqFileId: number,
        expropriationEvent: ApiGen_Concepts_ExpropriationEvent,
      ) =>
        api.put<ApiGen_Concepts_ExpropriationEvent>(
          `/acquisitionfiles/${acqFileId}/expropriation-events/${expropriationEvent.id}`,
          expropriationEvent,
        ),
      deleteAcquisitionExpropriationEventApi: (acqFileId: number, agreementId: number) =>
        api.delete<boolean>(`/acquisitionfiles/${acqFileId}/expropriation-events/${agreementId}`),
    }),
    [api],
  );
};
