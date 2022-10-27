import { IInsurance } from 'interfaces';
import { IBatchUpdateReply, IBatchUpdateRequest } from 'interfaces/batchUpdate';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the insurance endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiInsurances = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      postInsuranceBatch: (leaseId: number, payload: IBatchUpdateRequest<IInsurance>) =>
        api.post<IBatchUpdateReply<IInsurance>>(`/leases/${leaseId}/insurances?batch`, payload),
    }),
    [api],
  );
};
